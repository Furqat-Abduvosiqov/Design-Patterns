namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Visitor that calculates statistics about the file system.
/// Demonstrates the Visitor pattern working with the Composite pattern.
/// </summary>
public class FileSystemStatisticsVisitor : IFileSystemVisitor
{
    public int TotalFiles { get; private set; }
    public int TotalDirectories { get; private set; }
    public int TotalArchives { get; private set; }
    public long TotalSize { get; private set; }
    public long LargestFileSize { get; private set; }
    public string? LargestFileName { get; private set; }
    public Dictionary<string, int> FileExtensions { get; private set; }
    public Dictionary<string, long> DirectorySizes { get; private set; }
    public DateTime OldestFile { get; private set; } = DateTime.MaxValue;
    public DateTime NewestFile { get; private set; } = DateTime.MinValue;

    public FileSystemStatisticsVisitor()
    {
        FileExtensions = new Dictionary<string, int>();
        DirectorySizes = new Dictionary<string, long>();
    }

    public void VisitFile(File file)
    {
        TotalFiles++;
        TotalSize += file.Size;
        
        if (file.Size > LargestFileSize)
        {
            LargestFileSize = file.Size;
            LargestFileName = file.Name;
        }
        
        var extension = System.IO.Path.GetExtension(file.Name).ToLower();
        if (string.IsNullOrEmpty(extension))
            extension = "[no extension]";
            
        FileExtensions[extension] = FileExtensions.GetValueOrDefault(extension, 0) + 1;
        
        if (file.CreatedDate < OldestFile)
            OldestFile = file.CreatedDate;
        if (file.CreatedDate > NewestFile)
            NewestFile = file.CreatedDate;
    }

    public void VisitDirectory(Directory directory)
    {
        TotalDirectories++;
        DirectorySizes[directory.Name] = directory.Size;
        
        if (directory.CreatedDate < OldestFile)
            OldestFile = directory.CreatedDate;
        if (directory.CreatedDate > NewestFile)
            NewestFile = directory.CreatedDate;
    }

    public void VisitArchive(Archive archive)
    {
        TotalArchives++;
        TotalSize += archive.Size;
        
        if (archive.Size > LargestFileSize)
        {
            LargestFileSize = archive.Size;
            LargestFileName = archive.Name;
        }
        
        if (archive.CreatedDate < OldestFile)
            OldestFile = archive.CreatedDate;
        if (archive.CreatedDate > NewestFile)
            NewestFile = archive.CreatedDate;
    }

    public void PrintStatistics()
    {
        Console.WriteLine("📊 File System Statistics:");
        Console.WriteLine($"   Total Files: {TotalFiles}");
        Console.WriteLine($"   Total Directories: {TotalDirectories}");
        Console.WriteLine($"   Total Archives: {TotalArchives}");
        Console.WriteLine($"   Total Size: {FormatFileSize(TotalSize)}");
        Console.WriteLine($"   Largest File: {LargestFileName} ({FormatFileSize(LargestFileSize)})");
        Console.WriteLine($"   Oldest Item: {(OldestFile == DateTime.MaxValue ? "None" : OldestFile.ToString("yyyy-MM-dd HH:mm"))}");
        Console.WriteLine($"   Newest Item: {(NewestFile == DateTime.MinValue ? "None" : NewestFile.ToString("yyyy-MM-dd HH:mm"))}");
        
        if (FileExtensions.Count > 0)
        {
            Console.WriteLine("   File Extensions:");
            foreach (var ext in FileExtensions.OrderByDescending(kv => kv.Value))
            {
                Console.WriteLine($"     {ext.Key}: {ext.Value} files");
            }
        }
        
        if (DirectorySizes.Count > 0)
        {
            Console.WriteLine("   Directory Sizes:");
            foreach (var dir in DirectorySizes.OrderByDescending(kv => kv.Value).Take(5))
            {
                Console.WriteLine($"     {dir.Key}: {FormatFileSize(dir.Value)}");
            }
        }
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;
        
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        
        return $"{number:n1} {suffixes[counter]}";
    }
}

/// <summary>
/// Visitor that performs backup operations on the file system.
/// </summary>
public class BackupVisitor : IFileSystemVisitor
{
    private readonly string _backupPath;
    private readonly List<string> _backedUpItems;
    private long _totalBackupSize;

    public BackupVisitor(string backupPath)
    {
        _backupPath = backupPath ?? throw new ArgumentNullException(nameof(backupPath));
        _backedUpItems = new List<string>();
        _totalBackupSize = 0;
    }

    public void VisitFile(File file)
    {
        Console.WriteLine($"   📄 Backing up file: {file.Name} ({FormatFileSize(file.Size)})");
        _backedUpItems.Add($"File: {file.Path}");
        _totalBackupSize += file.Size;
        
        // Simulate backup delay
        Thread.Sleep(10);
    }

    public void VisitDirectory(Directory directory)
    {
        Console.WriteLine($"   📁 Backing up directory: {directory.Name}/");
        _backedUpItems.Add($"Directory: {directory.Path}");
        
        // Simulate backup delay
        Thread.Sleep(5);
    }

    public void VisitArchive(Archive archive)
    {
        Console.WriteLine($"   🗜️  Backing up archive: {archive.Name} ({FormatFileSize(archive.Size)})");
        _backedUpItems.Add($"Archive: {archive.Path}");
        _totalBackupSize += archive.Size;
        
        // Simulate backup delay
        Thread.Sleep(15);
    }

    public void PrintBackupSummary()
    {
        Console.WriteLine($"\n💾 Backup Summary:");
        Console.WriteLine($"   Backup Location: {_backupPath}");
        Console.WriteLine($"   Items Backed Up: {_backedUpItems.Count}");
        Console.WriteLine($"   Total Size: {FormatFileSize(_totalBackupSize)}");
        Console.WriteLine($"   Backup Completed: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;
        
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        
        return $"{number:n1} {suffixes[counter]}";
    }
}

/// <summary>
/// Visitor that performs security scanning on the file system.
/// </summary>
public class SecurityScanVisitor : IFileSystemVisitor
{
    private readonly List<SecurityIssue> _issues;
    private int _scannedFiles;
    private int _scannedDirectories;
    private int _scannedArchives;

    public SecurityScanVisitor()
    {
        _issues = new List<SecurityIssue>();
    }

    public void VisitFile(File file)
    {
        _scannedFiles++;
        
        // Check for security issues
        if (file.Name.ToLower().Contains("password") || file.Name.ToLower().Contains("secret"))
        {
            _issues.Add(new SecurityIssue
            {
                Type = SecurityIssueType.SensitiveFileName,
                Path = file.Path,
                Description = "File name suggests sensitive content",
                Severity = SecuritySeverity.Medium
            });
        }
        
        if (file.Permissions.CanWrite && file.Permissions.CanExecute)
        {
            _issues.Add(new SecurityIssue
            {
                Type = SecurityIssueType.OverlyPermissive,
                Path = file.Path,
                Description = "File has both write and execute permissions",
                Severity = SecuritySeverity.High
            });
        }
        
        var extension = System.IO.Path.GetExtension(file.Name).ToLower();
        if (extension == ".exe" || extension == ".bat" || extension == ".cmd")
        {
            _issues.Add(new SecurityIssue
            {
                Type = SecurityIssueType.ExecutableFile,
                Path = file.Path,
                Description = "Executable file detected",
                Severity = SecuritySeverity.Low
            });
        }
        
        Console.WriteLine($"   🔍 Scanning file: {file.Name}");
        Thread.Sleep(5); // Simulate scanning time
    }

    public void VisitDirectory(Directory directory)
    {
        _scannedDirectories++;
        
        // Check directory permissions
        if (!directory.Permissions.CanRead)
        {
            _issues.Add(new SecurityIssue
            {
                Type = SecurityIssueType.AccessDenied,
                Path = directory.Path,
                Description = "Directory is not readable",
                Severity = SecuritySeverity.Medium
            });
        }
        
        Console.WriteLine($"   🔍 Scanning directory: {directory.Name}/");
        Thread.Sleep(2);
    }

    public void VisitArchive(Archive archive)
    {
        _scannedArchives++;
        
        // Archives can be security risks
        _issues.Add(new SecurityIssue
        {
            Type = SecurityIssueType.ArchiveFile,
            Path = archive.Path,
            Description = "Archive file may contain malicious content",
            Severity = SecuritySeverity.Low
        });
        
        Console.WriteLine($"   🔍 Scanning archive: {archive.Name}");
        Thread.Sleep(8);
    }

    public void PrintSecurityReport()
    {
        Console.WriteLine($"\n🔒 Security Scan Report:");
        Console.WriteLine($"   Files Scanned: {_scannedFiles}");
        Console.WriteLine($"   Directories Scanned: {_scannedDirectories}");
        Console.WriteLine($"   Archives Scanned: {_scannedArchives}");
        Console.WriteLine($"   Security Issues Found: {_issues.Count}");
        
        if (_issues.Count > 0)
        {
            Console.WriteLine("\n   Issues by Severity:");
            var groupedIssues = _issues.GroupBy(i => i.Severity);
            
            foreach (var group in groupedIssues.OrderByDescending(g => g.Key))
            {
                Console.WriteLine($"     {group.Key}: {group.Count()} issues");
                foreach (var issue in group.Take(3)) // Show first 3 issues of each severity
                {
                    Console.WriteLine($"       - {issue.Description} ({issue.Path})");
                }
                if (group.Count() > 3)
                {
                    Console.WriteLine($"       ... and {group.Count() - 3} more");
                }
            }
        }
        else
        {
            Console.WriteLine("   ✅ No security issues found!");
        }
    }
}

/// <summary>
/// Represents a security issue found during scanning.
/// </summary>
public class SecurityIssue
{
    public SecurityIssueType Type { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SecuritySeverity Severity { get; set; }
}

/// <summary>
/// Types of security issues.
/// </summary>
public enum SecurityIssueType
{
    SensitiveFileName,
    OverlyPermissive,
    ExecutableFile,
    ArchiveFile,
    AccessDenied
}

/// <summary>
/// Security issue severity levels.
/// </summary>
public enum SecuritySeverity
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
