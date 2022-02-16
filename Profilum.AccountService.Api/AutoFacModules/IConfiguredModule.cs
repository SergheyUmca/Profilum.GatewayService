using Profilum.AccountService.Api.Models;

namespace Profilum.AccountService.Api.AutoFacModules
{
    public interface IConfiguredModule
    {
        Settings Settings { get; set; }
    }
}