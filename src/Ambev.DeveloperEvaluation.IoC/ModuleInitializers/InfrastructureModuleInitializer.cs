using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.EventLog;
using Ambev.DeveloperEvaluation.EventLog.Events.Interfaces;
using Ambev.DeveloperEvaluation.EventLog.Events.Persistence;
using Ambev.DeveloperEvaluation.EventLog.LogStorage.Persistence;
using Ambev.DeveloperEvaluation.EventLog.LogStorage.Interfaces;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.UoW;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.Common.Events;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        // Register NoSqlContext as a Singleton
        builder.Services.AddSingleton<NoSqlContext>();

        // Register MongoDB database instance
        builder.Services.AddSingleton<IMongoDatabase>(provider =>
        {
            var context = provider.GetRequiredService<NoSqlContext>();
            return context.Database;
        });

        // Register Event Bus
        builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

        // Register Event Store and Log Storage
        builder.Services.AddScoped<IEventStore, MongoEventStorage>();
        builder.Services.AddScoped<ILogStorage, MongoLogStorage>();

        // Automatically register all event handlers
        EventHandlerRegistrar.Register(builder.Services);

        // Register relational database context
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());

        // Register other services
        builder.Services.AddScoped<IDiscountService, DiscountService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

}
