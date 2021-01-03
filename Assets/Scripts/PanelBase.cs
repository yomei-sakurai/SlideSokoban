using UnityEngine;

namespace Assets.Scripts
{
    public abstract partial class PanelBase : BehaviourBase, IPanel
    {

        public virtual Enums.EPanelType Type => Enums.EPanelType.NONE;
        public virtual Enums.EPanelColor Color
        {
            get;
            protected set;
        }

        public virtual void SetColor (Enums.EPanelColor color)
        {
            this.Color = color;
        }

        public int Size
        {
            get { return Constants.PANEL_SIZE; }
        }

        public PanelBase ()
        {
            Initialize ();
        }

        #region SetPosition
        public void SetBoardPosition (Vector2Int position) => SetBoardPosition (position.x, position.y);
        public void SetBoardPosition (int x, int y) => SetLocalPosition (x * Constants.PANEL_SIZE, -y * Constants.PANEL_SIZE);
        public void SetLocalPosition (Vector2 position) => SetLocalPosition (position.x, position.y);
        public void SetLocalPosition (float x, float y) => this.transform.localPosition = new Vector3 (x, y);
        #endregion

        protected static T CreateInstanceBase<T> (string path) where T : PanelBase
        {
            var obj = Instantiate (Resources.Load<GameObject> (path));
            var instance = obj.GetComponent<T> ();
            return instance;
        }
    }

    public static class PanelBaseExtensions
    {
        public static bool IsEmpty (this PanelBase panel)
        {
            if (panel == null) return true;
            return panel.Type == Enums.EPanelType.NONE;
        }

        public static Enums.EPanelType GetType (this PanelBase panel)
        {
            if (panel == null) return Enums.EPanelType.NONE;
            return panel.Type;
        }
    }
}