namespace Sidespring;

sealed class Sidespring : Attacher, IHaveModels, IPickup {
    public SkinnedModel Model;

    public float PickupRadius => 16;

    const float speed = 160f;
    const float lift = 40f;
    float cooldown = 0;
    Vector3 dir;

    public Sidespring() {
        Model = new SkinnedModel(Assets.Models["spring_board"]) { Transform = Matrix4x4.CreateRotationY(-Calc.HalfPI) * Matrix4x4.CreateScale(8.0f) };
        Model.SetLooping("Spring", false);
        Model.Play("Idle");

        LocalBounds = new(Position + dir * 4, 16);
    }

    // facing is initialised after construct
    public override void Added() => dir = new(facing.Y, facing.X, 0f);

    public override void Update() {
        Model.Update();

        if (cooldown > 0) {
            cooldown -= Time.Delta;
            if (cooldown <= 0) UpdateOffScreen = false;
        }
    }

    public void CollectModels(List<(Actor Actor, Model Model)> populate) {
        populate.Add((this, Model));
    }

    public void Pickup(Player player) {
        if (cooldown <= 0) {
            UpdateOffScreen = true;
            Audio.Play(Sfx.sfx_springboard, Position);
            cooldown = 1.0f;
            Model.Play("Spring", true);

            player.StateMachine.State = Player.States.Normal;
            var res = Position + 3 * dir - player.Position;
            var pos = player.Position;
            // mimicking vanilla positioning behaviour but rotated is annoying
            var dot = Vector3.Dot(res, dir);
            player.Position += Calc.Approach(0f, dot, 2f) * dir;
            // remove any dir component
            res -= dot * dir;
            if (res != Vector3.Zero) {
                var len = res.Length();
                player.Position += Calc.Approach(0f, len, 4f) * res / len;
            }

            player.Velocity = dir * speed + Vector3.UnitZ * lift;
            player.HoldJumpSpeed = lift;
            player.THoldJump = player.SpringJumpHoldTime;
            player.TCoyote = 0;
            player.AutoJump = true;
            player.DashesLocal = System.Math.Max(player.DashesLocal, 1);
            player.CancelGroundSnap();

            if (AttachedTo is FallingBlock fallingBlock)
                fallingBlock.Trigger();
        }
    }
}