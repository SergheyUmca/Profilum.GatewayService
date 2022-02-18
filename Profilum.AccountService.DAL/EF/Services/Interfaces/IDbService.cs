using Profilum.AccountService.DAL.EF.Repositories;
using Profilum.AccountService.DAL.EF.Services.Implementations;

namespace Profilum.AccountService.DAL.EF.Services.Interfaces
{
    public interface IDbService : IDisposable
    {
        DbService DbServiceInstance { get; }
        

        IAccountRepository Accounts { get; }
    }
}