using Grpc.Core;
using Profilum.UserService;
using static Profilum.UserService.UserService;

namespace Profilum.AccountService.Api.Services;

public class UserService : UserServiceBase
{
     private readonly ILogger<UserService> _logger;

     public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }
        

    public override async Task<UserFullReply> GetUser(UserGetRequest request, ServerCallContext context)
    {

        return new UserFullReply();
    }
    
    public override async Task<UserFullReply> CreateUser(UserCreateRequest request, ServerCallContext context)
    {
        return new UserFullReply();
    }
     
    public override async Task<UserFullReply> UpdateUser(UserCreateRequest request, ServerCallContext context)
    {
        return new UserFullReply();
    }
      
    public override async Task<UserReply> DeleteUser(UserGetRequest request, ServerCallContext context)
    {
        return new UserReply();
    }
}