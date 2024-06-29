using Application.App_Start;
using AutoMapper;
using Domain.Interfaces;
using Domain.Services;
using Infraestructure.Repository;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace Application
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IAccountRepository, AccountRepository>();

            var mapperConfig = AutoMapperConfig.InitializeAutoMapper();
            IMapper mapper = mapperConfig.CreateMapper();

            container.RegisterType<IMapper, Mapper>(new InjectionConstructor(mapperConfig));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}