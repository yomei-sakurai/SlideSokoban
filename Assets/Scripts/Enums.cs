namespace Assets.Scripts
{
    public static class Enums
    {
        public enum EDirection
        {
            UP = 0,
            DOWN = 1,
            LEFT = 2,
            RIGHT = 3,
        }
        public enum EPanelType
        {
            NONE = 0,
            WALL = 1,
            JEWEL = 2,
            GOAL = 3,
        }

        public enum EPanelColor
        {
            NONE = 0,
            RED = 1,
            GREEN = 2,
            BLUE = 3,
        }

        public enum EWallByte : byte
        {
            LEFT_TOP = 0b_0000_0001,
            CENTER_TOP = 0b_0000_0010,
            RIGHT_TOP = 0b_0000_0100,
            LEFT_CENTER = 0b_0000_1000,
            RIGHT_CENTER = 0b_0001_0000,
            LEFT_BOTTOM = 0b_0010_0000,
            CENTER_BOTTOM = 0b_0100_0000,
            RIGHT_BOTTOM = 0b_1000_0000,
        }
    }
}