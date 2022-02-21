using Autofac;
using Profilum.GatewayService.Common.BaseModels;

namespace Profilum.GatewayService.Api.AutoFacModules
{
    public abstract class ConfiguredModule : Module, IConfiguredModule
    {
        protected ConfiguredModule(AppSettings settings)
        {
            Settings = settings;
        }

        public AppSettings Settings { get; set; }
    }
}