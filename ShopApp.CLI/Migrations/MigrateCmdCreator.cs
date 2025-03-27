using System.CommandLine;

namespace ShopApp.CLI.Migrations;

public interface ICmdCreator
{
    Command Create();
}

public class MigrateCmdCreator : ICmdCreator
{
    private readonly MigrationRunner _migrationRunner;
    private readonly Option<string> _fileOption = new("--file", "Path to the SQL migration file");
    private readonly Option<bool> _allOption = new("--all", "Run all migration files");

    public MigrateCmdCreator(MigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    public Command Create()
    {
        var migrateCommand = new Command("migrate", "Run database migrations");

        migrateCommand.AddOption(_allOption);
        migrateCommand.AddOption(_fileOption);
        
        SetCmdHandler(migrateCommand);
        return migrateCommand;
    }

    private void SetCmdHandler(Command cmd)
    {
        cmd.SetHandler(CmdHandler, _fileOption, _allOption);
    }

    private async Task CmdHandler(string file, bool all)
    {
        if (all) {
            await RunAll();
        }
        else if (!string.IsNullOrEmpty(file)) {
            await RunFile(file);
        }
        else { // no options fallback
            RunNoOptions();
        }
    }

    private async Task RunAll()
    {
        string baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));
        string migrationsPath = Path.Combine(baseDir, "Migrations", "migrations_sql");

        Console.WriteLine($"Running all migrations from {migrationsPath}");

        if (Directory.Exists(migrationsPath))
        {
            foreach (var sqlFile in Directory.GetFiles(migrationsPath, "*.sql"))
            {
                Console.WriteLine($"Executing {Path.GetFileName(sqlFile)}...");
                await _migrationRunner.Run(sqlFile);
            }
        }
        else
        {
            Console.WriteLine($"Migrations directory not found: {migrationsPath}");
        }
    }

    private async Task RunFile(string filePath)
    {
        Console.WriteLine($"Running migration: {filePath}");
        await _migrationRunner.Run(filePath);
    }
    
    private void RunNoOptions()
    {
        Console.WriteLine("Please specify a migration file with --file or use --all to run all migrations");
    }

}