global using Celeste64;
global using Foster.Framework;
global using System.Numerics;
global using System.Collections.Generic;

namespace Descent;

public sealed class DescentPlugin : Celeste64.Mod.GameMod {
    public override void OnPreMapLoaded(World world, Map map) {
        AddActorFactory(
            "ClockworkRotator",
            new((map, entity) => {
                List<float> angles = new();
                for (int i = 1; true; i++) {
                    if (!entity.Properties.ContainsKey($"angle_{i}")) break;
                    angles.Add(entity.GetFloatProperty($"angle_{i}", 0f) * Calc.DegToRad);
                }
                if (angles.Count == 0) angles.Add(Calc.HalfPI);
                return new ClockworkRotator(
                    entity.GetFloatProperty("speed", 1f),
                    entity.GetFloatProperty("rest", 1f),
                    map.FindTargetNodeFromParam(entity, "target"),
                    angles
                );
            }) { IsSolidGeometry = true }
        );
        AddActorFactory(
            "ClockworkMover",
            new((map, entity) => {
                List<Vector3> targets = new();
                for (int i = 1; true; i++) {
                    if (
                        !entity.Properties.ContainsKey($"target_{i}") ||
                        !map.FindTargetNode(entity.GetStringProperty($"target_{i}", string.Empty), out var pos)
                    ) break;
                    targets.Add(pos);
                }
                if (targets.Count == 0) return null;
                return new ClockworkMover(
                    entity.GetFloatProperty("max_speed", 600f),
                    entity.GetFloatProperty("acceleration", 400f),
                    entity.GetFloatProperty("rest", 1f),
                    entity.GetIntProperty("cycle", 1) > 0,
                    targets
                );
            }) { IsSolidGeometry = true }
        );
    }
}

