namespace Patterns_Example.Creational_Design_Patterns.Singleton.DatabaseConnectionPool;

/// <summary>
/// Represents a database connection.
/// </summary>
public class DatabaseConnection : IDisposable
{
    public int Id { get; }
    public string ConnectionString { get; }
    public DateTime CreatedAt { get; }
    public DateTime LastUsed { get; set; }
    public bool IsValid { get; private set; }
    public bool IsDisposed { get; private set; }

    public DatabaseConnection(string connectionString, int id)
    {
        Id = id;
        ConnectionString = connectionString;
        CreatedAt = DateTime.Now;
        LastUsed = DateTime.Now;
        IsValid = true;
        IsDisposed = false;
        
        // Simulate connection establishment
        Thread.Sleep(10); // Simulate connection time
    }

    /// <summary>
    /// Simulates executing a query.
    /// </summary>
    public void ExecuteQuery(string query)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(DatabaseConnection));

        if (!IsValid)
            throw new InvalidOperationException("Connection is not valid");

        LastUsed = DateTime.Now;
        
        // Simulate query execution
        Logger.Logger.Instance.Debug($"Executing query on connection {Id}: {query}", "Database");
        Thread.Sleep(Random.Shared.Next(10, 50)); // Simulate query time
    }

    /// <summary>
    /// Simulates connection validation.
    /// </summary>
    public bool ValidateConnection()
    {
        if (IsDisposed)
            return false;

        // Simulate validation logic
        var timeSinceCreation = DateTime.Now - CreatedAt;
        IsValid = timeSinceCreation.TotalMinutes < 30; // Connection expires after 30 minutes
        
        return IsValid;
    }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsValid = false;
            IsDisposed = true;
            Logger.Logger.Instance.Debug($"Disposed connection {Id}", "Database");
        }
    }

    public override string ToString()
    {
        return $"Connection {Id}: Created {CreatedAt:HH:mm:ss}, Last Used {LastUsed:HH:mm:ss}, Valid: {IsValid}";
    }
}