namespace LightsOut
{
    public interface ISwitch
    {
        SwitchState State { get; set; }
        Position Position { get; set; }
    }
}