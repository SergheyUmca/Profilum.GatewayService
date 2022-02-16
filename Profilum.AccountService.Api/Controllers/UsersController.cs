using Microsoft.AspNetCore.Mvc;
using Profilum.AccountService.Api.Models;
using Profilum.AccountService.BLL.Handlers.Interfaces;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<Response<UserResponse>> Get([FromServices]IUserHandler userHandler , long id)
    {
        var getUser = await userHandler.Get(id);
        return !getUser.IsSuccess
            ? new ErrorResponse<UserResponse>(getUser.LastResultMessage, getUser.ResultCode)
            : new Response<UserResponse>(new UserResponse(getUser.Data));
    }
    
    [HttpGet("GetAll")]
    public async Task<Response<List<UserResponse>>> GetAll([FromServices]IUserHandler userHandler)
    {
        var getUsers = await userHandler.GetAll();
        return !getUsers.IsSuccess
            ? new ErrorResponse<List<UserResponse>>(getUsers.LastResultMessage, getUsers.ResultCode)
            : new Response<List<UserResponse>>(getUsers.Data.Select(u => new UserResponse(u)).ToList());
    }
    
    [HttpPost]
    public async Task<Response<UserResponse>> Create([FromServices]IUserHandler userHandler, [FromBody]UserRequest request)
    {
        var createUser = await userHandler.Create(request.ConvertToBll());
        return !createUser.IsSuccess
            ? new ErrorResponse<UserResponse>(createUser.LastResultMessage, createUser.ResultCode)
            : new Response<UserResponse>(new UserResponse(createUser.Data));
    }
    
    [HttpPut]
    public async Task<Response<UserResponse>> Update([FromServices]IUserHandler userHandler, [FromBody]UserRequest request)
    {
        var updateUser = await userHandler.Update(request.ConvertToBll());
        return !updateUser.IsSuccess
            ? new ErrorResponse<UserResponse>(updateUser.LastResultMessage, updateUser.ResultCode)
            : new Response<UserResponse>(new UserResponse(updateUser.Data));
    }
    
    [HttpDelete]
    public async Task<Response> Delete([FromServices]IUserHandler userHandler, long id)
    {
        var deleteUser = await userHandler.Delete(id);
        return !deleteUser.IsSuccess
            ? new ErrorResponse(deleteUser.LastResultMessage, deleteUser.ResultCode)
            : new Response();
    }
    
    [HttpDelete("DeleteAll")]
    public async Task<Response> DeleteAll([FromServices]IUserHandler userHandler)
    {
        var deleteAll = await userHandler.DeleteAll();
        return !deleteAll.IsSuccess
            ? new ErrorResponse(deleteAll.LastResultMessage, deleteAll.ResultCode)
            : new Response();
    }
}