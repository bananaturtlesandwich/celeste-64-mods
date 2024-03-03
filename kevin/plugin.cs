global using Celeste64;

namespace Kevin;

public sealed class KevinPlugin : Celeste64.Mod.GameMod {
    public override void OnPreMapLoaded(World world, Map map) =>
        AddActorFactory("Kevin", new((map, entity) => new Kevin()) { IsSolidGeometry = true });
}

