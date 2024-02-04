global using Celeste64;

namespace Kevin;

public sealed class KevinPlugin : GameMod
{
    public override void OnModLoaded()
    {
        AddActorFactory("Kevin", new Map.ActorFactory((map, entity) => new Kevin()) { IsSolidGeometry = true });
    }
}

