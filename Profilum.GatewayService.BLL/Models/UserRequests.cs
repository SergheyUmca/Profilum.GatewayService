namespace Profilum.GatewayService.BLL.Models;

public class UserRequest
{
    public Guid Id { get; set; }
   
    public string Name { get; set; }
    
    public GatewayService.DAL.Models.UserRequest ConvertToDal()
    {
        return new GatewayService.DAL.Models.UserRequest
        {
            Id = Id,
            Name = Name
        };
    }
}