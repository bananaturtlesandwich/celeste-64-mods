global using Celeste64;

namespace DeltaruneExplosion;

public sealed class DeltaruneExplosionPlugin : GameMod {

    Player player = null;

    public override void OnActorCreated(Actor actor) {
        if (actor is Player player) this.player = player;
    }

    public override void OnActorDestroyed(Actor actor) {
        if (actor is not BreakBlock block || player.Destroying) return;
        // reflection because system isn't public >:p
        ((FMOD.Studio.System)typeof(Audio).GetField("system", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null)).getCoreSystem(out var core);
        core.createSound(@"Mods\deltarune explosion\Audio\explosion.wav", FMOD.MODE.DEFAULT, out var sound);
        core.playSound(sound, new FMOD.ChannelGroup(), false, out var channel);
        World.Add(new Explosion() { Position = block.Position });
    }
}

