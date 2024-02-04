using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Palworld.Discord.Bot.Net.Commands;
using Palworld.Discord.Bot.Net.Configuration;

namespace Palworld.Discord.Bot.Net;

public class DiscordStartupService(
    IOptions<BotOptions> options,
    DiscordSocketClient client,
    CommandHandler commandHandler,
    InteractionService interactionService)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        client.Log += this.LogAsync;
        interactionService.Log += this.LogAsync;

        await client.LoginAsync(TokenType.Bot, options.Value.Token);
        await client.StartAsync();
        await commandHandler.InstallCommandsAsync();
    }

    public async Task StopAsync(
        CancellationToken cancellationToken)
    {
        await client.StopAsync();
        await client.LogoutAsync();
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());

        return Task.CompletedTask;
    }
}