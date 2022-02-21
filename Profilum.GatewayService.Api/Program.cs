using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using Profilum.GatewayService.Api.AutoFacModules;
using Profilum.GatewayService.Common.BaseModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b =>
    b.RegisterConfiguredModulesFromAssemblyContaining<DiModule>(new AppSettings
    {
        KafkaServer = builder.Configuration.GetSection("Application:KafkaServerAddress").Value,
        AccountKafkaTopic = builder.Configuration.GetSection("Application:KafkaServerAccountServiceTopic").Value,
        AccountGrpcServerPort =
            int.TryParse(builder.Configuration.GetSection("Application:AccountGrpcServerPort")?.Value,
                out var accountGrpcPort)
            ? accountGrpcPort
            : throw new ArgumentException("AccountGrpcServerPort is not valid"),
        UserGrpcServerPort = int.TryParse(builder.Configuration.GetSection("Application:UserGrpcServerPort")?.Value,
            out var userGrpcPort)
            ? userGrpcPort
            :  throw new ArgumentException("UserGrpcServerPort is not valid"),
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();