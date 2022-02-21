using Profilum.GatewayService.BLL.Models;
using static Profilum.GatewayService.Common.BaseModels.AppResponse;

namespace Profilum.GatewayService.BLL.Handlers.Interfaces;

public interface IGatewayHandler
{
    IAsyncEnumerable<UserResponse> GetAll();

    Task<Response> InitData();

    Task<Response> DeleteAll();
}