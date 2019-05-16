using System;
using Microsoft.Extensions.Logging;

namespace XLogger.Models
{
    public class Log<TData>
    {
        public DateTime DateTime { get; set; }
        public LogLevel LogLevel { get; set; }
        public TData Data { get; set; }
        public LogException Exception { get; set; }

        public Log(DateTime dateTime, LogLevel logLevel, TData data, Exception exception)
        {
            LogException customException = null;
            if (exception != null)
                customException = new LogException
                {
                    HResult = exception.HResult,
                    HelpLink = exception.HelpLink,
                    InnerException = new LogInnerException
                    {
                        HResult = exception.InnerException?.HResult,
                        HelpLink = exception.InnerException?.HelpLink,
                        Message = exception.InnerException?.Message,
                        Source = exception.InnerException?.Source,
                        StackTrace = exception.InnerException?.StackTrace
                    },
                    Message = exception.Message,
                    Source = exception.Source,
                    StackTrace = exception.StackTrace
                };
                
            DateTime = DateTime.Now;
            LogLevel = logLevel;
            Data = data;
            Exception = customException;
        }
    }
}