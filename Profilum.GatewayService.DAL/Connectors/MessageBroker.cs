using System.Net;
using Confluent.Kafka;
using Profilum.GatewayService.Common;
using Profilum.GatewayService.Common.BaseModels;
using static Profilum.GatewayService.Common.BaseModels.AppResponse;

namespace Profilum.GatewayService.DAL.Connectors;

public sealed class MessageBus
{
    private readonly ProducerConfig _producerConfig;
    private readonly string _topicName;

    public MessageBus(string host, string topicName)
    {
        _topicName = topicName;
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = host,
            ClientId = Dns.GetHostName()
        };
    }

    public async Task<Response> SendMessage<T>(T message)
    {
        try
        {
            using (var producer = new ProducerBuilder<Null, T>(_producerConfig).Build())
            {
                var sendMessage =
                    await producer.ProduceAsync(_topicName, new Message<Null, T> { Value = message });
                if (sendMessage.Status == PersistenceStatus.NotPersisted)
                    throw new CustomException(ResponseCodes.TECHNICAL_ERROR, "message not sent");
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
}