using Autofac;
using Profilum.GatewayService.BLL.Handlers.Implementations;
using Profilum.GatewayService.BLL.Handlers.Interfaces;
using Profilum.GatewayService.Common.BaseModels;

namespace Profilum.GatewayService.Api.AutoFacModules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiModule : ConfiguredModule
    {
        public DiModule(AppSettings settings) : base(settings)
        {
            
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GatewayHandler>().As<IGatewayHandler>()
                .WithParameter(new NamedParameter("settings", Settings));
        }

        
    }
}