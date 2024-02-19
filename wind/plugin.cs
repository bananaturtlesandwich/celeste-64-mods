global using Celeste64;
global using Vec3 = System.Numerics.Vector3;

namespace Wind;

public sealed class WindPlugin : GameMod {
    public override void OnPreMapLoaded(World world, Map map) =>
        AddActorFactory("Wind", new((map, entity) => new Wind(entity.GetVectorProperty("direction", Vec3.Zero))) { UseSolidsAsBounds = true });
}

