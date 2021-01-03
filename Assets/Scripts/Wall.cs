using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class Wall : BlockBase
    {
        [SerializeField] private WallSprites wallSprites = null;
        [SerializeField] private Image leftTop = null;
        [SerializeField] private Image rightTop = null;
        [SerializeField] private Image leftBottom = null;
        [SerializeField] private Image rightBottom = null;
        private const string PREFAB_PATH = "Wall";
        public override Enums.EPanelType Type => Enums.EPanelType.WALL;
        public override Enums.EPanelColor Color => Enums.EPanelColor.NONE;
        public static Wall CreateInstance () => CreateInstanceBase<Wall> (PREFAB_PATH);
        public void SetPattern (byte wallPattern)
        {
            //LEFT_TOP
            if (CheckType (wallPattern, (byte) (Enums.EWallByte.LEFT_TOP | Enums.EWallByte.CENTER_TOP | Enums.EWallByte.LEFT_CENTER)))
            {
                this.leftTop.sprite = this.wallSprites[WallSprites.EWallType.CENTER];
            }
            else if (CheckType (wallPattern, (byte) (Enums.EWallByte.CENTER_TOP | Enums.EWallByte.LEFT_CENTER)))
            {
                this.leftTop.sprite = this.wallSprites[WallSprites.EWallType.RIGHT_BOTTOM_IN];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.CENTER_TOP))
            {
                this.leftTop.sprite = this.wallSprites[WallSprites.EWallType.LEFT];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.LEFT_CENTER))
            {
                this.leftTop.sprite = this.wallSprites[WallSprites.EWallType.TOP];
            }
            else
            {
                this.leftTop.sprite = this.wallSprites[WallSprites.EWallType.LEFT_TOP_OUT];
            }

            //RIGHT_TOP
            if (CheckType (wallPattern, (byte) (Enums.EWallByte.RIGHT_TOP | Enums.EWallByte.CENTER_TOP | Enums.EWallByte.RIGHT_CENTER)))
            {
                this.rightTop.sprite = this.wallSprites[WallSprites.EWallType.CENTER];
            }
            else if (CheckType (wallPattern, (byte) (Enums.EWallByte.CENTER_TOP | Enums.EWallByte.RIGHT_CENTER)))
            {
                this.rightTop.sprite = this.wallSprites[WallSprites.EWallType.LEFT_BOTTOM_IN];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.CENTER_TOP))
            {
                this.rightTop.sprite = this.wallSprites[WallSprites.EWallType.RIGHT];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.RIGHT_CENTER))
            {
                this.rightTop.sprite = this.wallSprites[WallSprites.EWallType.TOP];
            }
            else
            {
                this.rightTop.sprite = this.wallSprites[WallSprites.EWallType.RIGHT_TOP_OUT];
            }

            //LEFT_BOTTOM
            if (CheckType (wallPattern, (byte) (Enums.EWallByte.LEFT_BOTTOM | Enums.EWallByte.CENTER_BOTTOM | Enums.EWallByte.LEFT_CENTER)))
            {
                this.leftBottom.sprite = this.wallSprites[WallSprites.EWallType.CENTER];
            }
            else if (CheckType (wallPattern, (byte) (Enums.EWallByte.CENTER_BOTTOM | Enums.EWallByte.LEFT_CENTER)))
            {
                this.leftBottom.sprite = this.wallSprites[WallSprites.EWallType.RIGHT_TOP_IN];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.CENTER_BOTTOM))
            {
                this.leftBottom.sprite = this.wallSprites[WallSprites.EWallType.LEFT];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.LEFT_CENTER))
            {
                this.leftBottom.sprite = this.wallSprites[WallSprites.EWallType.BOTTOM];
            }
            else
            {
                this.leftBottom.sprite = this.wallSprites[WallSprites.EWallType.LEFT_BOTTOM_OUT];
            }

            //RIGHT_BOTTOM
            if (CheckType (wallPattern, (byte) (Enums.EWallByte.RIGHT_BOTTOM | Enums.EWallByte.CENTER_BOTTOM | Enums.EWallByte.RIGHT_CENTER)))
            {
                this.rightBottom.sprite = this.wallSprites[WallSprites.EWallType.CENTER];
            }
            else if (CheckType (wallPattern, (byte) (Enums.EWallByte.CENTER_BOTTOM | Enums.EWallByte.RIGHT_CENTER)))
            {
                this.rightBottom.sprite = this.wallSprites[WallSprites.EWallType.LEFT_TOP_IN];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.CENTER_BOTTOM))
            {
                this.rightBottom.sprite = this.wallSprites[WallSprites.EWallType.RIGHT];
            }
            else if (CheckType (wallPattern, (byte) Enums.EWallByte.RIGHT_CENTER))
            {
                this.rightBottom.sprite = this.wallSprites[WallSprites.EWallType.BOTTOM];
            }
            else
            {
                this.rightBottom.sprite = this.wallSprites[WallSprites.EWallType.RIGHT_BOTTOM_OUT];
            }
        }

        private bool CheckType (byte wallPattern, byte target)
        {
            var tmp = wallPattern & target;
            return tmp == target;
        }
    }
}