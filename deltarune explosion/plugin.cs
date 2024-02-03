global using Celeste64;

namespace DeltaruneExplosion;

public sealed class DeltaruneExplosion : GameMod
{
    Player player = null;
    public override void OnActorCreated(Actor actor)
    {
        switch (actor)
        {
            case Player player:
                this.player = player;
                break;
            default: break;
        }
    }

    public override void OnActorDestroyed(Actor actor)
    {
        switch (actor)
        {
            case BreakBlock block:
                if (player.Destroying) return;
                // reflection because system isn't public >:p
                ((FMOD.Studio.System)typeof(Audio).GetField("system", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null)).getCoreSystem(out var core);
                core.createSound(@"Mods\deltarune explosion\Audio\explosion.wav", FMOD.MODE.DEFAULT, out var sound);
                core.playSound(sound, new FMOD.ChannelGroup(), false, out var channel);
                World.Add(new Explosion()
                {
                    Position = block.Position
                });
                break;
            default: break;
        }
    }
}

