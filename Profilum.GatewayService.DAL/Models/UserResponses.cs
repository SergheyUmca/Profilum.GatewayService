namespace Profilum.GatewayService.DAL.Models;

public class UserResponse
{
   public Guid Id { get; set; }
   
   public string Name { get; set; }

   public UserResponse()
   {
      
   }

   public UserResponse(UserRequest request)
   {
      Id = request.Id;
      Name = request.Name;
   }
}