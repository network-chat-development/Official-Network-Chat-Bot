using DisCatSharp;
using DotNetEnv;

namespace NetworkChat.Core;

public sealed class BotBase {
    public DiscordClient Client { get; }
    public int ShardId { get; set; }
    public bool IsReady { get; private set; }

    public Broadcast broadcast { get; }

    //private readonly string _db;

    public BotBase(bool reconnect, int shardCount, string token) {
        if (shardCount < 0) throw new ArgumentOutOfRangeException(nameof(shardCount));

        Client = new(new()
        {
            ShardCount = shardCount,
            MessageCacheSize = 2048,
            Intents = DiscordIntents.MessageContent | DiscordIntents.AllUnprivileged,
            TokenType = TokenType.Bot,
            Token = token,
        });

        broadcast = new (new HttpClient(), Env.GetString("API_KEY"), this);
    }

    private async Task BaseRunAsync() {
        var ClientReady = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        try {
            Client.Ready += Client_Ready;
            await Client.ConnectAsync();
        }
        catch (Exception err) {
            Console.WriteLine($"Error: {err.ToString()}");
        }
    }

    private Task Client_Ready(DiscordClient sender, DisCatSharp.EventArgs.ReadyEventArgs e) {
        Console.WriteLine($"Logging in as {sender.CurrentUser.UsernameWithDiscriminator}");

        return Task.CompletedTask;
    }

    //public async Task SaveDatabaseAsync() { }

    public async Task RunBlockingAsync() {
        await BaseRunAsync();
        await Task.Delay(-1); //Blocking everything that comes after this awaitable function.
    }
}