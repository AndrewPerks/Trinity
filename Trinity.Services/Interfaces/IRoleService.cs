using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the role service
    /// </summary>
    public interface IRoleService
    {
        List<Role> Get(Expression<Func<Role, bool>> predicate = null, string includeProperties = "");
        Role GetById(int id);
        Role Search(string roleName);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void DeleteRoleById(int id);
        void Dispose();
    }
}
