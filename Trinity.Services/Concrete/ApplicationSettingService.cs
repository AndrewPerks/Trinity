using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trinity.DataAccess.Interfaces;
using Trinity.Model;
using Trinity.Services.Interfaces;

namespace Trinity.Services.Concrete
{
    /// <summary>
    /// Application setting service that carries out the business transactions involving application settings
    /// </summary>
    public class ApplicationSettingService : IApplicationSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationSettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ApplicationSetting> Get(Expression<Func<ApplicationSetting, bool>> predicate = null, string includeProperties = "")
        {
            var applicationSettings = _unitOfWork.Repository<ApplicationSetting>().Get(predicate, includeProperties);
            return applicationSettings.ToList();
        }

        public ApplicationSetting GetById(int id)
        {
            var applicationSetting = _unitOfWork.Repository<ApplicationSetting>().GetById(id);
            return applicationSetting;
        }

        public ApplicationSetting GetTimeOutLength()
        {
            var applicationSetting =_unitOfWork.Repository<ApplicationSetting>().Get().SingleOrDefault(a => a.ApplicationSettingName == "TimeoutLength");
            return applicationSetting;
        }

        public void AddApplicationSetting(ApplicationSetting applicationSetting)
        {
            _unitOfWork.Repository<ApplicationSetting>().Insert(applicationSetting);
            _unitOfWork.Save();
        }

        public void UpdateApplicationSetting(ApplicationSetting applicationSetting)
        {
            _unitOfWork.Repository<ApplicationSetting>().Update(applicationSetting);
            _unitOfWork.Save();
        }

        public void DeleteApplicationSetting(ApplicationSetting applicationSetting)
        {
            _unitOfWork.Repository<ApplicationSetting>().Delete(applicationSetting);
            _unitOfWork.Save();
        }

        public void DeleteApplicationSettingById(int id)
        {
            _unitOfWork.Repository<ApplicationSetting>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
