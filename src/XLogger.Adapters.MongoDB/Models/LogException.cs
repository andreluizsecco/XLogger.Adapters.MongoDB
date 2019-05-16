namespace XLogger.Models
{
    public class LogException
    {
        public int HResult { get; set; }
        public string HelpLink { get; set; }
        public LogInnerException InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }

    public class LogInnerException
    {
        public int? HResult { get; set; }
        public string HelpLink { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}