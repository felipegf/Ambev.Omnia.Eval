using Ambev.DeveloperEvaluation.Common.Events;
using Ambev.DeveloperEvaluation.EventLog.LogStorage.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.EventLog;

/// <summary>
/// Manages NoSQL database connections, specifically for MongoDB.
/// </summary>
public class NoSqlContext
{
    private readonly IMongoDatabase _database;

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSqlContext"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    public NoSqlContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDbConnection");
        var databaseName = configuration["MongoDB:DatabaseName"];

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    /// <summary>
    /// Gets the MongoDB database instance.
    /// </summary>
    public IMongoDatabase Database => _database;

    /// <summary>
    /// Gets the MongoDB collection for a given entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="collectionName">The collection name.</param>
    /// <returns>The MongoDB collection.</returns>
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    /// <summary>
    /// Gets the collection name for application logs.
    /// </summary>
    public IMongoCollection<LogEntry> ApplicationLogs =>
        _database.GetCollection<LogEntry>("ApplicationLogs");

    /// <summary>
    /// Gets the collection name for error logs.
    /// </summary>
    public IMongoCollection<LogEntry> ErrorLogs =>
        _database.GetCollection<LogEntry>("ErrorLogs");

    /// <summary>
    /// Gets the collection name for event logs.
    /// </summary>
    public IMongoCollection<IEvent> EventLogs =>
        _database.GetCollection<IEvent>("EventLogs");
}
