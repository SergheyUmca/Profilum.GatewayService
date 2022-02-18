namespace Profilum.AccountService.BLL.Models;

public class AccountResponse
{
   public long Id { get; set; }
   
   public long UserId { get; set; }
    
   public string AccountNumber { get; set; }

   public AccountResponse(DAL.Models.AccountResponse dbResponse)
   {
      Id = dbResponse.Id;
      UserId = dbResponse.UserId;
      AccountNumber = dbResponse.AccountNumber;
   }

   public AccountResponse()
   {
      
   }
}