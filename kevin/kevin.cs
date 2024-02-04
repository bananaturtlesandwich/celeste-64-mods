using Foster.Framework;
using System.Collections.Generic;
using CoEnumerator = System.Collections.Generic.IEnumerator<Celeste64.Co>;
using Vec3 = System.Numerics.Vector3;

namespace Kevin;

sealed class Kevin : Solid, IDashTrigger
{

    public bool BouncesPlayer => true;

    const float acceleration = 400;
    const float max = 600;
    const float retract = 50;

    Vec3 start;
    Vec3 target;
    bool bonked = false;

    readonly Routine routine = new();
    Sound sfxMove = null;
    Sound sfxRetract = null;

    public override void Added()
    {
        base.Added();
        start = Position;
        routine.Run(Sequence());
        sfxMove = World.Add(new Sound(this, Sfx.sfx_zipmover_loop));
        sfxRetract = World.Add(new Sound(this, Sfx.sfx_zipmover_retract_loop));
    }

    public override void Update()
    {
        base.Update();
        routine.Update();
    }

    public void HandleDash(Vec3 velocity)
    {
        World.SolidRayCast(WorldBounds.Center, (World.Get<Player>().Position - WorldBounds.Center).Normalized(), 100, out var inner, false);
        World.SolidRayCast(inner.Point, inner.Normal, 1000, out var hit);
        if (hit.Intersections == 0)
        {
            Audio.Play(Sfx.sfx_zipmover_stop, Position);
            TShake = .15f;
            UpdateOffScreen = true;
            return;
        }
        target = hit.Point - (inner.Point - WorldBounds.Center);
        bonked = true;
    }

    private CoEnumerator Sequence()
    {
        while (true)
        {
            while (!bonked) yield return Co.SingleFrame;

            bonked = false;

            Audio.Play(Sfx.sfx_zipmover_start, Position);
            TShake = .15f;
            UpdateOffScreen = true;
            yield return .15f;

            // move to target
            sfxMove?.Resume();
            var normal = (target - Position).Normalized();
            while (Position != target && Vec3.Dot((target - Position).Normalized(), normal) >= 0)
            {
                if (bonked)
                {
                    start = Position;
                    break;
                }
                Velocity = Utils.Approach(Velocity, max * normal, acceleration * Time.Delta);
                yield return Co.SingleFrame;
            }
            sfxMove?.Stop();
            Velocity = Vec3.Zero;
            MoveTo(target);

            Audio.Play(Sfx.sfx_zipmover_stop, Position);
            TShake = .2f;
            yield return .8f;

            // move back to start
            Audio.Play(Sfx.sfx_zipmover_retract_start, Position);
            sfxRetract?.Resume();
            normal = (start - Position).Normalized();
            while (Vec3.Dot((start - Position).Normalized(), normal) >= 0)
            {
                if (bonked)
                {
                    start = Position;
                    break;
                }
                Velocity = normal * retract;
                yield return Co.SingleFrame;
            }

            sfxRetract?.Stop();
            Velocity = Vec3.Zero;
            MoveTo(start);

            //reactivate
            Audio.Play(Sfx.sfx_zipmover_retract_stop, Position);
            TShake = .1f;
            UpdateOffScreen = false;
            yield return .5f;
        }
    }

    public void CollectSprites(List<Sprite> populate)
    {
        foreach (var vert in WorldVertices) populate.Add(Sprite.CreateBillboard(World, vert, "circle", 2, Color.Red));

        foreach (var face in WorldFaces)
        {
            if (face.Indices.Count <= 0) continue;

            var center = Vec3.Zero;
            foreach (var ind in face.Indices) center += WorldVertices[ind];
            center /= face.Indices.Count;

            for (int i = 0; i < 5; i++)
                populate.Add(Sprite.CreateBillboard(World, center + face.Plane.Normal * i * 1.5f, "circle", 1, Color.Green));
        }
    }

}
