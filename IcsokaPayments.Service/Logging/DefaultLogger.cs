using System;
using System.Collections.Generic;
using System.Linq;
using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Data.Repository;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Domain.Enums;
using IcsokaPayments.Service.Helpers;

namespace IcsokaPayments.Service.Logging
{
    public class DefaultLogger:ILogger
    {
        #region Fields

        private readonly IWebHelper _webHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
       

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logRepository">Log repository</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="unitOfWork"></param>
        public DefaultLogger(ILogRepository logRepository,
            IWebHelper webHelper,
            IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _webHelper = webHelper;
            _unitOfWork = unitOfWork;
           
        }

        #endregion

        #region Utitilities
   

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public virtual void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            _logRepository.Delete(log);
            Save();
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public virtual void ClearLog()
        {
            var log = _logRepository.GetAll().ToList();
            _logRepository.Delete(log);
               
            Save();
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
            var query = _logRepository.GetAll();
            if (fromUtc.HasValue)
                query = query.Where(l => fromUtc.Value <= l.CreatedOnUtc);
            if (toUtc.HasValue)
                query = query.Where(l => toUtc.Value >= l.CreatedOnUtc);
            if (logLevel.HasValue)
            {
                var logLevelId = (int)logLevel.Value;
                query = query.Where(l => logLevelId == l.LogLevelId);
            }
            if (!String.IsNullOrEmpty(message))
                query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));
            query = query.OrderByDescending(l => l.CreatedOnUtc);

            var log = new PagedResult<Log>(query.AsQueryable(), pageIndex, pageSize);
            return log;
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;

            return _logRepository.GetById(logId);
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<Log> GetLogByIds(int[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();

            var query = from l in _logRepository.GetAll()
                        where logIds.Contains(l.Id)
                        select l;
            var logItems = query.ToList();
            //sort by passed identifiers
            var sortedLogItems = new List<Log>();
            foreach (int id in logIds)
            {
                var log = logItems.Find(x => x.Id == id);
                if (log != null)
                    sortedLogItems.Add(log);
            }
            return sortedLogItems;
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
         
            
            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = _webHelper.GetCurrentIpAddress(),
                User = user,
                PageUrl = _webHelper.GetThisPageUrl(true),
                ReferrerUrl = _webHelper.GetUrlReferrer(),
                CreatedOnUtc = DateTime.UtcNow
            };

            _logRepository.Add(log);
            Save();
            return log;
        }

        public void ClearLog(DateTime toUtc, LogLevel? logLevel = null)
        {
            var log = _logRepository.GetMany(x => x.CreatedOnUtc <= toUtc);
            if (logLevel.HasValue)
            {
                log = log.Where(x => x.LogLevel == logLevel);
            }
            _logRepository.Delete(log);
            Save();
        }

        private void Save()
        {
            _unitOfWork.Commit();
        }

        #endregion
    }
}