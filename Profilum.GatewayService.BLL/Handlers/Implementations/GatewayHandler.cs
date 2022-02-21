using Profilum.GatewayService.Common.BaseModels;
using Profilum.GatewayService.BLL.Handlers.Interfaces;
using Profilum.GatewayService.BLL.Models;
using Profilum.GatewayService.Common;
using Profilum.GatewayService.DAL.Connectors;
using Profilum.GatewayService.DAL.Connectors.GrpcConnectors.Connectors;
using static Profilum.GatewayService.Common.BaseModels.AppResponse;
using DalResponse = Profilum.GatewayService.DAL.Models;

namespace Profilum.GatewayService.BLL.Handlers.Implementations;

public class GatewayHandler : IGatewayHandler
{
    
    private readonly UserServiceConnector _userServiceConnector;
    private readonly AccountServiceConnector _accountServiceConnector;
    private readonly AppSettings _settings;
    
    private const int InitCountUsers = 1000;
    
    public GatewayHandler(AppSettings settings)
    {
        _settings = settings;
        _userServiceConnector = new UserServiceConnector(settings);
        _accountServiceConnector = new AccountServiceConnector(settings);
    }
    
    public IAsyncEnumerable<UserResponse> GetAll()
    {
        try
        {
            var getUsers = _userServiceConnector.UserGetAllCall();
            var getAccounts = _accountServiceConnector.GetAllCall();
            var t = getUsers.GroupJoin(getAccounts, u => Guid.Parse(u.Id), a => a.UserId, (user, accounts) => new UserResponse
                {
                    Id = Guid.Parse(user.Id),
                    Name = user.Name,
                    Accounts = accounts.Select(a => new AccountResponse(a))
                });

            return t;
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

    public async Task<Response> InitData()
    {
        try
        {
            var messageBus = new MessageBus(_settings.KafkaServer, _settings.AccountKafkaTopic);
            for (var i = 0; i < InitCountUsers; i++) //TODO create task
            {
                var createUser = await _userServiceConnector.CreateUserCall(new DalResponse.UserRequest
                {
                    Name = $"User_{Guid.NewGuid()}"
                });
                if (!createUser.IsSuccess)
                    throw new CustomException(createUser.ResultCode, createUser.LastResultMessage);

                var sendMessageToKafka = await messageBus.SendMessage(createUser.Data.Id.ToString());
                if(!sendMessageToKafka.IsSuccess)
                    throw new CustomException(sendMessageToKafka.ResultCode, sendMessageToKafka.LastResultMessage);
            }
            
            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
    
    
    public async Task<Response> DeleteAll()
    {
        try
        {
            var deleteAllAccounts = await _accountServiceConnector.DeleteAllCall();
            if (!deleteAllAccounts.IsSuccess)
                throw new CustomException(deleteAllAccounts.ResultCode, deleteAllAccounts.LastResultMessage);
            
            var deleteAllUsers = await _userServiceConnector.DeleteAllCall();
            if (!deleteAllUsers.IsSuccess)
                throw new CustomException(deleteAllUsers.ResultCode, deleteAllUsers.LastResultMessage);

            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
}