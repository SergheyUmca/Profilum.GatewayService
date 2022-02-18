using Microsoft.EntityFrameworkCore;
using Profilum.AccountService.DAL.EF.Contexts;
using Profilum.AccountService.DAL.EF.Repositories;
using Profilum.AccountService.DAL.EF.Services.Interfaces;

namespace Profilum.AccountService.DAL.EF.Services.Implementations
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DbService : IDbService
    {
        private readonly BaseContext _db;
        private readonly Lazy<DbService> _lazyDbService;
        private readonly object _locker = new();
        private bool _disposed;
        
        private const int MaxConnectionRetriesNumber = 3;
        private const int MaxTimeBetweenRetriesInSeconds = 30;

        private IAccountRepository _accountRepository;

        public DbService (string connectionString)
        {
            lock (_locker)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BaseContext>();
                try
                {
                    optionsBuilder.UseNpgsql(connectionString, option =>
                    {
                        option.EnableRetryOnFailure(MaxConnectionRetriesNumber,
                            TimeSpan.FromSeconds(MaxTimeBetweenRetriesInSeconds),
                            null);
                        option.CommandTimeout(9000);
                    });
                }
                catch
                {
                    optionsBuilder.UseNpgsql(connectionString, option =>
                        option.EnableRetryOnFailure(2, TimeSpan.FromSeconds(30), null));
                }

                var db = new BaseContext(optionsBuilder.Options);
                _db = db;
                _lazyDbService = new Lazy<DbService>(() => new DbService(connectionString));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _db.Dispose();
            }

            _disposed = true;
        }

        public DbService DbServiceInstance => _lazyDbService.Value;
        
        
        public IAccountRepository Accounts =>
            _accountRepository ??= new AccountRepository(_db);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}