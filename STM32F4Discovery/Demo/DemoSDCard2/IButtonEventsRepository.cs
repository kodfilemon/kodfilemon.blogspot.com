namespace DemoSDCard2
{
    internal interface IButtonEventsRepository
    {
        void Add(ButtonEvent buttonEvent);
        ButtonEvent[] GetAll();
    }
}