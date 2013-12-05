using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TradesWebApplication.Logger;


namespace TradesWebApplication.DAL
{
    public class TradesInterceptorLogging : DbCommandInterceptor
    {
        private ILogger _logger = new Logger.Logger();

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            base.ScalarExecuting(command, interceptionContext);
            timespan.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "SchoolInterceptor.ScalarExecuting", timespan.Elapsed, "Command: {0}: ", command.CommandText);
            }
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            base.NonQueryExecuting(command, interceptionContext);
            timespan.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "SchoolInterceptor.NonQueryExecuting", timespan.Elapsed, "Command: {0}: ", command.CommandText);
            }
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            Stopwatch timespan = Stopwatch.StartNew();
            base.ReaderExecuting(command, interceptionContext);
            timespan.Stop();
            if (interceptionContext.Exception != null)
            {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQL Database", "SchoolInterceptor.ReaderExecuting", timespan.Elapsed, "Command: {0}: ", command.CommandText);
            }
        }
    }
}