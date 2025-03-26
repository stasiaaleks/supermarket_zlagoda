using System.CommandLine;

namespace ShopApp.CLI.Migrations;

public class CommandsRegistry
{
    private readonly ICmdCreator _migrateCmdCreator;

    public CommandsRegistry(ICmdCreator migrateCmdCreator)
    {
        _migrateCmdCreator = migrateCmdCreator;
    }

    public void RegisterAllCommands(RootCommand rootCommand)
    {
        rootCommand.Add(_migrateCmdCreator.Create());
    }
}