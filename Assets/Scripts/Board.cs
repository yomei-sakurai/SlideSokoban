using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class Board : BehaviourBase
    {
        [SerializeField] private Transform floorLayer = null;
        [SerializeField] private Transform blockLayer = null;
        private FloorList floorList = null;
        private BlockList blockList = null;
        private Vector2Int boardSize;
        private Subject<Unit> levelCompleteSubject = new Subject<Unit> ();
        public IObservable<Unit> LevelCompleteObservable => levelCompleteSubject;

        private bool IsBlockMoving => this.blockList != null && this.blockList.List.Any (block => block != null && block.IsMoving);

        public void SetLevel (int levelNum)
        {
            ClearBoard ();
            BoardSetUp (LoadStage (levelNum));
        }

        private void BoardSetUp (LevelData level)
        {
            this.boardSize = level.stage.size;
            this.blockList = new BlockList (this.boardSize);
            this.floorList = new FloorList (this.boardSize);
            GetComponent<RectTransform> ().sizeDelta = this.boardSize * Constants.PANEL_SIZE;
            this.boardSize.Foreach ((x, y) =>
            {
                SetPanel (x, y, level.stage[x, y]);
            });
        }

        private LevelData LoadStage (int stageNumber)
        {
            return Resources.Load<LevelData> (LevelData.GetLevelDataPath (stageNumber));
        }

        private void SetPanel (int x, int y, PanelData data)
        {
            SetFloor (x, y, data);
            SetBlock (x, y, data);
        }

        private void SetFloor (int x, int y, PanelData data)
        {
            FloorBase floorBase = null;
            switch (data.type)
            {
            case Enums.EPanelType.WALL:
                break;

            case Enums.EPanelType.JEWEL:
                break;

            case Enums.EPanelType.GOAL:
                var goal = Goal.CreateInstance ();
                floorBase = goal;
                break;
            }
            if (floorBase == null) return;

            floorBase.transform.SetParent (floorLayer);
            floorBase.transform.localScale = Vector3.one;
            floorBase.SetBoardPosition (x, y);
            floorBase.SetColor (data.color);
            this.floorList[x, y] = floorBase;
        }

        private void SetBlock (int x, int y, PanelData data)
        {
            BlockBase blockBase = null;
            switch (data.type)
            {
            case Enums.EPanelType.WALL:
                var wall = Wall.CreateInstance ();
                wall.SetPattern (data.adjacentWallPattern);
                blockBase = wall;
                break;

            case Enums.EPanelType.JEWEL:
                var jewel = ShapeBlock.CreateInstance ();
                jewel.MoveShapeBlockObservable.Subscribe (t => MoveShapeBlock (t.Item1, t.Item2));
                jewel.CheckClearObservable.Subscribe (unit => CheckLevelComplete ());
                blockBase = jewel;
                break;

            case Enums.EPanelType.GOAL:
                break;
            }
            if (blockBase == null) return;

            blockBase.transform.SetParent (blockLayer);
            blockBase.transform.localScale = Vector3.one;
            blockBase.SetBoardPosition (x, y);
            blockBase.SetColor (data.color);
            blockBase.SetFootType (this.floorList[x, y]);
            this.blockList[x, y] = blockBase;
        }

        private void MoveShapeBlock (ShapeBlock target, Enums.EDirection direction)
        {
            if (IsBlockMoving) return;

            var targetPos = blockList.GetPosition (target);
            var pos = targetPos;
            var stageSize = this.blockList.size;
            switch (direction)
            {
            case Enums.EDirection.UP:
                do
                {
                    var next = pos.Up ();
                    if (!this.blockList[next].IsEmpty ()) break;
                    pos = next;
                } while (pos.y > 0);
                break;
            case Enums.EDirection.DOWN:
                do
                {
                    var next = pos.Down ();
                    if (!this.blockList[next].IsEmpty ()) break;
                    pos = next;
                } while (pos.y < stageSize.y - 1);
                break;
            case Enums.EDirection.LEFT:
                do
                {
                    var next = pos.Left ();
                    if (!this.blockList[next].IsEmpty ()) break;
                    pos = next;
                } while (pos.x > 0);
                break;
            case Enums.EDirection.RIGHT:
                do
                {
                    var next = pos.Right ();
                    if (!this.blockList[next].IsEmpty ()) break;
                    pos = next;
                } while (pos.x < stageSize.x - 1);
                break;
            }
            if (targetPos == pos) return;
            target.MoveBoardPosition (pos);
            this.blockList.Exchange (targetPos, pos);
            target.SetFootType (this.floorList[pos]);
        }

        private void CheckLevelComplete ()
        {
            if (this.blockList.ShapeBlockList.All (b => b.IsOnGoal))
            {
                Debug.Log ("Complete!");
                levelCompleteSubject.OnNext (Unit.Default);
            }
        }

        private void OnDestroy ()
        {
            blockList = null;
        }

        private void ClearBoard ()
        {
            foreach (Transform b in this.blockLayer)
            {
                Destroy (b.gameObject);
            }

            foreach (Transform f in this.floorLayer)
            {
                Destroy (f.gameObject);
            }

            this.blockList = null;
            this.floorList = null;
        }

        private sealed class BlockList : TwoDimensionalList<BlockBase>
        {
            public BlockList (int x, int y) : base (x, y) { }
            public BlockList (Vector2Int vector) : base (vector) { }
            public IEnumerable<ShapeBlock> ShapeBlockList => this.list.Where (b => b != null && b.Type == Enums.EPanelType.JEWEL).Select (b => b as ShapeBlock);
        }

        private sealed class FloorList : TwoDimensionalList<FloorBase>
        {
            public FloorList (int x, int y) : base (x, y) { }
            public FloorList (Vector2Int vector) : this (vector.x, vector.y) { }
        }
    }
}