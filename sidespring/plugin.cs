global using Celeste64;
global using Foster.Framework;
global using System.Numerics;
global using System.Collections.Generic;

namespace Sidespring;

public sealed class SidespringPlugin : Celeste64.Mod.GameMod {
    public override void OnPreMapLoaded(World world, Map map) =>
        AddActorFactory("Sidespring", new((map, entity) => new Sidespring()));
}

