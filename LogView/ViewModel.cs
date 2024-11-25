using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogView.Log;
using Microsoft.Extensions.Logging;

namespace LogView;

public partial class ViewModel : ObservableObject, IObserver<LogEntry>, IDisposable
{
    private ILogger<ViewModel> logger;
    private readonly ILogEntryObservable _logObserve;
    private IDisposable? _observer;
    private object _observerLock = new object();

    [ObservableProperty]
    private ObservableCollection<LogEntry> records = new();

    public ViewModel(ILogger<ViewModel> logger, ILogEntryObservable logObserve)
    {
        this.logger = logger;
        _logObserve = logObserve;
        _observer = _logObserve.Subscribe(this);
        BindingOperations.EnableCollectionSynchronization(records, _observerLock);
    }

    [RelayCommand]
    public void AddLog()
    {
        logger.LogInformation("sss");
        logger.LogDebug("debug");
        logger.LogTrace("trace");
        logger.LogError("error");
        logger.LogWarning("warn");
    }

    public async Task<string> GetAAA()
    {
        await Task.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                logger.LogInformation($"{i}");
                Thread.Sleep(1000);
            }
        });

        return "OK";
    }

    [RelayCommand]
    public async Task GetLogAsync()
    {
        await GetAAA();
    }

    public void OnCompleted()
    {
        _observer?.Dispose();
        _observer = null;
    }

    public void OnError(Exception error)
    {
        logger.LogError(error, "");
    }

    public void OnNext(LogEntry value)
    {
        Records.Add(value);
    }

    public void Dispose()
    {
        _observer?.Dispose();
    }
}
