using Profilum.AccountService.BLL.Handlers.Interfaces;
using Profilum.AccountService.BLL.Models;
using Profilum.AccountService.Common.BaseModels;
using Profilum.AccountService.DAL.MongoDb.Repositories;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.BLL.Handlers.Implementations;

public class UserHandler : IUserHandler
{
    private readonly MongoUserRepository _mongoUserRepository;
    
    public UserHandler(string connectionString, string dbName)
    {
        _mongoUserRepository = new MongoUserRepository(connectionString, dbName);
    }
    
    public async Task<Response<List<UserResponse>>> GetAll()
    {
        try
        {
            var getAllUsers = await _mongoUserRepository.GetAll();
            if (!getAllUsers.IsSuccess)
                throw new CustomException(getAllUsers.ResultCode, getAllUsers.LastResultMessage);

            return new Response<List<UserResponse>>(getAllUsers.Data.Select(u => new UserResponse(u)).ToList());
        }
        catch (CustomException ce)
        {
            return new ErrorResponse<List<UserResponse>>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<List<UserResponse>>(e.Message);
        }
    }
    
    public async Task<Response<UserResponse>> Get(long id)
    {
        try
        {
            var getUser = await _mongoUserRepository.Get(id);
            if (!getUser.IsSuccess)
                throw new CustomException(getUser.ResultCode, getUser.LastResultMessage);

            return new Response<UserResponse>(new UserResponse(getUser.Data));
        }
        catch (CustomException ce)
        {
            return new ErrorResponse<UserResponse>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<UserResponse>(e.Message);
        }
    }
    
    public async Task<Response<UserResponse>> Create( UserRequest request)
    {
        try
        {
            var createUser = await _mongoUserRepository.Create(request.ConvertToDal());
            if (!createUser.IsSuccess)
                throw new CustomException(createUser.ResultCode, createUser.LastResultMessage);

            return new Response<UserResponse>(new UserResponse(createUser.Data));

        }
        catch (CustomException ce)
        {
            return new ErrorResponse<UserResponse>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<UserResponse>(e.Message);
        }
    }
    
    public async Task<Response<UserResponse>> Update(UserRequest request)
    {
        try
        {
            var updateUser = await _mongoUserRepository.Update(request.ConvertToDal());
            if (!updateUser.IsSuccess)
                throw new CustomException(updateUser.ResultCode, updateUser.LastResultMessage);

            return new Response<UserResponse>(new UserResponse(updateUser.Data));

        }
        catch (CustomException ce)
        {
            return new ErrorResponse<UserResponse>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<UserResponse>(e.Message);
        }
    }
    
    public async Task<Response> Delete(long id)
    {
        try
        {
            var delete = await _mongoUserRepository.Delete(id);
            if (!delete.IsSuccess)
                throw new CustomException(delete.ResultCode, delete.LastResultMessage);

            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
    
    public async Task<Response> DeleteAll()
    {
        try
        {
            var deleteAll = await _mongoUserRepository.DeleteAll();
            if (!deleteAll.IsSuccess)
                throw new CustomException(deleteAll.ResultCode, deleteAll.LastResultMessage);
            
            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
}