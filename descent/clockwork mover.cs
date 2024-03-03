namespace Descent;

sealed class ClockworkMover(float max_speed, float acceleration, float rest, bool cycle, List<Vector3> targets) : Solid {

    Routine routine = new();
    int i = 1;
    bool flipped = false;

    public override void Added() {
        targets.Insert(0, Position);
        routine.Run(Events());
    }

    public override void Update() {
        base.Update();
        routine.Update();
    }

    IEnumerator<Co> Events() {
        while (true) {
            var target = targets[i];
            var normal = (target - Position).Normalized();
            while (Position != target && Vector3.Dot((target - Position).Normalized(), normal) >= 0) {
                Velocity = Utils.Approach(Velocity, max_speed * normal, acceleration * Time.Delta);
                yield return Co.SingleFrame;
            }
            Velocity = Vector3.Zero;
            MoveTo(targets[i]);
            if (flipped) {
                i--;
                if (i == 0) flipped = false;
            } else i++;
            if (i == targets.Count) {
                if (cycle) i = 0;
                else {
                    flipped = true;
                    i -= 2;
                }
            }
            TShake = .15f;
            yield return rest;
        }
    }
}
