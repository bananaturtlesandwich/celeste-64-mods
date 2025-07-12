namespace Wind;

sealed class Wind(Vec3 dir) : Actor {

    Vec3 original = Vec3.Zero;
    SoundHandle? whoosh;

    public override void Added() => UpdateOffScreen = true;

    public override void Update() {
        var player = World.Get<Player>();
        switch (World.OverlapsFirst<Wind>(player.Position) == this, whoosh.HasValue)
        {
            case (true, false):
                whoosh = Audio.PlaySound("wind", int.MaxValue);
                var snow = World.Get<Snow>();
                original = snow.Direction;
                snow.Direction = dir;
                break;
            case (false, true):
                World.Get<Snow>().Direction = original;
                if (whoosh is SoundHandle handle) {
                    handle.Stop();
                    whoosh = null;
                }
                break;
            default: break;
        }
        if (whoosh.HasValue) World.Get<Player>().RidingPlatformMoved(dir * Foster.Framework.Time.Delta);
    }
}
