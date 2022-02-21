namespace Profilum.GatewayService.BLL.Models;

public class UserResponse
{
   public Guid Id { get; set; }
   
   public string Name { get; set; }
   
   public IAsyncEnumerable<AccountResponse> Accounts { get; set; }

   public UserResponse()
   {
      
   }

   public UserResponse(GatewayService.DAL.Models.UserResponse dbResponse)
   {
      Id = dbResponse.Id;
      Name = dbResponse.Name;
   }
}