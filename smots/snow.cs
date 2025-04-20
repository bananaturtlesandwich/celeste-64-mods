namespace Smots;

public class ColouredSnow() : Snow(0.3f, -Vec3.UnitZ), IHaveSprites {
    public override void CollectSprites(List<Sprite> populate) {
        var cameraPosition = World.Camera.Position;
        var cameraFrustum = World.Camera.Frustum;
        var cammeraNormal = World.Camera.Normal;
        var box = cameraFrustum.GetBoundingBox();

        var size = new Vec3(box.Max.X - box.Min.X, box.Max.Y - box.Min.Y, box.Max.Z - box.Min.Z);
        if (size.X <= 0 || size.Y <= 0 || size.Z <= 0)
            return;

        var area = 100;
        var subtex = Assets.Subtextures["circle"];
        var time = World.GeneralTimer;

        // welcome to the land of unoptimized snow :)

        float ax = System.MathF.Floor(box.Min.X / area) * area;
        float ay = System.MathF.Floor(box.Min.Y / area) * area;
        float az = System.MathF.Floor(box.Min.Z / area) * area;
        float bx = System.MathF.Ceiling(box.Max.X / area) * area;
        float by = System.MathF.Ceiling(box.Max.Y / area) * area;
        float bz = System.MathF.Ceiling(box.Max.Z / area) * area;

        for (float x = ax; x <= bx; x += area)
            for (float y = ay; y <= by; y += area)
                for (float z = az; z <= bz; z += area) {
                    var center = new Vec3(x, y, z) + Vec3.One * area * 0.50f;
                    if (Vec3.Dot(center - cameraPosition, cammeraNormal) <= 0)
                        continue;

                    var distZ = Calc.ClampedMap(cameraPosition.Z - center.Z, 0, 200, 1, 0);
                    if (distZ <= 0)
                        continue;

                    var inputDist2dSqrd = (cameraPosition.XY() - center.XY()).LengthSquared();
                    if (inputDist2dSqrd > 300 * 300)
                        continue;

                    var dist2d = Calc.ClampedMap(System.MathF.Sqrt(inputDist2dSqrd), 100, 300, 1, 0);
                    if (dist2d <= 0)
                        continue;

                    var alpha = dist2d * distZ;
                    if (alpha < 0.1f)
                        continue;

                    if (!cameraFrustum.Contains(new BoundingBox(center, area)))
                        continue;

                    var rng = new Rng(0);
                    var count = Calc.Lerp(0, 50, dist2d) * Amount;

                    for (int i = 0; i < count; i++) {
                        Vec3 point = new() {
                            X = x + Mod(rng.Float(area) + rng.Float(5, 25) * time * Direction.X, area),
                            Y = y + Mod(rng.Float(area) + rng.Float(5, 25) * time * Direction.Y, area),
                            Z = z + Mod(rng.Float(area) + rng.Float(5, 25) * time * Direction.Z, area)
                        };

                        populate.Add(Sprite.CreateBillboard(World, point, subtex, 0.50f, new[] { Color.Blue, Color.Green, Color.Red }[rng.Int(3)] * alpha));
                    }
                }
    }
}
