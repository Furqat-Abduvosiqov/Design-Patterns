namespace Patterns_Example.Structural_Design_Patterns.Proxy;

/// <summary>
/// Example class demonstrating the Proxy pattern usage
/// </summary>
public static class ProxyExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Proxy Pattern Example: Virtual File System ===\n");
        
        // Demonstrate the problem without proxy
        DemonstrateProblemsWithoutProxy();
        
        // Demonstrate basic proxy functionality
        DemonstrateBasicProxyUsage();
        
        // Demonstrate access control proxy
        DemonstrateAccessControlProxy();
        
        // Demonstrate caching proxy
        DemonstrateCachingProxy();
        
        // Demonstrate lazy initialization
        DemonstrateLazyInitialization();
        
        // Demonstrate audit logging
        DemonstrateAuditLogging();
        
        Console.WriteLine("=== Proxy Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Controlled access to expensive resources");
        Console.WriteLine("✓ Lazy initialization of heavy objects");
        Console.WriteLine("✓ Caching for improved performance");
        Console.WriteLine("✓ Access control and security");
        Console.WriteLine("✓ Audit logging and monitoring");
        Console.WriteLine("✓ Transparent interface - clients don't know about proxy");
    }

    private static void DemonstrateProblemsWithoutProxy()
    {
        Console.WriteLine("1. Problems WITHOUT Proxy Pattern:");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("❌ Direct access to expensive resources:");
        Console.WriteLine();
        
        Console.WriteLine("// Without proxy - direct instantiation");
        Console.WriteLine("var fileSystem = new RealFileSystem(); // Expensive!");
        Console.WriteLine("// Problems:");
        Console.WriteLine("  • Immediate expensive initialization");
        Console.WriteLine("  • No access control");
        Console.WriteLine("  • No caching - repeated expensive operations");
        Console.WriteLine("  • No audit trail");
        Console.WriteLine("  • No lazy loading");
        Console.WriteLine();
        
        // Actually demonstrate the expensive initialization
        Console.WriteLine("Demonstrating expensive initialization:");
        var realFileSystem = new RealFileSystem();
        Console.WriteLine("✓ RealFileSystem created (notice the initialization delay)");
        Console.WriteLine();
    }

    private static void DemonstrateBasicProxyUsage()
    {
        Console.WriteLine("2. Basic Proxy Usage:");
        Console.WriteLine(new string('=', 50));
        
        // Create a user context
        var user = new UserContext
        {
            Username = "john_doe",
            Role = UserRole.User,
            IsAuthenticated = true
        };
        
        Console.WriteLine($"Creating proxy for user: {user}");
        
        // Create proxy (notice no expensive initialization yet)
        var fileSystemProxy = new FileSystemProxy(user);
        Console.WriteLine("✓ Proxy created instantly (no expensive initialization)");
        Console.WriteLine();
        
        // Use the proxy - this will trigger lazy initialization
        Console.WriteLine("First operation through proxy:");
        var content = fileSystemProxy.ReadFile("/documents/readme.txt");
        Console.WriteLine($"File content: {content[..50]}...");
        Console.WriteLine();
        
        // Subsequent operations use the already initialized real object
        Console.WriteLine("Subsequent operations (real object already initialized):");
        var files = fileSystemProxy.ListDirectory("/documents");
        Console.WriteLine($"Files in /documents: {string.Join(", ", files)}");
        Console.WriteLine();
    }

    private static void DemonstrateAccessControlProxy()
    {
        Console.WriteLine("3. Access Control Proxy:");
        Console.WriteLine(new string('=', 50));
        
        // Create different users with different roles
        var regularUser = new UserContext
        {
            Username = "regular_user",
            Role = UserRole.User,
            IsAuthenticated = true
        };
        
        var adminUser = new UserContext
        {
            Username = "admin_user",
            Role = UserRole.Administrator,
            IsAuthenticated = true
        };
        
        var guestUser = new UserContext
        {
            Username = "guest",
            Role = UserRole.Guest,
            IsAuthenticated = false
        };
        
        // Test access control with different users
        Console.WriteLine("Testing access control with different user roles:");
        Console.WriteLine();
        
        // Regular user accessing documents (should work)
        Console.WriteLine("Regular user accessing /documents/readme.txt:");
        var userProxy = new FileSystemProxy(regularUser);
        try
        {
            var content = userProxy.ReadFile("/documents/readme.txt");
            Console.WriteLine("✓ Access granted - file read successfully");
        }
        catch (AccessDeniedException ex)
        {
            Console.WriteLine($"❌ Access denied: {ex.Message}");
        }
        Console.WriteLine();
        
        // Regular user accessing system files (should fail)
        Console.WriteLine("Regular user accessing /system/config.sys:");
        try
        {
            var content = userProxy.ReadFile("/system/config.sys");
            Console.WriteLine("✓ Access granted - file read successfully");
        }
        catch (AccessDeniedException ex)
        {
            Console.WriteLine($"❌ Access denied: {ex.Message}");
        }
        Console.WriteLine();
        
        // Admin user accessing system files (should work)
        Console.WriteLine("Admin user accessing /system/config.sys:");
        var adminProxy = new FileSystemProxy(adminUser);
        try
        {
            var content = adminProxy.ReadFile("/system/config.sys");
            Console.WriteLine("✓ Access granted - file read successfully");
        }
        catch (AccessDeniedException ex)
        {
            Console.WriteLine($"❌ Access denied: {ex.Message}");
        }
        Console.WriteLine();
        
        // Guest user accessing any file (should fail)
        Console.WriteLine("Guest user (not authenticated) accessing /documents/readme.txt:");
        var guestProxy = new FileSystemProxy(guestUser);
        try
        {
            var content = guestProxy.ReadFile("/documents/readme.txt");
            Console.WriteLine("✓ Access granted - file read successfully");
        }
        catch (AccessDeniedException ex)
        {
            Console.WriteLine($"❌ Access denied: {ex.Message}");
        }
        Console.WriteLine();
    }

    private static void DemonstrateCachingProxy()
    {
        Console.WriteLine("4. Caching Proxy:");
        Console.WriteLine(new string('=', 50));
        
        var user = new UserContext
        {
            Username = "cache_user",
            Role = UserRole.User,
            IsAuthenticated = true
        };
        
        var config = new ProxyConfiguration
        {
            EnableCaching = true,
            CacheExpirationMinutes = 5
        };
        
        var proxy = new FileSystemProxy(user, config);
        
        Console.WriteLine("First read (will be cached):");
        var startTime = DateTime.Now;
        var content1 = proxy.ReadFile("/documents/readme.txt");
        var firstReadTime = DateTime.Now - startTime;
        Console.WriteLine($"First read took: {firstReadTime.TotalMilliseconds:F0}ms");
        Console.WriteLine();
        
        Console.WriteLine("Second read (from cache):");
        startTime = DateTime.Now;
        var content2 = proxy.ReadFile("/documents/readme.txt");
        var secondReadTime = DateTime.Now - startTime;
        Console.WriteLine($"Second read took: {secondReadTime.TotalMilliseconds:F0}ms");
        Console.WriteLine($"Speed improvement: {(firstReadTime.TotalMilliseconds / secondReadTime.TotalMilliseconds):F1}x faster");
        Console.WriteLine();
        
        // Show cache statistics
        var cacheStats = proxy.GetCacheStatistics();
        Console.WriteLine("Cache statistics:");
        Console.WriteLine(cacheStats);
        Console.WriteLine();
    }

    private static void DemonstrateLazyInitialization()
    {
        Console.WriteLine("5. Lazy Initialization:");
        Console.WriteLine(new string('=', 50));
        
        var user = new UserContext
        {
            Username = "lazy_user",
            Role = UserRole.User,
            IsAuthenticated = true
        };
        
        Console.WriteLine("Creating proxy (no real object created yet):");
        var startTime = DateTime.Now;
        var proxy = new FileSystemProxy(user);
        var creationTime = DateTime.Now - startTime;
        Console.WriteLine($"Proxy creation took: {creationTime.TotalMilliseconds:F0}ms");
        Console.WriteLine();
        
        Console.WriteLine("First operation (triggers real object creation):");
        startTime = DateTime.Now;
        var exists = proxy.FileExists("/documents/readme.txt");
        var firstOpTime = DateTime.Now - startTime;
        Console.WriteLine($"First operation took: {firstOpTime.TotalMilliseconds:F0}ms (includes initialization)");
        Console.WriteLine($"File exists: {exists}");
        Console.WriteLine();
        
        Console.WriteLine("Second operation (real object already exists):");
        startTime = DateTime.Now;
        var files = proxy.ListDirectory("/documents");
        var secondOpTime = DateTime.Now - startTime;
        Console.WriteLine($"Second operation took: {secondOpTime.TotalMilliseconds:F0}ms (no initialization)");
        Console.WriteLine($"Files found: {files.Count}");
        Console.WriteLine();
        
        Console.WriteLine($"Lazy initialization saved: {firstOpTime.TotalMilliseconds - secondOpTime.TotalMilliseconds:F0}ms on subsequent operations");
        Console.WriteLine();
    }

    private static void DemonstrateAuditLogging()
    {
        Console.WriteLine("6. Audit Logging:");
        Console.WriteLine(new string('=', 50));
        
        var user = new UserContext
        {
            Username = "audit_user",
            Role = UserRole.PowerUser,
            IsAuthenticated = true
        };
        
        var proxy = new FileSystemProxy(user);
        
        Console.WriteLine("Performing various operations (all logged):");
        
        // Perform various operations
        try
        {
            proxy.ReadFile("/documents/readme.txt");
            proxy.ListDirectory("/documents");
            proxy.WriteFile("/temp/test.txt", "Test content");
            proxy.GetFileInfo("/documents/readme.txt");
            proxy.ReadFile("/system/config.sys"); // This might fail due to permissions
        }
        catch (AccessDeniedException ex)
        {
            Console.WriteLine($"Expected access denial: {ex.Message}");
        }
        
        Console.WriteLine();
        Console.WriteLine("Audit log entries:");
        var auditLog = proxy.GetAuditLog();
        foreach (var entry in auditLog)
        {
            Console.WriteLine($"  {entry}");
        }
        Console.WriteLine();
        
        // Show audit statistics
        var successfulOps = auditLog.Count(e => e.Success);
        var failedOps = auditLog.Count(e => !e.Success);
        var avgDuration = auditLog.Average(e => e.Duration.TotalMilliseconds);
        
        Console.WriteLine("Audit statistics:");
        Console.WriteLine($"  Total operations: {auditLog.Count}");
        Console.WriteLine($"  Successful: {successfulOps}");
        Console.WriteLine($"  Failed: {failedOps}");
        Console.WriteLine($"  Average duration: {avgDuration:F1}ms");
        Console.WriteLine();
    }

    private static void DemonstrateAdvancedProxyFeatures()
    {
        Console.WriteLine("7. Advanced Proxy Features:");
        Console.WriteLine(new string('=', 50));
        
        var user = new UserContext
        {
            Username = "advanced_user",
            Role = UserRole.Administrator,
            IsAuthenticated = true
        };
        
        var config = new ProxyConfiguration
        {
            EnableCaching = true,
            EnableAuditing = true,
            EnableAccessControl = true,
            CacheExpirationMinutes = 1,
            MaxCacheEntries = 100
        };
        
        var proxy = new FileSystemProxy(user, config);
        
        Console.WriteLine("Testing file operations with full proxy features:");
        
        // Test file operations
        proxy.WriteFile("/temp/proxy_test.txt", "This is a test file created through proxy");
        var content = proxy.ReadFile("/temp/proxy_test.txt");
        Console.WriteLine($"Created and read file: {content}");
        
        // Test file copy
        proxy.CopyFile("/temp/proxy_test.txt", "/temp/proxy_test_copy.txt");
        Console.WriteLine("File copied successfully");
        
        // Test file move
        proxy.MoveFile("/temp/proxy_test_copy.txt", "/temp/proxy_test_moved.txt");
        Console.WriteLine("File moved successfully");
        
        // Test directory operations
        proxy.CreateDirectory("/temp/proxy_dir");
        var tempFiles = proxy.ListDirectory("/temp");
        Console.WriteLine($"Files in /temp: {string.Join(", ", tempFiles)}");
        
        // Show final statistics
        Console.WriteLine();
        Console.WriteLine("Final proxy statistics:");
        var finalCacheStats = proxy.GetCacheStatistics();
        Console.WriteLine(finalCacheStats);
        
        var finalAuditLog = proxy.GetAuditLog();
        Console.WriteLine($"Total operations logged: {finalAuditLog.Count}");
        Console.WriteLine();
    }
}
