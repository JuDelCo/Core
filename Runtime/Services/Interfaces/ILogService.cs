
namespace Ju
{
public delegate void LogServiceEvent(string message, string timeStamp, params object[] args);
public delegate void LogGeneralServiceEvent(string message, string timeStamp, LogLevel logLevel, params object[] args);

public enum LogLevel
{
	Debug = 0,
	Info = 1,
	Notice = 2,
	Warning = 3,
	Error = 4
}

public interface ILogService
{
	event LogServiceEvent OnDebugMessage;
	event LogServiceEvent OnInfoMessage;
	event LogServiceEvent OnNoticeMessage;
	event LogServiceEvent OnWarningMessage;
	event LogServiceEvent OnErrorMessage;
	event LogGeneralServiceEvent OnAnyMessage;

	void SetLogLevel(LogLevel logLevel);
	void SuscribeLoggable(ILoggableService loggable, LogLevel logLevel = LogLevel.Debug);

	void Debug(string message, params object[] args);
	void Info(string message, params object[] args);
	void Notice(string message, params object[] args);
	void Warning(string message, params object[] args);
	void Error(string message, params object[] args);
}
}
