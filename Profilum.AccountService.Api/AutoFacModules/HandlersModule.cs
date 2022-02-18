using Autofac;
using Autofac.Core;
using Profilum.AccountService.Api.Models;
using Profilum.AccountService.BLL.Handlers.Implementations;
using Profilum.AccountService.BLL.Handlers.Interfaces;

namespace Profilum.AccountService.Api.AutoFacModules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HandlersModule : ConfiguredModule
    {
        public HandlersModule(Settings settings) : base(settings)
        {
            
        }
        protected override void Load(ContainerBuilder builder)
        {
            var parameters = new List<Parameter>
            {
                new NamedParameter("connectionString", Settings.ConnectionString),
                new NamedParameter("dbName", Settings.Database)
            };

            builder.RegisterType<AccountHandler>().As<IAccountHandler>().WithParameters(parameters);
        }

        
    }
}