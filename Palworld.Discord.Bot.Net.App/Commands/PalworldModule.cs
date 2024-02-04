using Discord.Commands;
using Microsoft.Extensions.Options;
using Palworld.Discord.Bot.Net.Rcon;

namespace Palworld.Discord.Bot.Net.Commands;

public sealed class PalworldModule : ModuleBase<SocketCommandContext>
{
    private readonly RconClientInternal _client;

    public PalworldModule(IOptions<RconOptions> rconOptions)
    {
        this._client = new RconClientInternal(rconOptions);
    }

    [Command("Palworld.ShowPlayers", RunMode = RunMode.Async)]
    [Summary("Shows the players currently on the server")]
    public async Task ShowPlayersAsync()
    {
        var response = await this._client.SendCommandAsync("ShowPlayers");

        await this.ReplyAsync(response);
    }

    [Command("Palworld.ShowServerInfo", RunMode = RunMode.Async)]
    [Summary("Shows the server info")]
    public async Task ShowServerInfoAsync()
    {
        var response = await this._client.SendCommandAsync("Info");

        await this.ReplyAsync(response);
    }
}