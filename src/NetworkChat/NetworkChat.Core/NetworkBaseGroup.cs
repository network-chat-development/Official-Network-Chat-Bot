using DisCatSharp;
using DisCatSharp.Entities;
using Newtonsoft.Json;

using NetworkChat.Core;

public interface BanEntry {
    public DiscordUser User { get; set; }
    public string Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public Dictionary<string, object> ToDictionary() {
        return new Dictionary<string, object>() {
            { "user", User.Id },
        };
    }
}

public class NetworkGroupBase {
    public string _Id { get; set; } = null!;
    public BotBase Bot { get; } = null!;
    public string Name { get; set; } = null!;
    public List<DiscordChannel> Channels { get; set; } = null!;

    public List<DiscordGuild> Guilds { get; set; } = null!;
    public Dictionary<int, string> Webhooks { get; set; } = null!;
    public DiscordUser Owner { get; set; } = null!;
    public List<string> BlacklistedWords { get; set; } = null!;

    public List<DiscordUser> Admins { get; set; } = null!;
    public List<DiscordUser> Mods { get; set; } = null!;
    public bool AllowedNSFW { get; set; } = false;
    public bool FilterEnabled { get; set; } = true;

    public int SlowMode { get; set; } = 5;
    public List<BanEntry>? bans { get; set; }
}

public class NetworkGroup {
    public readonly Broadcast broadcast;
    public readonly NetworkGroupBase data;

    public NetworkGroup(NetworkGroupBase data) {
        this.data = data;
        broadcast = data.Bot.broadcast;
    }

    public async Task SendMessageGroupAsync(DiscordMessage message) {
        foreach (var word in data.BlacklistedWords) {
            if (message.Content.Contains(word)) {
                return;
            }
        }

        var structure = new BroadCastStruct();

        await Task.Delay(1);

        //await broadcast.BroadcastAsync();
    }

    public static NetworkGroup FromDictionary(BotBase bot, Dictionary<string, object> data) {
        return FromDictionary(bot, data);
    }

    /*
     * public static Dictionary<string, object> ToDictionary() {
        return
    }
    */

    /*
     * private static Dictionary<string, object> TemplateForBan() {

    }*/

    private static string MessageCleansify(string content) {
        return content.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
    }
}
