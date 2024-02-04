namespace Palworld.Discord.Bot.Net.Configuration;

public sealed class BotOptions
{
    public string Token { get; set; }

    public IReadOnlyList<ulong> Admins { get; set; }
}