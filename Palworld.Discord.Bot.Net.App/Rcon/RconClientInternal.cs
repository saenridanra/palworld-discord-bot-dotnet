using Microsoft.Extensions.Options;
using Rcon;

namespace Palworld.Discord.Bot.Net.Rcon;

internal sealed class RconClientInternal(IOptions<RconOptions> options)
{
    private readonly RconClient _rconClient = new();

    public async Task<string> SendCommandAsync(
        string command)
    {
        if (!this._rconClient.Authenticated)
        {
            await this.ConnectAsync();
        }

        return await this._rconClient.SendCommandAsync(command);
    }

    private async Task<bool> ConnectAsync()
    {
        await this._rconClient.ConnectAsync(options.Value.Host, options.Value.Port);
        await this._rconClient.AuthenticateAsync(options.Value.Password);

        return this._rconClient.Authenticated;
    }
}