using System;
using System.Threading;

namespace TransactionBoosting.Util
{
    public class Logger
    {
        
        public enum Mode
        {
            Console, 
            File
        }

        public enum LogLevel
        {
            Info,
            Warn,
            Error
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            if (LogLevel.Info == logLevel)
            {
                return "Info";
            }
            else if (LogLevel.Warn == logLevel)
            {
                return "Warn";
            }
            else if (LogLevel.Info == logLevel)
            {
                return "Error";
            }
            else
            {
                return "Underfined Log Level";
            }
        }

        private static string GetFinalFormatString(LogLevel logLevel, string format)
        {
            return "[" + GetLogLevelString(logLevel) + "] Thread " +
                                 Thread.CurrentThread.ManagedThreadId + "] " + format;
        }

        public static void log(LogLevel logLevel, string format, Mode mode = Mode.Console)
        {
            
            if (Mode.Console == mode)
            {
                Console.WriteLine(GetFinalFormatString(logLevel, format));
                
            }
            else if(Mode.File == mode)
            {
                
            }
        }
        
        public static void log(LogLevel logLevel, string format, object arg1, Mode mode = Mode.Console)
        {
            if (Mode.Console == mode)
            {
                Console.WriteLine(GetFinalFormatString(logLevel, format), arg1);
            }
            else if(Mode.File == mode)
            {
                
            }
        }
        
        public static void log(LogLevel logLevel, string format, object arg1, object arg2, Mode mode = Mode.Console)
        {
            if (Mode.Console == mode)
            {
                Console.WriteLine(GetFinalFormatString(logLevel, format), arg1, arg2);
            }
            else if(Mode.File == mode)
            {
                
            }
        }
        
        public static void log(LogLevel logLevel, string format, object arg1, object arg2, object arg3, Mode mode = Mode.Console)
        {
            if (Mode.Console == mode)
            {
                Console.WriteLine(GetFinalFormatString(logLevel, format), arg1, arg2, arg3);
            }
            else if(Mode.File == mode)
            {
                
            }
        }
        
    }
}