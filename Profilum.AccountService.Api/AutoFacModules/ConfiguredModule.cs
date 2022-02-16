using Autofac;
using Profilum.AccountService.Api.Models;

namespace Profilum.AccountService.Api.AutoFacModules
{
    public abstract class ConfiguredModule : Module, IConfiguredModule
    {
        protected ConfiguredModule(Settings settings)
        {
            Settings = settings;
        }

        public Settings Settings { get; set; }
    }
}