using UnityEngine;

[CreateAssetMenu (fileName = "WallSprites", menuName = "SlideSokoban/WallSprites", order = 0)]
public sealed class WallSprites : ScriptableObject
{
    public enum EWallType
    {
        LEFT_TOP_OUT = 0,
        TOP = 1,
        RIGHT_TOP_OUT = 2,
        LEFT = 3,
        CENTER = 4,
        RIGHT = 5,
        LEFT_BOTTOM_OUT = 6,
        BOTTOM = 7,
        RIGHT_BOTTOM_OUT = 8,
        LEFT_TOP_IN = 9,
        RIGHT_TOP_IN = 10,
        LEFT_BOTTOM_IN = 11,
        RIGHT_BOTTOM_IN = 12,

    }
    public Sprite this [EWallType index]
    {
        get => this.value[(int) index];
    }

    [SerializeField] private Sprite[] value = null;
}