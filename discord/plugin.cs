using DiscordRPC;

namespace Discord;

public sealed class DiscordPlugin : Celeste64.Mod.GameMod {

    DiscordRpcClient client = new("1204546983629561887", pipe: -1);

    public DiscordPlugin() {
        client.Initialize();
        client.SetPresence(new() {
            State = "idle",
            Assets = new Assets() {
                LargeImageKey = "mountain",
            }
        });
    }

    ~DiscordPlugin() => client.Dispose();

    public override void OnMapLoaded(Celeste64.Map map) => client.SetPresence(new() {
        State = map.Filename,
        Assets = new Assets() {
            LargeImageKey = "mountain",
        }
    });

    public override void Update(float deltaTime) => client.Invoke();

}

