using Grpc.Core;
using static Profilum.AccountService.AccountService;

namespace Profilum.AccountService.Api.Services;

public class AccountService : AccountServiceBase
{
     private readonly ILogger<AccountService> _logger;

     public AccountService(ILogger<AccountService> logger)
        {
            _logger = logger;
        }
        
     public override async Task<AccountFullReply> GetAccount(AccountGetRequest request, ServerCallContext context)
     {

         return new AccountFullReply();
     }
    
     public override async Task<AccountFullReply> CreateAccount(AccountCreateRequest request, ServerCallContext context)
     {
         return new AccountFullReply();
     }
     
     public override async Task<AccountFullReply> UpdateAccount(AccountCreateRequest request, ServerCallContext context)
     {
         return new AccountFullReply();
     }
      
     public override async Task<AccountReply> DeleteAccount(AccountGetRequest request, ServerCallContext context)
     {
         return new AccountReply();
     }
}