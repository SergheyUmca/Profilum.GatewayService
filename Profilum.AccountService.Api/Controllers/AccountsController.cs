using Microsoft.AspNetCore.Mvc;
using Profilum.AccountService.Api.Models;
using Profilum.AccountService.BLL.Handlers.Interfaces;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    [HttpGet]
    public async Task<Response<AccountResponse>> Get([FromServices]IAccountHandler userHandler , long id)
    {
        var getUser = await userHandler.Get(id);
        return !getUser.IsSuccess
            ? new ErrorResponse<AccountResponse>(getUser.LastResultMessage, getUser.ResultCode)
            : new Response<AccountResponse>(new AccountResponse(getUser.Data));
    }
    
    [HttpGet("GetAll")]
    public async Task<Response<List<AccountResponse>>> GetAll([FromServices]IAccountHandler userHandler)
    {
        var getUsers = await userHandler.GetAll();
        return !getUsers.IsSuccess
            ? new ErrorResponse<List<AccountResponse>>(getUsers.LastResultMessage, getUsers.ResultCode)
            : new Response<List<AccountResponse>>(getUsers.Data.Select(u => new AccountResponse(u)).ToList());
    }
    
    [HttpPost]
    public async Task<Response<AccountResponse>> Create([FromServices]IAccountHandler userHandler, [FromBody]AccountRequest request)
    {
        var createUser = await userHandler.Create(request.ConvertToBll());
        return !createUser.IsSuccess
            ? new ErrorResponse<AccountResponse>(createUser.LastResultMessage, createUser.ResultCode)
            : new Response<AccountResponse>(new AccountResponse(createUser.Data));
    }
    
    [HttpPut]
    public async Task<Response<AccountResponse>> Update([FromServices]IAccountHandler userHandler, [FromBody]AccountRequest request)
    {
        var updateUser = await userHandler.Update(request.ConvertToBll());
        return !updateUser.IsSuccess
            ? new ErrorResponse<AccountResponse>(updateUser.LastResultMessage, updateUser.ResultCode)
            : new Response<AccountResponse>(new AccountResponse(updateUser.Data));
    }
    
    [HttpDelete]
    public async Task<Response> Delete([FromServices]IAccountHandler userHandler, long id)
    {
        var deleteUser = await userHandler.Delete(id);
        return !deleteUser.IsSuccess
            ? new ErrorResponse(deleteUser.LastResultMessage, deleteUser.ResultCode)
            : new Response();
    }
    
    [HttpDelete("DeleteAll")]
    public async Task<Response> DeleteAll([FromServices]IAccountHandler userHandler)
    {
        var deleteAll = await userHandler.DeleteAll();
        return !deleteAll.IsSuccess
            ? new ErrorResponse(deleteAll.LastResultMessage, deleteAll.ResultCode)
            : new Response();
    }
}