using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Palworld.Discord.Bot.Net.Commands;

public sealed class CommandHandler(
    DiscordSocketClient client,
    InteractionService commands,
    IServiceProvider services)
{
    public async Task InstallCommandsAsync()
    {
        client.InteractionCreated += this.HandleInteractionAsync;
        client.Ready += this.OnReadyAsync;

        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
    }

    private async Task HandleInteractionAsync(
        SocketInteraction arg)
    {
        try
        {
            // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
            var ctx = new SocketInteractionContext(client, arg);
            await commands.ExecuteCommandAsync(ctx, services);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            // if a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if (arg.Type == InteractionType.ApplicationCommand)
            {
                await arg.GetOriginalResponseAsync().ContinueWith(async msg => await msg.Result.DeleteAsync());
            }
        }
    }

    private async Task OnReadyAsync()
    {
        await client.SetGameAsync("Palworld");
        await commands.RegisterCommandsGloballyAsync();
    }
}