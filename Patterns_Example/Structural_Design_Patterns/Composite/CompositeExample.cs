namespace Patterns_Example.Structural_Design_Patterns.Composite;

/// <summary>
/// Example class demonstrating the Composite pattern usage
/// </summary>
public static class CompositeExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Composite Pattern Example: File System Management ===\n");
        
        // Demonstrate basic composite structure
        DemonstrateBasicCompositeStructure();
        
        // Demonstrate uniform treatment
        DemonstrateUniformTreatment();
        
        // Demonstrate search operations
        DemonstrateSearchOperations();
        
        // Demonstrate visitor pattern integration
        DemonstrateVisitorPattern();
        
        // Demonstrate archive functionality
        DemonstrateArchiveFunctionality();
        
        Console.WriteLine("=== Composite Pattern Benefits Demonstrated ===");
        Console.WriteLine("✓ Uniform treatment of individual objects and compositions");
        Console.WriteLine("✓ Simplified client code - same interface for all components");
        Console.WriteLine("✓ Easy to add new component types");
        Console.WriteLine("✓ Recursive operations on tree structures");
        Console.WriteLine("✓ Flexible hierarchy management");
        Console.WriteLine("✓ Integration with other patterns (Visitor)");
    }

    private static void DemonstrateBasicCompositeStructure()
    {
        Console.WriteLine("1. Basic Composite Structure:");
        Console.WriteLine(new string('=', 50));
        
        // Create root directory
        var root = new Directory("MyProject", "/home/user/MyProject");
        
        // Create files in root
        root.CreateFile("README.md", "# My Project\nThis is a sample project demonstrating the Composite pattern.");
        root.CreateFile("LICENSE", "MIT License\n\nCopyright (c) 2024");
        
        // Create subdirectories
        var srcDir = root.CreateSubdirectory("src");
        var docsDir = root.CreateSubdirectory("docs");
        var testsDir = root.CreateSubdirectory("tests");
        
        // Add files to src directory
        srcDir.CreateFile("Program.cs", "using System;\n\nnamespace MyProject\n{\n    class Program\n    {\n        static void Main(string[] args)\n        {\n            Console.WriteLine(\"Hello World!\");\n        }\n    }\n}");
        srcDir.CreateFile("Utils.cs", "using System;\n\nnamespace MyProject\n{\n    public static class Utils\n    {\n        public static void Log(string message)\n        {\n            Console.WriteLine($\"[{DateTime.Now}] {message}\");\n        }\n    }\n}");
        
        // Add files to docs directory
        docsDir.CreateFile("API.md", "# API Documentation\n\n## Classes\n\n### Program\nMain entry point of the application.");
        docsDir.CreateFile("INSTALL.md", "# Installation Guide\n\n1. Clone the repository\n2. Run dotnet build\n3. Run dotnet run");
        
        // Add files to tests directory
        testsDir.CreateFile("ProgramTests.cs", "using Xunit;\n\nnamespace MyProject.Tests\n{\n    public class ProgramTests\n    {\n        [Fact]\n        public void TestMain()\n        {\n            // Test implementation\n        }\n    }\n}");
        
        // Display the entire structure
        Console.WriteLine("Project structure:");
        root.Display();
        Console.WriteLine();
    }

    private static void DemonstrateUniformTreatment()
    {
        Console.WriteLine("2. Uniform Treatment of Components:");
        Console.WriteLine(new string('=', 50));
        
        // Create a mixed collection of components
        var components = new List<IFileSystemComponent>();
        
        // Add individual files
        components.Add(new File("config.json", "/app/config.json", 256));
        components.Add(new File("app.exe", "/app/app.exe", 1024000));
        
        // Add a directory with contents
        var libDir = new Directory("lib", "/app/lib");
        libDir.CreateFile("library1.dll", "Binary content for library 1");
        libDir.CreateFile("library2.dll", "Binary content for library 2");
        components.Add(libDir);
        
        // Add an archive
        var archive = new Archive("backup.zip", "/app/backup.zip", "zip");
        archive.Add(new File("data.txt", "/temp/data.txt", 512));
        archive.Add(new File("settings.ini", "/temp/settings.ini", 128));
        archive.Compress(0.6); // 60% compression
        components.Add(archive);
        
        // Treat all components uniformly
        Console.WriteLine("Processing all components uniformly:");
        foreach (var component in components)
        {
            Console.WriteLine($"Component: {component.Name}");
            Console.WriteLine($"  Type: {component.GetType().Name}");
            Console.WriteLine($"  Size: {FormatFileSize(component.Size)}");
            Console.WriteLine($"  Path: {component.Path}");
            Console.WriteLine($"  Permissions: {component.Permissions}");
            Console.WriteLine();
        }
    }

    private static void DemonstrateSearchOperations()
    {
        Console.WriteLine("3. Search Operations:");
        Console.WriteLine(new string('=', 50));
        
        // Create a complex directory structure
        var projectRoot = CreateSampleProject();
        
        // Search for files containing "test"
        Console.WriteLine("Searching for items containing 'test':");
        var searchOptions = new SearchOptions
        {
            CaseSensitive = false,
            SearchContent = true,
            MaxDepth = 10
        };
        
        var testResults = projectRoot.Search("test", searchOptions);
        foreach (var result in testResults)
        {
            Console.WriteLine($"  Found: {result.Name} ({result.GetType().Name}) at {result.Path}");
        }
        Console.WriteLine();
        
        // Find all files
        Console.WriteLine("Finding all files in the project:");
        var allFiles = projectRoot.FindByType(FileSystemType.File);
        Console.WriteLine($"  Total files found: {allFiles.Count}");
        foreach (var file in allFiles.Take(5)) // Show first 5
        {
            Console.WriteLine($"    {file.Name} ({FormatFileSize(file.Size)})");
        }
        if (allFiles.Count > 5)
        {
            Console.WriteLine($"    ... and {allFiles.Count - 5} more files");
        }
        Console.WriteLine();
        
        // Find large files (> 500 bytes)
        Console.WriteLine("Finding large files (> 500 bytes):");
        var largeFiles = projectRoot.FindBySize(500, long.MaxValue);
        foreach (var file in largeFiles)
        {
            Console.WriteLine($"  {file.Name}: {FormatFileSize(file.Size)}");
        }
        Console.WriteLine();
    }

    private static void DemonstrateVisitorPattern()
    {
        Console.WriteLine("4. Visitor Pattern Integration:");
        Console.WriteLine(new string('=', 50));
        
        var projectRoot = CreateSampleProject();
        
        // Statistics visitor
        Console.WriteLine("Collecting file system statistics...");
        var statsVisitor = new FileSystemStatisticsVisitor();
        projectRoot.Accept(statsVisitor);
        statsVisitor.PrintStatistics();
        Console.WriteLine();
        
        // Backup visitor
        Console.WriteLine("Performing backup operation...");
        var backupVisitor = new BackupVisitor("/backup/project_backup");
        projectRoot.Accept(backupVisitor);
        backupVisitor.PrintBackupSummary();
        Console.WriteLine();
        
        // Security scan visitor
        Console.WriteLine("Performing security scan...");
        var securityVisitor = new SecurityScanVisitor();
        projectRoot.Accept(securityVisitor);
        securityVisitor.PrintSecurityReport();
        Console.WriteLine();
    }

    private static void DemonstrateArchiveFunctionality()
    {
        Console.WriteLine("5. Archive Functionality:");
        Console.WriteLine(new string('=', 50));
        
        // Create an archive and add files to it
        var archive = new Archive("project_backup.zip", "/backups/project_backup.zip", "zip");
        
        // Add individual files
        archive.Add(new File("important_data.txt", "/temp/important_data.txt", 1024));
        archive.Add(new File("config.json", "/temp/config.json", 512));
        
        // Add a directory structure
        var configDir = new Directory("config", "/temp/config");
        configDir.CreateFile("database.conf", "host=localhost\nport=5432\nuser=admin");
        configDir.CreateFile("app.conf", "debug=true\nlog_level=info");
        archive.Add(configDir);
        
        // Compress the archive
        archive.Compress(0.7); // 70% compression ratio
        
        Console.WriteLine("Archive created:");
        archive.Display();
        Console.WriteLine();
        
        // Extract the archive
        var extractionDir = new Directory("extracted", "/temp/extracted");
        Console.WriteLine("Extracting archive...");
        archive.Extract(extractionDir);
        Console.WriteLine();
        
        Console.WriteLine("Extracted contents:");
        extractionDir.Display();
        Console.WriteLine();
    }

    private static Directory CreateSampleProject()
    {
        var root = new Directory("SampleProject", "/projects/SampleProject");
        
        // Root files
        root.CreateFile("README.md", "# Sample Project\nThis is a demonstration project for the Composite pattern.\n\n## Features\n- File management\n- Directory operations\n- Search functionality");
        root.CreateFile("LICENSE", "MIT License\n\nPermission is hereby granted, free of charge, to any person obtaining a copy of this software...");
        root.CreateFile(".gitignore", "bin/\nobj/\n*.user\n*.suo\n.vs/");
        
        // Source directory
        var src = root.CreateSubdirectory("src");
        src.CreateFile("Program.cs", "using System;\nusing System.IO;\n\nnamespace SampleProject\n{\n    class Program\n    {\n        static void Main(string[] args)\n        {\n            Console.WriteLine(\"Sample Project Running\");\n            // Application logic here\n        }\n    }\n}");
        src.CreateFile("FileManager.cs", "using System;\nusing System.Collections.Generic;\n\nnamespace SampleProject\n{\n    public class FileManager\n    {\n        public void ProcessFiles()\n        {\n            // File processing logic\n        }\n    }\n}");
        src.CreateFile("password_manager.cs", "// This file contains sensitive password management code\nusing System.Security;\n\nnamespace SampleProject\n{\n    public class PasswordManager\n    {\n        private string secretKey = \"top_secret\";\n    }\n}");
        
        // Tests directory
        var tests = root.CreateSubdirectory("tests");
        tests.CreateFile("ProgramTests.cs", "using Xunit;\nusing SampleProject;\n\nnamespace SampleProject.Tests\n{\n    public class ProgramTests\n    {\n        [Fact]\n        public void TestMainExecution()\n        {\n            // Test the main program execution\n            Assert.True(true);\n        }\n    }\n}");
        tests.CreateFile("FileManagerTests.cs", "using Xunit;\nusing SampleProject;\n\nnamespace SampleProject.Tests\n{\n    public class FileManagerTests\n    {\n        [Fact]\n        public void TestFileProcessing()\n        {\n            var manager = new FileManager();\n            // Test file processing\n        }\n    }\n}");
        
        // Documentation directory
        var docs = root.CreateSubdirectory("docs");
        docs.CreateFile("API.md", "# API Documentation\n\n## Classes\n\n### Program\nMain entry point\n\n### FileManager\nHandles file operations");
        docs.CreateFile("CONTRIBUTING.md", "# Contributing Guidelines\n\n1. Fork the repository\n2. Create a feature branch\n3. Make your changes\n4. Submit a pull request");
        
        // Binary directory with executable
        var bin = root.CreateSubdirectory("bin");
        var executable = new File("app.exe", "/projects/SampleProject/bin/app.exe", 2048000);
        executable.SetPermissions(new FileSystemPermissions
        {
            CanRead = true,
            CanWrite = true,
            CanExecute = true,
            CanDelete = true
        });
        bin.Add(executable);
        
        return root;
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
