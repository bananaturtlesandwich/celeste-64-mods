global using Celeste64;

namespace DeltaruneExplosion;

public sealed class DeltaruneExplosionPlugin : Celeste64.Mod.GameMod {

    Player player = null;

    public override void OnActorCreated(Actor actor) {
        if (actor is Player player) this.player = player;
    }

    public override void OnActorDestroyed(Actor actor) {
        if (actor is not BreakBlock block || player.Destroying) return;
        Audio.PlaySound("explosion");
        World.Add(new Explosion() { Position = block.Position });
    }
}

