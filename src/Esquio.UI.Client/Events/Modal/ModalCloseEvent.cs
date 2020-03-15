namespace Esquio.UI.Client.Events
{
    public class ModalCloseEvent : IEvent
    {
        public EventType Type => EventType.ModalCloseEvent;

        public static ModalCloseEvent Create() => new ModalCloseEvent();
    }
}
