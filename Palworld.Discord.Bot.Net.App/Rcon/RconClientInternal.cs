using Microsoft.Extensions.Options;
using Rcon;

namespace Palworld.Discord.Bot.Net.Rcon;

internal sealed class RconClientInternal
{
    private readonly IOptions<RconOptions> _options;
    private readonly RconClient _rconClient;

    public RconClientInternal(
        IOptions<RconOptions> options)
    {
        this._options = options;
        this._rconClient = new RconClient();
    }

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
        await this._rconClient.ConnectAsync(this._options.Value.Host, this._options.Value.Port);
        await this._rconClient.AuthenticateAsync(this._options.Value.Password);

        return this._rconClient.Authenticated;
    }
}