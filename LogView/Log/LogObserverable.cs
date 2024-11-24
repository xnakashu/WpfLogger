namespace LogView.Log;

public interface ILogEntryObservable : IObservable<LogEntry>
{
    void OnLogEntry(LogEntry logEntry);
}

public class LogObserverable : ILogEntryObservable
{
    private List<IObserver<LogEntry>> _observers;

    public LogObserverable()
    {
        _observers = new();
    }

    public IDisposable Subscribe(IObserver<LogEntry> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void OnLogEntry(LogEntry logEntry)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(logEntry);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<LogEntry>> _observers;
        private IObserver<LogEntry> _observer;

        public Unsubscriber(List<IObserver<LogEntry>> observers, IObserver<LogEntry> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
