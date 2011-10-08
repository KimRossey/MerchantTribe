using System;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Data;
using log4net;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce
{
    public class EventLog: MerchantTribe.Web.Logging.ILogger
    {

        public EventLog()
        {

        }

        /// <summary>
        /// Records and event to the BV Commerce event log
        /// </summary>
        /// <param name="source">The source of the event</param>
        /// <param name="message">The description or information about the event</param>
        /// <returns>True if the event was recorded, otherwise false</returns>
        public static bool LogEvent(string source, string message, EventLogSeverity severity)
        {
            ILog Log = LogManager.GetLogger(source);

            switch (severity)
            {
                case EventLogSeverity.Debug:
                    Log.Debug(message);
                    break;
                case EventLogSeverity.Error:
                    Log.Error(message);
                    break;
                case EventLogSeverity.Fatal:
                    Log.Error(message);
                    break;
                case EventLogSeverity.Information:
                    Log.Info(message);
                    break;
                case EventLogSeverity.None:
                    Log.Info(message);
                    break;
                case EventLogSeverity.Warning:
                    Log.Warn(message);
                    break;
            }

            return true;    
        }

        /// <summary>
        /// Logs an exception to the BV Commerce log
        /// </summary>
        /// <param name="ex">Exception to be recorded</param>
        /// <returns>True if the exception was recorded, otherwise false</returns>
        public static bool LogEvent(Exception ex)
        {
            return LogEvent(ex, EventLogSeverity.Error);            
        }
        public static bool LogEvent(Exception ex, EventLogSeverity severity)
        {
            ILog Log = LogManager.GetLogger("MerchantTribe.System");
            switch (severity)
            {
                case EventLogSeverity.Debug:
                    Log.Debug(ex);
                    break;
                case EventLogSeverity.Error:
                    Log.Error(ex);
                    break;
                case EventLogSeverity.Fatal:
                    Log.Error(ex);
                    break;
                case EventLogSeverity.Information:
                    Log.Info(ex);
                    break;
                case EventLogSeverity.None:
                    Log.Info(ex);
                    break;
                case EventLogSeverity.Warning:
                    Log.Warn(ex);
                    break;
            }
            return true;
        }
                            

        #region ILogger Members

        public void LogMessage(string message)
        {
            EventLog.LogEvent("Logger STORE 0", message, EventLogSeverity.Information);
        }

        public void LogException(Exception ex)
        {
            EventLog.LogEvent(ex);
        }
        
        public void LogException(Exception ex, EventLogSeverity severity)
        {
            EventLog.LogEvent(ex, severity);            
        }

        public void LogMessage(string source, string message, EventLogSeverity severity)
        {
            EventLog.LogEvent(source, message, severity);
        }

        #endregion
    }

}