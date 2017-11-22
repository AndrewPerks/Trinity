using Microsoft.Practices.Unity;
using Trinity.DataAccess.Concrete;
using Trinity.DataAccess.Interfaces;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;

namespace Trinity.Helpers
{
    /// <summary>
    /// IOC Container
    /// </summary>
    public static class ContainerHelper
    {
        private static IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<ITrinityContext, TrinityContext>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUnitOfWork, UnitOfWork>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClientService, ClientService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IContactService, ContactService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRoleService, RoleService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IApplicationSettingService, ApplicationSettingService>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }

    }
}
