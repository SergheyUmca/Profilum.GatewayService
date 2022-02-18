

namespace Profilum.AccountService.DAL.Models;

public class AccountRequest
{
    public long Id { get; set; }
   
    public long UserId { get; set; }
    
    public string AccountNumber { get; set; }
    
}