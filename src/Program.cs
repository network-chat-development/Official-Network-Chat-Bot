using DotNetEnv;
//using NetworkChat.Core;
using Newtonsoft.Json;

Env.TraversePath().Load();

var f = new Dictionary<string, int>() {
    { "test1", 1 },
    { "test2", 2 },
}; //Prints {'test1': 1, 'test2': 2}

Console.WriteLine(JsonConvert.SerializeObject(f));

//await new BotBase(true, 2, token: Env.GetString("TOKEN")).RunBlockingAsync();