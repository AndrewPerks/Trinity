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
    /// Role service that carries out the business transactions involving roles
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Role> Get(Expression<Func<Role, bool>> predicate = null, string includeProperties = "")
        {
            var roles = _unitOfWork.Repository<Role>().Get(predicate, includeProperties);
            return roles.ToList();
        }

        public Role Search(string roleName)
        {
            var role = Get(r => r.Role1 == roleName).SingleOrDefault();
            return role;
        }

        public Role GetById(int id)
        {
            var role = _unitOfWork.Repository<Role>().GetById(id);
            return role;
        }

        public void AddRole(Role role)
        {
            _unitOfWork.Repository<Role>().Insert(role);
            _unitOfWork.Save();
        }

        public void UpdateRole(Role role)
        {
            _unitOfWork.Repository<Role>().Update(role);
            _unitOfWork.Save();
        }

        public void DeleteRole(Role role)
        {
            _unitOfWork.Repository<Role>().Delete(role);
            _unitOfWork.Save();
        }

        public void DeleteRoleById(int id)
        {
            _unitOfWork.Repository<Role>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
