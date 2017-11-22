using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the application setting service
    /// </summary>
    public interface IApplicationSettingService
    {
        List<ApplicationSetting> Get(Expression<Func<ApplicationSetting, bool>> predicate = null, string includeProperties = "");
        ApplicationSetting GetById(int id);
        ApplicationSetting GetTimeOutLength();
        void AddApplicationSetting(ApplicationSetting applicationSetting);
        void UpdateApplicationSetting(ApplicationSetting applicationSetting);
        void DeleteApplicationSetting(ApplicationSetting applicationSetting);
        void DeleteApplicationSettingById(int id);
        void Dispose();
    }
}
