using DotNetEnv;
using NetworkChat.Core;

Env.TraversePath().Load();

await new BotBase(true, 2, token: Env.GetString("TOKEN")).RunBlockingAsync();