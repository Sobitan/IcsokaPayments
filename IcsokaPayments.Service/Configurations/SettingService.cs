using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Data.Repository;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Service.Caching;
using IcsokaPayments.Service.Helpers;
using IcsokaPayments.Service.Payments;

namespace IcsokaPayments.Service.Configurations
{
    public class SettingService:ISettingService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const string SETTINGS_ALL_KEY = "vat.setting.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const string SETTINGS_PATTERN_KEY = "vat.setting.";

        #endregion

        #region Fields

        private readonly ISettingRepository _settingRepository;

        private readonly ICacheManager _cacheManager;

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="settingRepository">Setting repository</param>
        /// <param name="unitOfWork"></param>
        public SettingService(ICacheManager cacheManager,
             ISettingRepository settingRepository, IUnitOfWork unitOfWork)
        {
            _cacheManager = cacheManager;

            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            //cache
            string key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
                var query = from s in _settingRepository.GetAll()
                            orderby s.Name
                            select s;
                var settings = query.ToList();
                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value,
                       
                    };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        //already added
                        //most probably it's the setting with the same name but for some certain store (institutionId > 0)
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }
                return dictionary;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Add(setting);
            Save();
            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);


        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Update(setting);
            Save();
            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);


        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        public virtual void DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Delete(setting);
            Save();
            //cache
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

        }

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.GetById(settingId);
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="institutionId">Institution identifier</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSetting(string key, int institutionId = 0, bool loadSharedValueIfNotFound = false)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault();

                //load shared value?
                if (setting == null && institutionId > 0 && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault();

                if (setting != null)
                    return GetSettingById(setting.Id);
            }

            return null;
        }

        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="institutionId">Institution identifier</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting value</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T),
            int institutionId = 0, bool loadSharedValueIfNotFound = false)
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault();

                //load shared value?
                if (setting == null && institutionId > 0 && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault();

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="institutionId">Institution identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SetSetting<T>(string key, T value, int institutionId = 0, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            key = key.Trim().ToLowerInvariant();
            string valueStr = CommonHelper.GetCustomTypeConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr,
                   
                };
                InsertSetting(setting, clearCache);
            }
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        public virtual IList<Setting> GetAllSettings()
        {
            var query = from s in _settingRepository.GetAll()
                        orderby s.Name
                        select s;
            var settings = query.ToList();
            return settings;
        }

        /// <summary>
        /// Determines whether a setting exists
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="institutionId">Institution identifier</param>
        /// <returns>true -setting exists; false - does not exist</returns>
        public virtual bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int institutionId = 0)
            where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);

            var setting = GetSettingByKey<string>(key, institutionId: institutionId);
            return setting != null;
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="institutionId">Institution identifier for which settigns should be loaded</param>
        public virtual T LoadSetting<T>(int institutionId = 0) where T : ISettings, new()
        {
            var settings = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //load by store
                var setting = GetSettingByKey<string>(key, institutionId: institutionId, loadSharedValueIfNotFound: true);
                if (setting == null)
                    continue;

                if (!CommonHelper.GetCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!CommonHelper.GetCustomTypeConverter(prop.PropertyType).IsValid(setting))
                    continue;

                object value = CommonHelper.GetCustomTypeConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings;
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="institutionId">Institution identifier</param>
        /// <param name="settings">Setting instance</param>
        public virtual void SaveSetting<T>(T settings, int institutionId = 0) where T : ISettings, new()
        {
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!CommonHelper.GetCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                string key = typeof(T).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(settings, null);
                SetSetting(key, value ?? "", institutionId, false);
            }

            //and now clear cache
            ClearCache();
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="institutionId">Institution ID</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            int institutionId = 0, bool clearCache = true) where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            string key = settings.GetSettingKey(keySelector);
            //Duck typing is not supported in C#. That's why we're using dynamic type
            dynamic value = propInfo.GetValue(settings, null);
            SetSetting(key, value ?? "", institutionId, clearCache);
        }

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            foreach (var setting in settingsToDelete)
                DeleteSetting(setting);
        }

        /// <summary>
        /// Delete settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="institutionId">Institution ID</param>
        public virtual void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int institutionId = 0) where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching == null) return;
            //update
            var setting = GetSettingById(settingForCaching.Id);
            DeleteSetting(setting);
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        private void Save()
        {
            _unitOfWork.Commit();
        }
        #endregion
    }
}
