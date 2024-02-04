using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Palworld.Discord.Bot.Net.Commands;
using Palworld.Discord.Bot.Net.Configuration;

namespace Palworld.Discord.Bot.Net;

public class DiscordStartupService : IHostedService
{
    private readonly CommandHandler _commandHandler;

    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordStartupService> _logger;
    private readonly IOptions<BotOptions> _options;

    public DiscordStartupService(
        ILogger<DiscordStartupService> logger,
        IOptions<BotOptions> options,
        DiscordSocketClient client,
        CommandHandler commandHandler)
    {
        this._logger = logger;
        this._options = options;
        this._client = client;
        this._commandHandler = commandHandler;
    }

    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        this._client.Log += this.LogAsync;
        await this._client.LoginAsync(TokenType.Bot, this._options.Value.Token);
        await this._client.StartAsync();
        await this._commandHandler.InstallCommandsAsync();
    }

    public async Task StopAsync(
        CancellationToken cancellationToken)
    {
        await this._client.StopAsync();
        await this._client.LogoutAsync();
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());

        return Task.CompletedTask;
    }
}