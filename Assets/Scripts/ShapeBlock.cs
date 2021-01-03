using System;
using Assets.Scripts;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class ShapeBlock : BlockBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Sprite squareFrame = null;
        [SerializeField] private Sprite squareBack = null;
        [SerializeField] private Sprite circleFrame = null;
        [SerializeField] private Sprite circleBack = null;
        [SerializeField] private Sprite diamondFrame = null;
        [SerializeField] private Sprite diamondBack = null;
        [SerializeField] private Image baseBack = null;
        [SerializeField] private Image shapeBack = null;
        [SerializeField] private Image shapeFrame = null;

        private const float MOVE_SPEED = 10f;
        private const string PREFAB_PATH = "ShapeBlock";
        private const float DRAG_THRESHOLD = 10f;

        private const string COLOR_RED_BASE = "#FF3F3FFF";
        private const string COLOR_RED_OFF = "#7F1F1FFF";
        private const string COLOR_RED_ON = "#FF7F7FFF";
        private const string COLOR_GREEN_BASE = "#3FFF3FFF";
        private const string COLOR_GREEN_OFF = "#1F7F1FFF";
        private const string COLOR_GREEN_ON = "#7FFF7FFF";
        private const string COLOR_BLUE_BASE = "#3F3FFFFF";
        private const string COLOR_BLUE_OFF = "#1F1F7FFF";
        private const string COLOR_BLUE_ON = "#7F7FFFFF";

        private Vector2 startPosition;
        private bool isDraging = false;
        private Enums.EPanelColor color = Enums.EPanelColor.NONE;

        private Subject < (ShapeBlock, Enums.EDirection) > moveShapeBlockSubject = new Subject < (ShapeBlock, Enums.EDirection) > ();
        public IObservable < (ShapeBlock, Enums.EDirection) > MoveShapeBlockObservable => moveShapeBlockSubject;
        private Subject<Unit> checkClearSubject = new Subject<Unit> ();
        public IObservable<Unit> CheckClearObservable => checkClearSubject;
        public override Enums.EPanelType Type => Enums.EPanelType.JEWEL;
        private ReactiveProperty<bool> isMoving = null;
        public override bool IsMoving => isMoving.Value;

        private Color BaseColor
        {
            get
            {
                switch (this.color)
                {
                case Enums.EPanelColor.RED:
                    return COLOR_RED_BASE.ToColor ();
                case Enums.EPanelColor.GREEN:
                    return COLOR_GREEN_BASE.ToColor ();
                case Enums.EPanelColor.BLUE:
                    return COLOR_BLUE_BASE.ToColor ();
                }
                return default (Color);
            }
        }
        private Color ShapeColorOn
        {
            get
            {
                switch (this.color)
                {
                case Enums.EPanelColor.RED:
                    return COLOR_RED_ON.ToColor ();
                case Enums.EPanelColor.GREEN:
                    return COLOR_GREEN_ON.ToColor ();
                case Enums.EPanelColor.BLUE:
                    return COLOR_BLUE_ON.ToColor ();
                }
                return default (Color);
            }
        }
        private Color ShapeColorOff
        {
            get
            {
                switch (this.color)
                {
                case Enums.EPanelColor.RED:
                    return COLOR_RED_OFF.ToColor ();
                case Enums.EPanelColor.GREEN:
                    return COLOR_GREEN_OFF.ToColor ();
                case Enums.EPanelColor.BLUE:
                    return COLOR_BLUE_OFF.ToColor ();
                }
                return default (Color);
            }
        }

        public bool IsOnGoal
        {
            get
            {
                if (this.IsMoving) return false;
                if (this.footPanel.type != Enums.EPanelType.GOAL) return false;
                if (this.footPanel.color != Enums.EPanelColor.NONE && this.footPanel.color != this.Color) return false;
                return true;
            }
        }

        public override void Initialize ()
        {
            this.isMoving = new ReactiveProperty<bool> (false);
            this.isMoving.Subscribe (b =>
            {
                UpdateState ();
            });
        }

        public override void SetColor (Enums.EPanelColor color)
        {
            base.SetColor (color);
            this.color = color;
            switch (color)
            {
            case Enums.EPanelColor.RED:
                this.shapeBack.sprite = this.squareBack;
                this.shapeFrame.sprite = this.squareFrame;
                break;
            case Enums.EPanelColor.GREEN:
                this.shapeBack.sprite = this.circleBack;
                this.shapeFrame.sprite = this.circleFrame;
                break;
            case Enums.EPanelColor.BLUE:
                this.shapeBack.sprite = this.diamondBack;
                this.shapeFrame.sprite = this.diamondFrame;
                break;
            }

            this.baseBack.color = this.BaseColor;
            this.shapeBack.color = this.ShapeColorOff;
        }

        #region  MovePosition
        public void MoveBoardPosition (Vector2Int position) => MoveBoardPosition (position.x, position.y);
        public void MoveBoardPosition (int x, int y) => MoveLocalPosition (x * Constants.PANEL_SIZE, -y * Constants.PANEL_SIZE);
        public void MoveLocalPosition (Vector2 position) => MoveLocalPosition (position.x, position.y);
        public void MoveLocalPosition (float x, float y)
        {
            var current = this.transform.localPosition;
            var target = new Vector3 (x, y);
            var distance = Vector3.Distance (current, target) / Constants.PANEL_SIZE;
            this.isMoving.Value = true;
            this.transform.DOLocalMove (target, distance / MOVE_SPEED)
                .SetEase (Ease.OutQuad)
                .OnComplete (() =>
                {
                    this.isMoving.Value = false;
                });
        }
        #endregion

        private void UpdateState ()
        {
            if (this.shapeBack != null) this.shapeBack.color = this.IsOnGoal ? this.ShapeColorOn : this.ShapeColorOff;
            this.checkClearSubject.OnNext (Unit.Default);
        }

        public static ShapeBlock CreateInstance () => CreateInstanceBase<ShapeBlock> (PREFAB_PATH);

        public void OnBeginDrag (PointerEventData eventData)
        {
            this.startPosition = eventData.position;
            this.isDraging = true;
        }

        public void OnDrag (PointerEventData eventData)
        {
            if (!this.isDraging) return;
            var distance = eventData.position - this.startPosition;
            if (Mathf.Abs (distance.x) > DRAG_THRESHOLD || Mathf.Abs (distance.y) > DRAG_THRESHOLD)
            {
                moveShapeBlockSubject.OnNext ((this, GetDragDirection (distance)));
                this.isDraging = false;
            }
        }

        public void OnEndDrag (PointerEventData eventData) { }

        private Enums.EDirection GetDragDirection (Vector2 distance)
        {
            if (Mathf.Abs (distance.x) > Mathf.Abs (distance.y))
            {
                if (distance.x > 0)
                {
                    return Enums.EDirection.RIGHT;
                }
                else
                {
                    return Enums.EDirection.LEFT;
                }
            }
            else
            {
                if (distance.y > 0)
                {
                    return Enums.EDirection.UP;
                }
                else
                {
                    return Enums.EDirection.DOWN;
                }
            }
        }
    }
}