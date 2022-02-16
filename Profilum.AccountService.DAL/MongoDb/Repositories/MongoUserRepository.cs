using Profilum.AccountService.Common;
using Profilum.AccountService.Common.BaseModels;
using Profilum.AccountService.DAL.Models;
using Profilum.AccountService.DAL.MongoDb.Models;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.DAL.MongoDb.Repositories;

public class MongoUserRepository
{
    private readonly IRepository<Users> _repository;
        
    public MongoUserRepository(string connectionString, string databaseName)
    {
        _repository = new MongoRepository<Users>(connectionString, databaseName);
    }
    
    
    public async Task<Response<List<UserResponse>>> GetAll()
    {
        try
        {
            var getAllUsers = await _repository.All();

            return new Response<List<UserResponse>>(getAllUsers.Select(u => new UserResponse(u)).ToList());
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
            var getUser = await _repository.Single(id, "Id");
            
            return new Response<UserResponse>(new UserResponse(getUser));

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
            await _repository.Save(request.ConvertToEntity());
            
            return new Response<UserResponse>(new UserResponse(request));

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
            var update = await _repository.Update(request.Id, nameof(request.Id), request.ConvertToEntity());
            if (!update)
                throw new CustomException(ResponseCodes.DATABASE_ERROR, $"Entity not updated");
            
            return new Response<UserResponse>(new UserResponse(request));
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
            await _repository.Delete(id, "Id");

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
            await _repository.DeleteAll();

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