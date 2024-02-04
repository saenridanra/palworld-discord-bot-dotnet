using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Palworld.Discord.Bot.Net;
using Palworld.Discord.Bot.Net.Commands;
using Palworld.Discord.Bot.Net.Configuration;
using Palworld.Discord.Bot.Net.Rcon;

var builder = Host.CreateApplicationBuilder(args);

IHostEnvironment env = builder.Environment;

builder.Configuration.Sources.Clear();

builder.Configuration.AddEnvironmentVariables();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

BotOptions options = new();
builder.Configuration.GetSection("Bot").Bind(options);
builder.Services.Configure<BotOptions>(
    builder.Configuration.GetSection(
        key: "Bot"));

RconOptions rconOtions = new();
builder.Configuration.GetSection("Rcon").Bind(rconOtions);
builder.Services.Configure<RconOptions>(
    builder.Configuration.GetSection(
        key: "Rcon"));

builder.Services.AddSingleton(
    new DiscordSocketClient(
        new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
            UseInteractionSnowflakeDate = false
        }));

builder.Services.AddSingleton<RconClientInternal>();

builder.Services.AddSingleton<InteractionService>();
builder.Services.AddSingleton<CommandHandler>();
builder.Services.AddHostedService<DiscordStartupService>();

var host = builder.Build();
await host.RunAsync();
