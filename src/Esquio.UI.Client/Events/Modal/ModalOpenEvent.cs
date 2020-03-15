namespace Esquio.UI.Client.Events
{
    public class ModalOpenEvent : IEvent
    {
        public EventType Type => EventType.ModalOpenEvent;

        public static ModalOpenEvent Create() => new ModalOpenEvent();
    }
}
