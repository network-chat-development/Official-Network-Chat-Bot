using DisCatSharp;
using DisCatSharp.Entities;
using NetworkChat.Core;

public class UtilitiesCoreBot {
    public BotBase bot { get; } = null!;

    public UtilitiesCoreBot(BotBase bot) {
        this.bot = bot;
    }

    public async Task GetUsernameAsync() {

    }

    /// <summary>
    /// Terrible solution for resolving reference from MessageReference.
    /// </summary>
    /// <param name="data"></param>
    /// <returns> Dictionary<string></object></returns>
    public Dictionary<string, object> ResolveRef(DiscordMessage message) {
        //TODO: Use a better solution.
        string resolvedMsg = "";

        if (string.IsNullOrEmpty(message.Content)) {
            resolvedMsg = message.Content;
        } else if (message.Embeds.Count == 2) { 
            resolvedMsg = message.Embeds[1].Thumbnail.Url.ToString();
        } else if (message.Embeds.Count > 0 && message.Embeds.Count < 2) {
            resolvedMsg = message.Embeds[0].Thumbnail.Url.ToString();
        }

        var obj = new Dictionary<string, object>() { };
        obj.Add("author", new Dictionary<string, string>() {
            { "username", "" }, //TODO: Resolve Username.
            { "avatar_url", message.Author.AvatarUrl }
        });

        var resolved = new Dictionary<string, object>() {};

        resolved.Add("content", resolvedMsg);
        resolved.Add("author", obj);
        resolved.Add("messageURL", message.JumpLink);

        return resolved;
    }
}