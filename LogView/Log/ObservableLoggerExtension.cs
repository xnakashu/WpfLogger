using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace LogView.Log;

public static class ObservableLoggerExtension
{
    public static ILoggingBuilder AddObservable(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        var provider = ServiceDescriptor.Singleton<ILoggerProvider, ObservableLoggerProvider>();
        builder.Services.Add(provider);

        //// プロバイダーとオプションを関連付けます
        LoggerProviderOptions.RegisterProviderOptions<
            ObservableLoggerOptions,
            ObservableLoggerProvider
        >(builder.Services);
        return builder;
    }
}
