using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;

public class DeveloperCommandsWrap: ApplicationCommandsModule {
    [SlashCommandGroup("developers", "List of developer commands stored here.")]
    public class InnerDeveloperCommands: ApplicationCommandsModule {

    }
}