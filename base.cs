namespace spaceworld;

using BepInEx;

[BepInPlugin("spuds.base", "base", "1.0.0")]
sealed class Spaceworld : BaseUnityPlugin
{
    public void OnEnable() => On.RainWorld.OnModsInit += (orig, rainworld) =>
    {
        orig.Invoke(rainworld);
        // register hooks
    };
}