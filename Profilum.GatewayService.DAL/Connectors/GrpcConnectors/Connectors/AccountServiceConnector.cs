using Grpc.Core;
using Grpc.Net.Client;
using Profilum.GatewayService.Common;
using Profilum.GatewayService.Common.BaseModels;
using Profilum.GatewayService.DAL.Models;
using static Profilum.GatewayService.Common.BaseModels.AppResponse;
using EmptyRequest = Profilum.AccountService.EmptyRequest;

namespace Profilum.GatewayService.DAL.Connectors.GrpcConnectors.Connectors
{
    public class AccountServiceConnector
    {
        private readonly AccountService.AccountService.AccountServiceClient _accountClient;
        private const int FolderDeleterConnectionAttemptThreadSleepingTime = 1000;
        private const int FolderDeleterConnectionAttemptsNumber = 3;

        public AccountServiceConnector(AppSettings settings)
        {
            try
            {
                var serviceAddress = $"http://localhost:{settings.AccountGrpcServerPort}";
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                var channel = GrpcChannel.ForAddress(serviceAddress, new GrpcChannelOptions
                {
                    HttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }
                });
                _accountClient = new AccountService.AccountService.AccountServiceClient(channel);
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseCodes.TECHNICAL_ERROR, e.Message);
            }
           
        }

        public IAsyncEnumerable<AccountResponse> GetAllCall()
        {
            try
            {
                var request = _accountClient.GetAllAccounts(new EmptyRequest());

                return request.ResponseStream.ReadAllAsync().Select(a => new AccountResponse
                {
                    Id = a.Id,
                    UserId = Guid.Parse(a.UserId),
                    AccountNumber = a.AccountNumber
                });
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseCodes.TECHNICAL_ERROR, e.Message);
            }
        }
        
        public async Task<Response> DeleteAllCall()
        {
            try
            {
                var replyFromDeleter =  await _accountClient.DeleteAllAccountsAsync(new EmptyRequest());

                for (var i = 0; i < FolderDeleterConnectionAttemptsNumber; i++)
                {
                    if (replyFromDeleter.ReplyStateCode == 0) 
                        return new Response();

                    Thread.Sleep(FolderDeleterConnectionAttemptThreadSleepingTime);
                }
                
                return new ErrorResponse("ServiceUnavailableErrorMessage");
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseCodes.TECHNICAL_ERROR, e.Message);
            }
        }
    }
}