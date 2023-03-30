using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

using DisCatSharp.Entities;
using Newtonsoft.Json;

using NetworkChat.Core;

public class BroadCastStruct {
    public string GroupId { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DiscordUser User { get; set; } = null!;
    public DiscordChannel Channel { get; set; } = null!;

    public DiscordGuild Guild { get; set; } = null!;
    public DiscordMessageReference? Refer { get; set; }
    public DiscordEmbed? Embed { get; set; }
}

public sealed class Broadcast {
    private string BaseHttp = "https://api.network-chat.xyz";
    private string token = null!;
    
    public BotBase bot { get; }
    public HttpClient handler { get; set; }

    private int CheckCounter = 0;
    private int RetryCounter = 0;
    private int RetryTime = 5;
    private int HeartbeatCount = 0;

    public Broadcast(HttpClient handler, string token, BotBase bot) {
        this.handler = handler;
        this.token = token;
        this.bot = bot;
    }

    public async Task BroadcastAsync(BroadCastStruct broadData) {
        var url = $"{BaseHttp}/{broadData.GroupId}";

        var contentData = new Dictionary<string, object>() {
            { "content", broadData.Content },
            { "author", new Dictionary<string, object>() },
            { "guild", new Dictionary<string, string>() { { "id", broadData.Guild.Id.ToString() } } },

        };

        if (broadData.Refer != null) {
            var msg = await broadData.Guild.GetChannel(broadData.Refer.Channel.Id).GetMessageAsync(broadData.Refer.Message.Id);

            if (msg != null) {

            }

            var refStruct = new Dictionary<string, object>() {
                { "content", compare},

            };

            Console.WriteLine(refStruct);
            contentData.Add("reference", refStruct);
        }

        var result = await handler.PostAsync(url, new StringContent(JsonConvert.SerializeObject(contentData)));
    }

    public async Task SendHeartbeatAsync() {
        var url = $"{BaseHttp}/heartbeat";
        HeartbeatCount += 1;

        bot.Client.Logger.LogInformation($"Heartbeat #{HeartbeatCount} sent.");

        handler.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await handler.GetAsync(url);

        if (res.IsSuccessStatusCode) {
            CheckCounter += 1;

            if (CheckCounter % 5 == 0) {
                await SendCheckAsync();
            }

        } else {
            await SendCheckAsync();
        }
    }

    public async Task SendCheckAsync() {
        var url = $"{BaseHttp}/check";

        if (RetryCounter == 5) {
            bot.Client.Logger.LogError("API is unable to reach. Aborting request...");

            var channel = await bot.Client.GetChannelAsync(1075403869653311628);
            var embed = new DiscordEmbedBuilder();

            embed.WithTitle("API Unavaliable");

            await channel.SendMessageAsync(embed.Build());
        }

        handler.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await handler.GetAsync(url);

        if ( res.IsSuccessStatusCode ) {
            RetryCounter = 0;
        } else {
            bot.Client.Logger.LogError($"API is unable to reach... Retrying in {RetryTime} seconds.");
            await Task.Delay(RetryTime * 1000);

            RetryCounter++;
            RetryTime += 5;

            await SendCheckAsync();
        }
    }
}