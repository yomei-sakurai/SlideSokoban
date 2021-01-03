using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class Goal : FloorBase
    {
        [SerializeField] private Image image = null;
        [SerializeField] private Sprite square = null;
        [SerializeField] private Sprite circle = null;
        [SerializeField] private Sprite diamond = null;
        private const string PREFAB_PATH = "Goal";
        public override Enums.EPanelType Type => Enums.EPanelType.GOAL;
        public static Goal CreateInstance () => CreateInstanceBase<Goal> (PREFAB_PATH);

        public override void SetColor (Enums.EPanelColor color)
        {
            base.SetColor (color);
            switch (color)
            {
            case Enums.EPanelColor.RED:
                this.image.sprite = this.square;
                break;
            case Enums.EPanelColor.GREEN:
                this.image.sprite = this.circle;
                break;
            case Enums.EPanelColor.BLUE:
                this.image.sprite = this.diamond;
                break;
            }
        }
    }
}