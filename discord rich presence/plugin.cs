using DiscordRPC;

namespace Discord;

public sealed class DiscordPlugin : Celeste64.Mod.GameMod {

    static DiscordRpcClient client;
    static Timestamps now = Timestamps.Now;
    static string level = "Level Select";

    public override void OnModLoaded() {
        client = new("1204546983629561886", -1);
        client.Initialize();
        SetPresence();
    }

    public override void Update(float deltaTime) => client.Invoke();

    [On.Celeste64.LevelInfo.Enter]
    static void OnSelect(On.Celeste64.LevelInfo.orig_Enter orig, Celeste64.LevelInfo info, Celeste64.ScreenWipe wipe, float hold) {
        orig(info, wipe, hold);
        level = info.Name;
        SetPresence();
    }

    static void SetPresence() => client.SetPresence(new() {
        State = "Playing in " + level,
        Assets = new Assets() {
            LargeImageKey = "mountain",
        },
        Timestamps = now
    });

    // [On.Celeste64.Overworld.ctor]
    // static void OnEnter(On.Celeste64.Overworld.orig_ctor orig, Celeste64.Overworld overworld, bool use_last_selected) {
    //     orig(overworld, use_last_selected);
    //     level = "Level Select";
    //     SetPresence();
    // }

    public override void OnModUnloaded() => client.Dispose();
}

