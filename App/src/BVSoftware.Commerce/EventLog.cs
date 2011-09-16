using System;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Data;

namespace BVSoftware.Commerce
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
            bool ret = false;
            Metrics.EventLogEntry e = new Metrics.EventLogEntry(source, message, severity);

            RequestContext context = null;
            try
            {
                context = RequestContext.GetCurrentRequestContext();
            }
            catch
            {
                // nothing
            }

            Metrics.EventLogRepository repository = new Metrics.EventLogRepository(context);
            ret = repository.Create(e);
            
            return ret;
        }

        /// <summary>
        /// Logs an exception to the BV Commerce log
        /// </summary>
        /// <param name="ex">Exception to be recorded</param>
        /// <returns>True if the exception was recorded, otherwise false</returns>
        public static bool LogEvent(Exception ex)
        {
            bool ret = false;
            Metrics.EventLogEntry e = new Metrics.EventLogEntry(ex);

            RequestContext context = null;
            try
            {
                context = RequestContext.GetCurrentRequestContext();
            }
            catch
            {
                // nothing
                context = new RequestContext();                
            }

            if (context.ConnectionString == string.Empty)
            {
                context.ConnectionString = "server=.;database=bvcommerce;uid=novaliduser;pwd=nopassword";
                context.ConnectionStringForEntityFramework = "metadata=res://*/Data.EF.Models.csdl|res://*/Data.EF.Models.ssdl|res://*/Data.EF.Models.msl;provider=System.Data.SqlClient;provider connection string=\"server=.;database=bvcommerce;uid=novaliduser;pwd=nopassword;MultipleActiveResultSets=True\";";
            }

            Metrics.EventLogRepository repository = new Metrics.EventLogRepository(context);
            ret = repository.Create(e);
            return ret;
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