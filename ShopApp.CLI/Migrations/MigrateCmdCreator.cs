using System.CommandLine;

namespace ShopApp.CLI.Migrations;

public interface ICmdCreator
{
    Command Create();
}

public class MigrateCmdCreator : ICmdCreator
{
    private MigrationRunner _migrationRunner;

    public MigrateCmdCreator(MigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    public Command Create()
    {
        var migrateCommand = new Command("migrate", "Run database migrations");

        var fileOption = new Option<string>(
            name: "--file",
            description: "The path to the migration SQL file");
        fileOption.AddAlias("-f");
        migrateCommand.AddOption(fileOption);

        var allOption = new Option<bool>(
            name: "--all",
            description: "Run all migrations in the default directory");
        allOption.AddAlias("-a");
        migrateCommand.AddOption(allOption);
        
        migrateCommand.SetHandler(async (string file, bool all) =>
        {
            if (all)
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
            else if (!string.IsNullOrEmpty(file))
            {
                Console.WriteLine($"Running migration: {file}");
                await _migrationRunner.Run(file);
            }
            else
            {
                Console.WriteLine("Please specify a migration file with --file or use --all to run all migrations");
            }
        }, fileOption, allOption);
        return migrateCommand;
    }
}