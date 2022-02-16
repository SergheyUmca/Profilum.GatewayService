using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using Profilum.AccountService.Api.AutoFacModules;
using Profilum.AccountService.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b =>
    b.RegisterConfiguredModulesFromAssemblyContaining<HandlersModule>(new Settings
    {
        ConnectionString =  builder.Configuration.GetSection("MongoConnection:ConnectionString").Value,
        Database = builder.Configuration.GetSection("MongoConnection:Database").Value
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