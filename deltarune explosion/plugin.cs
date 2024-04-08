global using Celeste64;

namespace DeltaruneExplosion;

public sealed class DeltaruneExplosionPlugin : Celeste64.Mod.GameMod {
    [On.Celeste64.BreakBlock.HandleDash]
    static void Play(On.Celeste64.BreakBlock.orig_HandleDash orig, BreakBlock block, System.Numerics.Vector3 vel) {
        Audio.PlaySound("explosion");
        block.World.Add(new Explosion() { Position = block.Position });
        orig(block, vel);
    }
}

