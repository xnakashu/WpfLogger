using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LogView.Log;

public class ObservableLoggerOptions { }

/// <summary>
/// </summary>
internal class ObservableLogger : ILogger
{
    private readonly ILogEntryObservable _logObserver;
    private readonly string _categoryName;

    /// <summary>
    /// インスタンスを生成します。
    /// </summary>
    /// <param name="categoryName">カテゴリー</param>
    /// <param name="options">オプション</param>
    public ObservableLogger(string categoryName, ILogEntryObservable logObserver)
    {
        this._categoryName = categoryName;
        this._logObserver = logObserver;
    }

    /// <inheritdoc/>>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return new Scope<TState>(state);
    }

    /// <inheritdoc/>>
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    /// <inheritdoc/>>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        try
        {
            string msg = formatter(state, exception);
            var date = DateTime.Now;
            _logObserver.OnLogEntry(new LogEntry(logLevel, date.ToString("HH:mm:ss.fff "), msg));
        }
        finally
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    /// <inheritdoc/>>
    private class Scope<TState> : IDisposable
    {
        internal Scope(TState state)
        {
            State = state;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public TState State { get; }
    }
}
