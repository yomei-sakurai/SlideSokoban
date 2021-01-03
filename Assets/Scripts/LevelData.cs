using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts
{

    [CreateAssetMenu (fileName = "LevelData", menuName = "SlideSokoban/LevelData", order = 0)]
    public sealed class LevelData : ScriptableObject
    {
        public static readonly string LEVEL_DATA_DIR_PATH = "Level/";
        public enum EDataType
        {
            NONE = 0,
            WALL = 1,
            BOX_NORMAL = 10,
            BOX_RED = 11,
            BOX_GREEN = 12,
            BOX_BLUE = 13,
            GOAL_NORMAL = 20,
            GOAL_RED = 21,
            GOAL_GREEN = 22,
            GOAL_BLUE = 23,
        }
        public int number;
        public StageData stage;

        public static string GetLevelDataPath (int num) => string.Format ("{0}{1:00}", LEVEL_DATA_DIR_PATH, num);

        public LevelData Clone ()
        {
            var clone = (LevelData) MemberwiseClone ();
            clone.stage = (StageData) this.stage.Clone ();
            return clone;
        }

        public LevelData (int number)
        {
            this.number = number;
            this.stage = new StageData ();
        }

        public void UpdateLevel (LevelData level)
        {
            this.number = level.number;
            this.stage = (StageData) level.stage.Clone ();
        }

        public void ResetLevel ()
        {
            this.stage.Initialize ();
        }
    }

    [System.Serializable]
    public struct PanelData
    {
        public Enums.EPanelType type;
        public Enums.EPanelColor color;
        public byte adjacentWallPattern;
        public PanelData (Enums.EPanelType type, Enums.EPanelColor color)
        {
            this.type = type;
            this.color = color;
            this.adjacentWallPattern = 0;
        }
    }

    [System.Serializable]
    public sealed class StageData : TwoDimensionalList<PanelData>
    {
        public StageData () : base () { }
        public StageData (int x, int y) : base (x, y) { }
        public StageData (Vector2Int vector) : base (vector) { }
        public override void Initialize ()
        {
            base.Initialize ();
            for (int x = 0; x < this.size.x; ++x)
            {
                for (int y = 0; y < this.size.y; ++y)
                {
                    if (x == 0 || x == this.size.x - 1 || y == 0 || y == this.size.y - 1)
                    {
                        this [x, y] = new PanelData (Enums.EPanelType.WALL, Enums.EPanelColor.NONE);
                    }
                }
            }
        }

        public byte GetAdjacentWallPattern (int x, int y)
        {
            byte pattern = 0;
            for (int i = 0; i < 8; ++i)
            {
                var tmpI = i + i / 4;

                var tmpX = x + (tmpI % 3) - 1;
                var tmpY = y + (tmpI / 3) - 1;

                if (!CheckSize (tmpX, tmpY)) continue;
                if (this [tmpX, tmpY].type != Enums.EPanelType.WALL) continue;

                pattern |= (byte) (1 << i);
            }

            return pattern;
        }
    }
}