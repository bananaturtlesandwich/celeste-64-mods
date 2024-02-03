using Foster.Framework;

namespace DeltaruneExplosion;

sealed class Explosion : Actor, IHaveSprites
{
    float timer = 0;
    int i = 0;
    Subtexture frame = Assets.Subtextures[sprites[0]];

    public override void Update()
    {
        timer += Time.Delta;
        if (timer > 0.05)
        {
            timer = 0;
            i++;
            if (i == 16) World.Destroy(this);
            frame = Assets.Subtextures[sprites[i]];
        }
    }

    public void CollectSprites(System.Collections.Generic.List<Sprite> populate)
    {
        populate.Add(Sprite.CreateBillboard(World, Position, Assets.Subtextures[sprites[i]], 15, Color.White));
    }

    private static readonly string[] sprites = [
        "explosion_0",
        "explosion_1",
        "explosion_2",
        "explosion_3",
        "explosion_4",
        "explosion_5",
        "explosion_6",
        "explosion_7",
        "explosion_8",
        "explosion_9",
        "explosion_10",
        "explosion_11",
        "explosion_12",
        "explosion_13",
        "explosion_14",
        "explosion_15",
        "explosion_16",
    ];
}
