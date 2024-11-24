using Microsoft.Extensions.Logging;

namespace LogView;

/// <summary>
/// ログ.
/// </summary>
/// <param name="LogLevel">ログレベル</param>
/// <param name="DateTime">発生日時</param>
/// <param name="Message">メッセージ</param>
public record LogEntry(LogLevel LogLevel, string DateTime, string Message);
