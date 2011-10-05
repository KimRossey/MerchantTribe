using System;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Data;
using log4net;

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
        public static bool LogEvent(string source, string message, Metrics.EventLogSeverity severity)
        {
            ILog Log = LogManager.GetLogger(source);

            switch (severity)
            {
                case Metrics.EventLogSeverity.Debug:
                    Log.Debug(message);
                    break;
                case Metrics.EventLogSeverity.Error:
                    Log.Error(message);
                    break;
                case Metrics.EventLogSeverity.Fatal:
                    Log.Error(message);
                    break;
                case Metrics.EventLogSeverity.Information:
                    Log.Info(message);
                    break;
                case Metrics.EventLogSeverity.None:
                    Log.Info(message);
                    break;
                case Metrics.EventLogSeverity.Warning:
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
            ILog Log = LogManager.GetLogger("MerchantTribe.System");
            Log.Error("Exception", ex);
            return true;
  
        }
                            

        #region ILogger Members

        public void LogMessage(string message)
        {
            EventLog.LogEvent("Logger STORE 0", message, Metrics.EventLogSeverity.Information);
        }

        public void LogException(Exception ex)
        {
            EventLog.LogEvent(ex);
        }

        #endregion
    }

}