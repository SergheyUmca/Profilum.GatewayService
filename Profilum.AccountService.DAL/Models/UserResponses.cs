using System.ComponentModel.DataAnnotations;
using Profilum.AccountService.DAL.MongoDb.Models;

namespace Profilum.AccountService.DAL.Models;

public class UserResponse
{
   public long Id { get; set; }
   
   public string Name { get; set; }

   public UserResponse(Users userEntity)
   {

      Id = userEntity.Id;
      Name = userEntity.Name;
   }
   
   public UserResponse(UserRequest request)
   {
      Id = request.Id;
      Name = request.Name;
   }
}