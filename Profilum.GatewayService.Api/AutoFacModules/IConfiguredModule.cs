using Profilum.GatewayService.Api.Models;
using Profilum.GatewayService.Common.BaseModels;

namespace Profilum.GatewayService.Api.AutoFacModules
{
    public interface IConfiguredModule
    {
        AppSettings Settings { get; set; }
    }
}