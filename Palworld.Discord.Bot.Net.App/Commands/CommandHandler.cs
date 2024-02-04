using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

namespace Palworld.Discord.Bot.Net.Commands;

public sealed class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    public CommandHandler(
        DiscordSocketClient client,
        CommandService commands,
        IServiceProvider services)
    {
        this._client = client;
        this._commands = commands;
        this._services = services;
    }

    public async Task InstallCommandsAsync()
    {
        this._client.MessageReceived += this.HandleCommandAsync;
        this._client.Ready += this.OnReadyAsync;

        await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), this._services);
    }

    private async Task OnReadyAsync()
    {
        await this._client.SetGameAsync("Palworld");
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage userMessage)
        {
            return;
        }

        int argPos = 0;
        if (!userMessage.HasStringPrefix("!", ref argPos) ||
            userMessage.Author.IsBot)
        {
            return;
        }

        SocketCommandContext context = new(this._client, userMessage);
        await this._commands.ExecuteAsync(
            context,
            argPos,
            this._services);
    }
}