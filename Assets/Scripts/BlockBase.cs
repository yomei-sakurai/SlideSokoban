namespace Assets.Scripts
{
    public abstract class BlockBase : PanelBase
    {
        public virtual bool IsMoving => false;

        protected PanelData footPanel;
        public void SetFootType (IPanel panel)
        {
            if (panel == null)
            {
                footPanel.type = Enums.EPanelType.NONE;
                footPanel.color = Enums.EPanelColor.NONE;
            }
            else
            {
                footPanel.type = panel.Type;
                footPanel.color = panel.Color;
            }
        }
    }
}