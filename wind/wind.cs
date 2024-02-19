namespace Wind;

sealed class Wind(Vec3 dir) : Actor {

    Vec3 original = Vec3.Zero;
    bool active = false;

    public override void Added() => UpdateOffScreen = true;

    public override void Update() {
        var player = World.Get<Player>();
        switch (World.OverlapsFirst<Wind>(player.Position) == this, active) {
            case (true, false):
                active = true;
                var snow = World.Get<Snow>();
                original = snow.Direction;
                snow.Direction = dir;
                break;
            case (false, true):
                active = false;
                World.Get<Snow>().Direction = original;
                break;
            default: break;
        }
        if (active) World.Get<Player>().RidingPlatformMoved(dir * Foster.Framework.Time.Delta);
    }
}
