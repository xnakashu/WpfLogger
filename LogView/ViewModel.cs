﻿using System.Collections.ObjectModel;
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

    [ObservableProperty]
    private ObservableCollection<LogEntry> records = new();

    public ViewModel(ILogger<ViewModel> logger, ILogEntryObservable logObserve)
    {
        this.logger = logger;
        _logObserve = logObserve;
        _observer = _logObserve.Subscribe(this);
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

    [RelayCommand]
    public async Task<string> GetLogAsync()
    {
        await Task.Delay(5000);
        return "100";
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
