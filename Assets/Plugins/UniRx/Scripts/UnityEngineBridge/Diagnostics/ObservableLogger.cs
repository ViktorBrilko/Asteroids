using System;

namespace UniRx.Diagnostics
{
    public class ObservableLogger : IObservable<LogEntry>
    {
        private static readonly Subject<LogEntry> logPublisher = new();

        public static readonly ObservableLogger Listener = new();

        private ObservableLogger()
        {
        }

        public IDisposable Subscribe(IObserver<LogEntry> observer)
        {
            return logPublisher.Subscribe(observer);
        }

        public static Action<LogEntry> RegisterLogger(Logger logger)
        {
            if (logger.Name == null) throw new ArgumentNullException("logger.Name is null");

            return logPublisher.OnNext;
        }
    }
}