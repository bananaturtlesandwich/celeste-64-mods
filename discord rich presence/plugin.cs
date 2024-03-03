using DiscordRPC;

namespace Discord;

public sealed class DiscordPlugin : Celeste64.Mod.GameMod {

    DiscordRpcClient client = new("1204546983629561886", pipe: -1);

    public override void OnModLoaded() {
        client.Initialize();
        client.SetPresence(new() {
            State = "idle",
            Assets = new Assets() {
                LargeImageKey = "mountain",
            }
        });
    }

    public override void OnModUnloaded() => client.Dispose();

    public override void OnMapLoaded(Celeste64.Map map) => client.SetPresence(new() {
        State = "playing in " + map.Filename.Replace("Maps/", "").Replace(".map", ""),
        Assets = new Assets() {
            LargeImageKey = "mountain",
        }
    });

    public override void Update(float deltaTime) => client.Invoke();

}

