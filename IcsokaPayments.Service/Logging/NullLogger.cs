﻿using System;
using System.Collections.Generic;
using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Domain.Enums;

namespace IcsokaPayments.Service.Logging
{
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            return false;
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public virtual void DeleteLog(Log log)
        {
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public virtual void ClearLog()
        {
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">Log item creation from; null to load all records</param>
        /// <param name="toUtc">Log item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log level; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Log item collection</returns>
        public virtual IPagedList<Log> GetAllLogs(DateTime? fromUtc, DateTime? toUtc,
            string message, LogLevel? logLevel, int pageIndex, int pageSize)
        {
            return new PagedResult<Log>(new List<Log>(), pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual Log GetLogById(int logId)
        {
            return null;
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<Log> GetLogByIds(int[] logIds)
        {
            return new List<Log>();
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="user">The user to associate log record with</param>
        /// <returns>A log item</returns>
        public virtual Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", User user = null)
        {
            return null;
        }

        public void ClearLog(DateTime toUtc, LogLevel? logLevel = null)
        {
            
        }
    }
}
