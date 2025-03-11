using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.MongoDB;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics;

namespace Ambev.DeveloperEvaluation.Common.Logging;

/// <summary>
/// Provides a default logging configuration using Serilog.
/// Supports console, file, and MongoDB sinks.
/// </summary>
public static class LoggingExtension
{
    private static readonly DestructuringOptionsBuilder _destructuringOptionsBuilder = new DestructuringOptionsBuilder()
        .WithDefaultDestructurers()
        .WithDestructurers([new DbUpdateExceptionDestructurer()]);

    private static readonly Func<LogEvent, bool> _filterPredicate = exclusionPredicate =>
    {
        if (exclusionPredicate.Level != LogEventLevel.Information) return true;

        exclusionPredicate.Properties.TryGetValue("StatusCode", out var statusCode);
        exclusionPredicate.Properties.TryGetValue("Path", out var path);

        var excludeByStatusCode = statusCode == null || statusCode.ToString().Equals("200");
        var excludeByPath = path?.ToString().Contains("/health") ?? false;

        return excludeByStatusCode && excludeByPath;
    };

    /// <summary>
    /// Configures Serilog logging for the application.
    /// </summary>
    public static WebApplicationBuilder AddDefaultLogging(this WebApplicationBuilder builder)
    {
        var mongoDbConnection = builder.Configuration["ConnectionStrings:MongoDbConnection"];
        var applicationLogsCollection = builder.Configuration["MongoDB:ApplicationLogsCollection"];
        var errorLogsCollection = builder.Configuration["MongoDB:ErrorLogsCollection"];
        var eventLogsCollection = builder.Configuration["MongoDB:EventLogsCollection"];

        // Ensure MongoDB connection string is not null
        if (string.IsNullOrEmpty(mongoDbConnection))
        {
            Console.WriteLine("MongoDB ConnectionString is missing! Logs will not be persisted in MongoDB.");
            mongoDbConnection = "mongodb://localhost:27017"; // Default value for local development
        }

        // Ensure Collection Names are not null
        applicationLogsCollection ??= "ApplicationLogs";
        errorLogsCollection ??= "ErrorLogs";
        eventLogsCollection ??= "EventLogs";

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Filter.ByExcluding(_filterPredicate)
            .WriteTo.Console(theme: SystemConsoleTheme.Colored)
            .WriteTo.File("logs/app-log-.txt", rollingInterval: Serilog.RollingInterval.Day)
            .WriteTo.MongoDB(mongoDbConnection, collectionName: applicationLogsCollection, restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.MongoDB(mongoDbConnection, collectionName: errorLogsCollection, restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.MongoDB(mongoDbConnection, collectionName: eventLogsCollection, restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddLogging();

        return builder;
    }

    /// <summary>
    /// Enables logging in the application and logs the startup message.
    /// </summary>
    public static WebApplication UseDefaultLogging(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Logger>>();
        var mode = Debugger.IsAttached ? "Debug" : "Release";

        logger.LogInformation("Logging enabled for '{Application}' on '{Environment}' - Mode: {Mode}",
            app.Environment.ApplicationName, app.Environment.EnvironmentName, mode);

        return app;
    }
}
