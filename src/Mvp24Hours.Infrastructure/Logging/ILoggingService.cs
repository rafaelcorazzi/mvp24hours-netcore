//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;

namespace Mvp24Hours.Infrastructure.Logging
{
    /// <summary>
    /// Provides logging interface and utility functions.
    /// </summary>
    public interface ILoggingService
    {
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsTraceEnabled { get; }
        bool IsWarnEnabled { get; }

        void Debug(Exception exception);
        void Debug(string format, params object[] args);
        void Debug(Exception exception, string format, params object[] args);
        void Error(Exception exception);
        void Error(string format, params object[] args);
        void Error(Exception exception, string format, params object[] args);
        void Fatal(Exception exception);
        void Fatal(string format, params object[] args);
        void Fatal(Exception exception, string format, params object[] args);
        void Info(Exception exception);
        void Info(string format, params object[] args);
        void Info(Exception exception, string format, params object[] args);
        void Trace(Exception exception);
        void Trace(string format, params object[] args);
        void Trace(Exception exception, string format, params object[] args);
        void Warn(Exception exception);
        void Warn(string format, params object[] args);
        void Warn(Exception exception, string format, params object[] args);
    }
}
