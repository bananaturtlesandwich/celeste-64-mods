namespace Smots;

sealed class NeedleBlock : Actor, IHaveModels {

    SimpleModel model;

    public override void Added() {
        var rng = new Rng(0);
        var area = LocalBounds.Size.X * LocalBounds.Size.Y * LocalBounds.Size.Z / 1000;
        var models = new List<SkinnedModel>((int)area);
        for (int i = 0; i < area; i++) {
            models.Add(new SkinnedModel(Assets.Models["needleball"]) {
                Flags = ModelFlags.Terrain,
                Transform = Matrix4x4.CreateScale(new Vector3(2f)) * Matrix4x4.CreateTranslation(new System.Numerics.Vector3(
                    rng.Float(LocalBounds.Min.X, LocalBounds.Max.X),
                    rng.Float(LocalBounds.Min.Y, LocalBounds.Max.Y),
                    rng.Float(LocalBounds.Min.Z, LocalBounds.Max.Z)
                ))
            });
        }
        model = new(models);
    }

    public override void Update() {
        var player = World.Get<Player>();
        if (World.OverlapsFirst<NeedleBlock>(player.Position) == this) {
            player.Kill();
        }
    }

    public void CollectModels(List<(Actor Actor, Model Model)> populate) {
        if (model != null) populate.Add((this, model));
    }
}
