namespace Patterns_Example.Creational_Design_Patterns.Singleton.DatabaseConnectionPool;

/// <summary>
/// Connection pool statistics.
/// </summary>
public class PoolStatistics
{
    public int MaxConnections { get; set; }
    public int AvailableConnections { get; set; }
    public int UsedConnections { get; set; }
    public int TotalConnectionsCreated { get; set; }
    public string ConnectionString { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"""
                Connection Pool Statistics:
                  Max Connections: {MaxConnections}
                  Available: {AvailableConnections}
                  In Use: {UsedConnections}
                  Total Created: {TotalConnectionsCreated}
                  Utilization: {(double)UsedConnections / MaxConnections * 100:F1}%
                """;
    }
}