using Esquio.UI.Client.Events;
using System;
using System.Reactive.Subjects;

namespace Esquio.UI.Client.Services
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
        IDisposable Subscribe(IObserver<IEvent> observer);
        IDisposable Subscribe(Action<IEvent> onNext);
    }

    public class EventBus : IEventBus
    {
        private static Subject<IEvent> events = new Subject<IEvent>();

        public void Publish(IEvent @event) => events.OnNext(@event);
        public IDisposable Subscribe(IObserver<IEvent> observer)
            => events.Subscribe(observer);
        public IDisposable Subscribe(Action<IEvent> onNext)
            => Subscribe(new EventObserver(onNext: onNext));
    }

    public class EventObserver : IObserver<IEvent>
    {
        private readonly Action onCompleted;
        private readonly Action<Exception> onError;
        private readonly Action<IEvent> onNext;

        public EventObserver(
            Action onCompleted = null,
            Action<Exception> onError = null,
            Action<IEvent> onNext = null)
        {
            this.onCompleted = onCompleted;
            this.onError = onError;
            this.onNext = onNext;
        }

        public void OnCompleted() => onCompleted?.Invoke();

        public void OnError(Exception error) => onError?.Invoke(error);

        public void OnNext(IEvent value) => onNext?.Invoke(value);
    }
}
