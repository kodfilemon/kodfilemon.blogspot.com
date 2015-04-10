namespace DemoDS18B20
{
    public interface IDisplay
    {
        void ShowTemperature(float temperature);
        void ShowError(string message);
    }
}