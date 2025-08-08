namespace Patterns_Example.Creational_Design_Patterns.Singleton.DatabaseConnectionPool;

/// <summary>
/// Thread-safe Singleton Database Connection Pool.
/// Demonstrates the Singleton pattern for managing shared resources.
/// </summary>
public sealed class DatabaseConnectionPool
{
    private static readonly object _lock = new object();
    private static DatabaseConnectionPool? _instance;
    private readonly Queue<DatabaseConnection> _availableConnections;
    private readonly HashSet<DatabaseConnection> _usedConnections;
    private readonly object _poolLock = new object();
    private readonly int _maxConnections;
    private readonly string _connectionString;
    private int _totalConnectionsCreated;

    public int MaxConnections => _maxConnections;
    public int AvailableConnections 
    { 
        get 
        { 
            lock (_poolLock) 
            { 
                return _availableConnections.Count; 
            } 
        } 
    }
    public int UsedConnections 
    { 
        get 
        { 
            lock (_poolLock) 
            { 
                return _usedConnections.Count; 
            } 
        } 
    }
    public int TotalConnectionsCreated => _totalConnectionsCreated;

    /// <summary>
    /// Private constructor prevents direct instantiation.
    /// </summary>
    private DatabaseConnectionPool()
    {
        _maxConnections = 10; // Default pool size
        _connectionString = ConfigurationManager.ConfigurationManager.Instance.GetSetting("DatabaseConnectionString");
        _availableConnections = new Queue<DatabaseConnection>();
        _usedConnections = new HashSet<DatabaseConnection>();
        _totalConnectionsCreated = 0;
        
        // Pre-create some connections
        InitializePool();
    }

    /// <summary>
    /// Thread-safe singleton instance access.
    /// </summary>
    public static DatabaseConnectionPool Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnectionPool();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Gets a connection from the pool.
    /// </summary>
    public DatabaseConnection GetConnection()
    {
        lock (_poolLock)
        {
            DatabaseConnection connection;

            if (_availableConnections.Count > 0)
            {
                // Reuse existing connection
                connection = _availableConnections.Dequeue();
                Logger.Logger.Instance.Debug($"Reusing connection {connection.Id}", "ConnectionPool");
            }
            else if (_usedConnections.Count < _maxConnections)
            {
                // Create new connection
                connection = CreateNewConnection();
                Logger.Logger.Instance.Info($"Created new connection {connection.Id}", "ConnectionPool");
            }
            else
            {
                // Pool is exhausted, wait or throw exception
                throw new InvalidOperationException($"Connection pool exhausted. Maximum {_maxConnections} connections allowed.");
            }

            _usedConnections.Add(connection);
            connection.LastUsed = DateTime.Now;
            return connection;
        }
    }

    /// <summary>
    /// Returns a connection to the pool.
    /// </summary>
    public void ReturnConnection(DatabaseConnection connection)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        lock (_poolLock)
        {
            if (_usedConnections.Remove(connection))
            {
                if (connection.IsValid)
                {
                    _availableConnections.Enqueue(connection);
                    Logger.Logger.Instance.Debug($"Returned connection {connection.Id} to pool", "ConnectionPool");
                }
                else
                {
                    // Connection is invalid, create a new one to maintain pool size
                    Logger.Logger.Instance.Warning($"Connection {connection.Id} is invalid, discarding", "ConnectionPool");
                    connection.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// Closes all connections and clears the pool.
    /// </summary>
    public void CloseAllConnections()
    {
        lock (_poolLock)
        {
            // Close available connections
            while (_availableConnections.Count > 0)
            {
                var connection = _availableConnections.Dequeue();
                connection.Dispose();
            }

            // Close used connections (this might interrupt ongoing operations)
            foreach (var connection in _usedConnections.ToList())
            {
                connection.Dispose();
            }
            _usedConnections.Clear();

            Logger.Logger.Instance.Info("All database connections closed", "ConnectionPool");
        }
    }

    /// <summary>
    /// Gets pool statistics.
    /// </summary>
    public PoolStatistics GetStatistics()
    {
        lock (_poolLock)
        {
            return new PoolStatistics
            {
                MaxConnections = _maxConnections,
                AvailableConnections = _availableConnections.Count,
                UsedConnections = _usedConnections.Count,
                TotalConnectionsCreated = _totalConnectionsCreated,
                ConnectionString = _connectionString
            };
        }
    }

    /// <summary>
    /// Initializes the connection pool with some pre-created connections.
    /// </summary>
    private void InitializePool()
    {
        var initialConnections = Math.Min(3, _maxConnections); // Create 3 initial connections
        
        for (int i = 0; i < initialConnections; i++)
        {
            var connection = CreateNewConnection();
            _availableConnections.Enqueue(connection);
        }
        
        Logger.Logger.Instance.Info($"Initialized connection pool with {initialConnections} connections", "ConnectionPool");
    }

    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    private DatabaseConnection CreateNewConnection()
    {
        var connection = new DatabaseConnection(_connectionString, ++_totalConnectionsCreated);
        return connection;
    }

    public override string ToString()
    {
        return $"DatabaseConnectionPool: {UsedConnections}/{MaxConnections} used, {AvailableConnections} available";
    }
}