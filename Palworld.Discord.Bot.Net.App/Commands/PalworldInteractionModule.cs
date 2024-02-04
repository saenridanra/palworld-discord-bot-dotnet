using System.Diagnostics;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Options;
using Palworld.Discord.Bot.Net.Configuration;
using Palworld.Discord.Bot.Net.Rcon;

namespace Palworld.Discord.Bot.Net.Commands;

public sealed class PalworldInteractionModule(
    IOptions<RconOptions> rconOptions,
    IOptions<BotOptions> botOptions)
    : InteractionModuleBase<SocketInteractionContext>
{
    private readonly RconClientInternal _client = new(rconOptions);

    [SlashCommand("palworld_ban", "Bans a player using the steam ID.", false, RunMode.Async)]
    public async Task BanAsync(
        string steamId)
    {
        if (!await this.AuthorizeAsync())
        {
            return;
        }

        await this.DeferAsync(true);

        _ = await this._client.SendCommandAsync($"BanPlayer {steamId}");

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = "Kicked the player!";
                _.Flags = MessageFlags.Ephemeral;
            });
    }

    [SlashCommand("palworld_broadcast", "Broadcasts a message.", false, RunMode.Async)]
    public async Task BroadcastAsync(
        string message)
    {
        if (!await this.AuthorizeAsync())
        {
            return;
        }

        await this.DeferAsync(true);

        _ = await this._client.SendCommandAsync($"Broadcast {message}");

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = "Broadcasted the message!";
                _.Flags = MessageFlags.Ephemeral;
            });
    }

    [SlashCommand("palworld_kick", "Kicks a player using the steam ID.", false, RunMode.Async)]
    public async Task KickAsync(
        string steamId)
    {
        if (!await this.AuthorizeAsync())
        {
            return;
        }

        await this.DeferAsync(true);

        _ = await this._client.SendCommandAsync($"KickPlayer {steamId}");

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = "Kicked the player!";
                _.Flags = MessageFlags.Ephemeral;
            });
    }

    [SlashCommand("palworld_save", "Saves the current server world state.", false, RunMode.Async)]
    public async Task SaveAsync()
    {
        if (!await this.AuthorizeAsync())
        {
            return;
        }

        await this.DeferAsync(true);

        var response = await this._client.SendCommandAsync("Save");

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = "Saved the current world state!";
                _.Flags = MessageFlags.Ephemeral;
            });
    }

    [SlashCommand("palworld_online", "Shows the players currently on the server", false, RunMode.Async)]
    public async Task ShowPlayersAsync()
    {
        await this.DeferAsync();

        var response = await this._client.SendCommandAsync("ShowPlayers");

        string formattedResponse = null;

        // The first line contains "name,playeruid,steamid"
        // The other lines contain the actual player data
        var players = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var playerCount = players.Length - 1;

        if (playerCount == 0)
        {
            formattedResponse = "There are no players online!";
        }
        else
        {
            var playerList = string.Join('\n', players, 1, playerCount);
            formattedResponse = $"There are {playerCount} players online:\n{playerList}";
        }

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = formattedResponse;
            });
    }

    [SlashCommand("palworld_info", "Shows the server info", false, RunMode.Async)]
    public async Task ShowServerInfoAsync()
    {
        await this.DeferAsync();

        var response = await this._client.SendCommandAsync("Info");

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = response;
            });
    }

    [SlashCommand("palworld_shutdown", "Shuts down the palworld server.", false, RunMode.Async)]
    public async Task ShutdownAsync(
        int seconds = 10,
        string reason = "Shutting down the server")
    {
        if (!await this.AuthorizeAsync())
        {
            return;
        }

        await this.DeferAsync(true);

        var response = await this._client.SendCommandAsync($"Shutdown {seconds} {reason}");

        await this.ModifyOriginalResponseAsync(
            _ =>
            {
                _.Content = "Shutting down the server!";
                _.Flags = MessageFlags.Ephemeral;
            });
    }

    private async Task<bool> AuthorizeAsync()
    {
        if (!botOptions.Value.Admins.Contains(this.Context.User.Id))
        {
            await this.RespondAsync("You are not authorized to use this command!", ephemeral: true);
            return false;
        }

        return true;
    }
}