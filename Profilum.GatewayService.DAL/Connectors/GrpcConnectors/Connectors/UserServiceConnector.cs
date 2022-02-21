using Grpc.Core;
using Grpc.Net.Client;
using Profilum.GatewayService.Common;
using Profilum.GatewayService.Common.BaseModels;
using Profilum.GatewayService.DAL.Models;
using Profilum.UserService;
using static Profilum.GatewayService.Common.BaseModels.AppResponse;

namespace Profilum.GatewayService.DAL.Connectors.GrpcConnectors.Connectors
{
    public class UserServiceConnector
    {
        private readonly UserService.UserService.UserServiceClient _userClient;
        private const int ConnectionAttemptThreadSleepingTime = 1000;
        private const int ConnectionAttemptsNumber = 3;

        public UserServiceConnector(AppSettings settings)
        {
            try
            {
                var serviceAddress = $"http://localhost:{settings.UserGrpcServerPort}";
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                var channel = GrpcChannel.ForAddress(serviceAddress, new GrpcChannelOptions
                {
                    HttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }
                });
                _userClient = new UserService.UserService.UserServiceClient(channel);
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseCodes.TECHNICAL_ERROR, e.Message);
            }
           
        }

        public IAsyncEnumerable<UserFullReply> UserGetAllCall()
        {
            try
            {
                var request = _userClient.GetAllUsers(new EmptyRequest());

                return request.ResponseStream.ReadAllAsync();
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
                var deleteAllUsersAsync =  await _userClient.DeleteAllUsersAsync(new EmptyRequest());
                if (deleteAllUsersAsync.ReplyStateCode != 0) 
                    return new ErrorResponse("ServiceUnavailableErrorMessage");
                
                for (var i = 0; i < ConnectionAttemptsNumber; i++)
                {
                    deleteAllUsersAsync =  await _userClient.DeleteAllUsersAsync(new EmptyRequest());
                    if (deleteAllUsersAsync.ReplyStateCode == 0) 
                        return new Response();

                    Thread.Sleep(ConnectionAttemptThreadSleepingTime);
                }

                return new ErrorResponse("ServiceUnavailableErrorMessage");
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseCodes.TECHNICAL_ERROR, e.Message);
            }
        }
        
        public async Task<Response<UserResponse>> CreateUserCall(UserRequest request)
        {
            try
            {
                var createUserAsync =  await _userClient.CreateUserAsync(new UserCreateRequest
                {
                    Name = request.Name
                });
                if (createUserAsync.ReplyStateCode == 0)
                {
                    return new Response<UserResponse>(new UserResponse
                    {
                        Id = Guid.Parse(createUserAsync.Id),
                        Name = createUserAsync.Name
                    });
                }
                
                for (var i = 0; i < ConnectionAttemptsNumber; i++)
                {
                    createUserAsync =  await _userClient.CreateUserAsync(new UserCreateRequest
                    {
                        Name = request.Name
                    });
                    if (createUserAsync.ReplyStateCode == 0)
                    {
                        return new Response<UserResponse>(new UserResponse
                        {
                            Id = Guid.Parse(createUserAsync.Id),
                            Name = createUserAsync.Name
                        });
                    }

                    Thread.Sleep(ConnectionAttemptThreadSleepingTime);
                }
                
                return new ErrorResponse<UserResponse>("ServiceUnavailableErrorMessage");
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseCodes.TECHNICAL_ERROR, e.Message);
            }
        }
    }
}