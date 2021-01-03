namespace Assets.Scripts
{
    public interface IPanel
    {
        Enums.EPanelType Type { get; }
        Enums.EPanelColor Color { get; }
    }
}