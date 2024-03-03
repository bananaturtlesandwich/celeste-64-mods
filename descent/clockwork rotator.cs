using static System.MathF;

namespace Descent;

sealed class ClockworkRotator(float speed, float rest, Vector3 axis, List<float> angles) : Solid {

    Routine routine = new();
    Quaternion last = Quaternion.Identity;
    int i = 0;
    float rotated = 0f;
    Quaternion Rotation => Quaternion.CreateFromAxisAngle(Vector3.UnitY, tilt.Y) *
                Quaternion.CreateFromAxisAngle(Vector3.UnitX, tilt.X) *
                Quaternion.CreateFromAxisAngle(Vector3.UnitZ, facing.Angle() + Calc.HalfPI);

    public override void Added() {
        axis = (Position - axis).Normalized();
        last = Rotation;
        routine.Run(Events());
    }

    public override void Update() {
        base.Update();
        routine.Update();
    }

    void UpdateRotation(Quaternion r) {
        Tilt = new(
            Asin(2.0f * (r.X * r.W - r.Y * r.Z)),
            Atan2(2.0f * (r.Y * r.W + r.X * r.Z), 1.0f - 2.0f * (r.X * r.X + r.Y * r.Y))
        );
        Facing = Calc.AngleToVector(Atan2(2.0f * (r.X * r.Y + r.Z * r.W), 1.0f - 2.0f * (r.X * r.X + r.Z * r.Z)) - Calc.HalfPI);
        // not sure why this is happening tbh but this fixes it
        // if (float.IsNaN(Tilt.Y)) Tilt = new(Tilt.X, -Calc.HalfPI);
    }

    void UpdatePlayer(Quaternion r) {
        if (!HasPlayerRider()) return;
        var player = World.Get<Player>();
        player.Position = (Matrix4x4.CreateTranslation(Position) * Matrix4x4.CreateFromQuaternion(-r) * Matrix4x4.CreateTranslation(player.Position - Position)).Translation;
    }

    IEnumerator<Co> Events() {
        while (true) {
            var offset = angles[i] * speed * Time.Delta;
            rotated += float.Abs(offset);
            Quaternion rot;
            if (rotated > float.Abs(angles[i])) {
                rotated = 0f;
                rot = Quaternion.CreateFromAxisAngle(axis, angles[i]);
                // UpdatePlayer(rot);
                UpdateRotation(rot * last);
                last = Rotation;
                i++;
                if (i == angles.Count) i = 0;
                TShake = .15f;
                yield return rest;
            } else {
                rot = Quaternion.CreateFromAxisAngle(axis, offset);
                // UpdatePlayer(rot);
                UpdateRotation(rot * Rotation);
                yield return Co.SingleFrame;
            }
        }
    }

    // to enable debug ui uncomment and add IHaveUI interface
    /*
    public void RenderUI(Batcher batch, Rect rect) {
        if (World.Paused) return;
        Menu menu = new();
        menu.Add(new Menu.Option($"{Tilt}, {Facing}"));
        menu.Render(batch, rect.BottomCenter + new System.Numerics.Vector2(0, -50));
    }
    */
}
