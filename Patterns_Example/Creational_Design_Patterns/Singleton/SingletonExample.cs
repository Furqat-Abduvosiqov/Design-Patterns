using Patterns_Example.Creational_Design_Patterns.Singleton.DatabaseConnectionPool;

namespace Patterns_Example.Creational_Design_Patterns.Singleton;

/// <summary>
/// Example class demonstrating the Singleton pattern usage
/// </summary>
public static class SingletonExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Singleton Pattern Example: Application Infrastructure ===\n");
        
        // Demonstrate Configuration Manager
        DemonstrateConfigurationManager();
        
        // Demonstrate Logger
        DemonstrateLogger();
        
        // Demonstrate Database Connection Pool
        DemonstrateDatabaseConnectionPool();
        
        // Demonstrate Application Cache
        DemonstrateApplicationCache();
        
        // Demonstrate Thread Safety
        DemonstrateThreadSafety();
        
        Console.WriteLine("=== Singleton Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Single instance ensures consistent state across application");
        Console.WriteLine("✓ Global access point for shared resources");
        Console.WriteLine("✓ Lazy initialization saves memory and startup time");
        Console.WriteLine("✓ Thread-safe implementation prevents race conditions");
        Console.WriteLine("✓ Controlled access to expensive resources");
        Console.WriteLine("✓ Centralized configuration and logging");
    }

    private static void DemonstrateConfigurationManager()
    {
        Console.WriteLine("1. Configuration Manager Singleton:");
        Console.WriteLine(new string('=', 50));
        
        // Get the singleton instance
        var config = ConfigurationManager.ConfigurationManager.Instance;
        
        Console.WriteLine("Default configuration settings:");
        var allSettings = config.GetAllSettings();
        foreach (var setting in allSettings.Take(5)) // Show first 5 settings
        {
            Console.WriteLine($"  {setting.Key}: {setting.Value}");
        }
        Console.WriteLine($"  ... and {allSettings.Count - 5} more settings");
        Console.WriteLine();
        
        // Demonstrate that multiple references point to the same instance
        var config2 = ConfigurationManager.ConfigurationManager.Instance;
        Console.WriteLine($"Same instance check: {ReferenceEquals(config, config2)}");
        
        // Modify configuration
        config.SetSetting("NewFeatureEnabled", "true");
        config.SetSetting("MaxRetries", "5");
        
        // Verify the change is visible from another reference
        Console.WriteLine($"NewFeatureEnabled from config2: {config2.GetSetting("NewFeatureEnabled")}");
        Console.WriteLine($"Configuration has {config.SettingsCount} settings");
        Console.WriteLine();
    }

    private static void DemonstrateLogger()
    {
        Console.WriteLine("2. Logger Singleton:");
        Console.WriteLine(new string('=', 50));
        
        var logger = Logger.Logger.Instance;
        
        // Log messages at different levels
        logger.Info("Application started", "Startup");
        logger.Debug("Loading configuration", "Config");
        logger.Warning("Using default database connection", "Database");
        logger.Error("Failed to connect to external service", "Network");
        logger.Critical("System running low on memory", "System");
        
        // Demonstrate that it's the same instance
        var logger2 = Logger.Logger.Instance;
        logger2.Info("Message from second logger reference", "Test");
        
        Console.WriteLine($"Same logger instance: {ReferenceEquals(logger, logger2)}");
        Console.WriteLine($"Total log entries: {logger.LogCount}");
        
        // Show log statistics
        var stats = logger.GetStatistics();
        Console.WriteLine(stats);
        Console.WriteLine();
    }

    private static void DemonstrateDatabaseConnectionPool()
    {
        Console.WriteLine("3. Database Connection Pool Singleton:");
        Console.WriteLine(new string('=', 50));
        
        var pool = DatabaseConnectionPool.DatabaseConnectionPool.Instance;
        
        Console.WriteLine($"Initial pool state: {pool}");
        
        // Get some connections
        var connections = new List<DatabaseConnection>();
        for (int i = 0; i < 3; i++)
        {
            var conn = pool.GetConnection();
            connections.Add(conn);
            Console.WriteLine($"Got connection: {conn}");
        }
        
        Console.WriteLine($"After getting 3 connections: {pool}");
        
        // Use the connections
        foreach (var conn in connections)
        {
            conn.ExecuteQuery($"SELECT * FROM Users WHERE Id = {Random.Shared.Next(1, 100)}");
        }
        
        // Return connections to pool
        foreach (var conn in connections)
        {
            pool.ReturnConnection(conn);
        }
        
        Console.WriteLine($"After returning connections: {pool}");
        
        // Show pool statistics
        var poolStats = pool.GetStatistics();
        Console.WriteLine(poolStats);
        Console.WriteLine();
    }

    private static void DemonstrateApplicationCache()
    {
        Console.WriteLine("4. Application Cache Singleton:");
        Console.WriteLine(new string('=', 50));
        
        var cache = ApplicationCache.ApplicationCache.Instance;
        
        // Cache some data
        cache.Set("user:123", new { Id = 123, Name = "John Doe", Email = "john@example.com" });
        cache.Set("product:456", new { Id = 456, Name = "Laptop", Price = 999.99 }, TimeSpan.FromMinutes(30));
        cache.Set("config:theme", "dark-mode", TimeSpan.FromHours(1));
        cache.Set("temp:session", Guid.NewGuid().ToString(), TimeSpan.FromMinutes(5));
        
        Console.WriteLine($"Cached 4 items. Cache count: {cache.Count}");
        
        // Retrieve cached data
        var user = cache.Get<object>("user:123");
        var theme = cache.Get<string>("config:theme");
        var nonExistent = cache.Get<string>("not:found");
        
        Console.WriteLine($"Retrieved user: {user}");
        Console.WriteLine($"Retrieved theme: {theme}");
        Console.WriteLine($"Non-existent key result: {nonExistent ?? "null"}");
        
        // Show cache statistics
        var cacheStats = cache.GetStatistics();
        Console.WriteLine(cacheStats);
        Console.WriteLine();
    }

    private static void DemonstrateThreadSafety()
    {
        Console.WriteLine("5. Thread Safety Demonstration:");
        Console.WriteLine(new string('=', 50));
        
        const int threadCount = 5;
        const int operationsPerThread = 10;
        var tasks = new List<Task>();
        var instances = new List<ConfigurationManager.ConfigurationManager>();
        var lockObject = new object();
        
        // Create multiple threads that access singletons
        for (int i = 0; i < threadCount; i++)
        {
            int threadId = i;
            var task = Task.Run(() =>
            {
                for (int j = 0; j < operationsPerThread; j++)
                {
                    // Access configuration manager
                    var config = ConfigurationManager.ConfigurationManager.Instance;
                    config.SetSetting($"Thread{threadId}_Setting{j}", $"Value from thread {threadId}");
                    
                    // Access logger
                    var logger = Logger.Logger.Instance;
                    logger.Info($"Operation {j} from thread {threadId}", $"Thread{threadId}");
                    
                    // Access cache
                    var cache = ApplicationCache.ApplicationCache.Instance;
                    cache.Set($"thread{threadId}:item{j}", $"Data from thread {threadId}, operation {j}");
                    
                    lock (lockObject)
                    {
                        instances.Add(config);
                    }
                    
                    Thread.Sleep(10); // Simulate some work
                }
            });
            tasks.Add(task);
        }
        
        // Wait for all threads to complete
        Task.WaitAll(tasks.ToArray());
        
        // Verify all instances are the same
        var uniqueInstances = instances.Distinct().Count();
        Console.WriteLine($"Created {instances.Count} references across {threadCount} threads");
        Console.WriteLine($"Unique instances: {uniqueInstances} (should be 1)");
        Console.WriteLine($"Thread safety verified: {uniqueInstances == 1}");
        
        // Show final state
        var config = ConfigurationManager.ConfigurationManager.Instance;
        var logger = Logger.Logger.Instance;
        var cache = ApplicationCache.ApplicationCache.Instance;
        
        Console.WriteLine($"Final configuration settings: {config.SettingsCount}");
        Console.WriteLine($"Final log entries: {logger.LogCount}");
        Console.WriteLine($"Final cache items: {cache.Count}");
        Console.WriteLine();
    }
}
