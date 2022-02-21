using Microsoft.AspNetCore.Mvc;
using Profilum.GatewayService.BLL.Handlers.Interfaces;
using Profilum.GatewayService.Api.Models;
using static Profilum.GatewayService.Common.BaseModels.AppResponse;

namespace Profilum.GatewayService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GatewayController : ControllerBase
{
    
    [HttpGet("GetAll")]
    public IAsyncEnumerable<UserResponse> GetAll([FromServices]IGatewayHandler gatewayHandler)
    {
       return gatewayHandler.GetAll().Select(u => new UserResponse
       {
           Id = u.Id,
           Name = u.Name,
           Accounts = u.Accounts.Select(a => new AccountResponse(a))
       });
    }
    
    [HttpPost("InitData")]
    public async Task<Response> InitData([FromServices]IGatewayHandler gatewayHandler)
    {
        var createUser = await gatewayHandler.InitData();
        return !createUser.IsSuccess
            ? new ErrorResponse(createUser.LastResultMessage, createUser.ResultCode)
            : new Response();
    }
    
    
    [HttpDelete("DeleteAll")]
    public async Task<Response> DeleteAll([FromServices]IGatewayHandler gatewayHandler)
    {
        var deleteAll = await gatewayHandler.DeleteAll();
        return !deleteAll.IsSuccess
            ? new ErrorResponse(deleteAll.LastResultMessage, deleteAll.ResultCode)
            : new Response();
    }
}