using Foster.Framework;

namespace DeltaruneExplosion;

sealed class Explosion : Actor, IHaveSprites {
    float timer = 0;
    int i = 0;

    public override void Update() {
        timer += Time.Delta;
        if (timer > 0.05) {
            timer = 0;
            i++;
            if (i == 16)
                World.Destroy(this);
        }
    }

    public void CollectSprites(System.Collections.Generic.List<Sprite> populate) =>
        populate.Add(Sprite.CreateBillboard(World, Position, Assets.Subtextures[$"explosion_{i}"], 15, Color.White));
}
