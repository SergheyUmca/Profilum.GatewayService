namespace Profilum.AccountService.BLL.Models;

public class UserRequest
{
    public long Id { get; set; }
   
    public string Name { get; set; }
    
    public DAL.Models.UserRequest ConvertToDal()
    {
        return new DAL.Models.UserRequest
        {
            Id = Id,
            Name = Name
        };
    }
}