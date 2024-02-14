global using Celeste64;

namespace Kevin;

public sealed class KevinPlugin : GameMod {
    public override void OnPreMapLoaded(World world, Map map) =>
        AddActorFactory("Kevin", new((map, entity) => new Kevin()) { IsSolidGeometry = true });
}

