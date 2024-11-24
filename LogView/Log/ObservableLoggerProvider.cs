using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LogView.Log;

/// <summary>
/// ロガープロバイダーのサンプル。<see cref="ObservableLogger"/> を生成します。
/// </summary>
[ProviderAlias("SampleLogger")]
public class ObservableLoggerProvider : ILoggerProvider
{
    private readonly ILogEntryObservable logObserver;

    /// <summary>
    /// インスタンスを生成します。
    /// </summary>
    /// <param name="options">オプション</param>
    public ObservableLoggerProvider(ILogEntryObservable logObserver)
    {
        this.logObserver = logObserver;
    }

    /// <summary>
    /// ロガーを生成します。
    /// </summary>
    /// <param name="categoryName">カテゴリー</param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName)
    {
        return new ObservableLogger(categoryName, logObserver);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
