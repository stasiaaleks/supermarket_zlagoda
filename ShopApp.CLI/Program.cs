﻿using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.CLI.Migrations;
using ShopApp.Data;

namespace ShopApp.CLI;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        services.AddDataServices();
        services.AddScoped<ICmdCreator, MigrateCmdCreator>();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<CommandsRegistry>();
        services.AddSingleton<MigrationRunner>();
        
        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<CommandsRegistry>();
        var rootCommand = new RootCommand("ShopApp DB migration CLI tool");
            
        registry.RegisterAllCommands(rootCommand);
        
        return await rootCommand.InvokeAsync(args);
    }
}