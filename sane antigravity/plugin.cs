using BepInEx;

[BepInPlugin("spuds.sane-antigrav", "sane antigravity", "1.0.0")]
sealed class SaneAntigrav : BaseUnityPlugin {
    public void OnEnable() => On.RainWorld.OnModsInit += (orig, rainworld) => {
        orig.Invoke(rainworld);
        On.Player.Jump += (orig, self) => {
            RedirectAnim(self);
            orig(self);
        };
        On.Player.MovementUpdate += (orig, self, eu) => {
            RedirectAnim(self);
            orig(self, eu);
        };
        On.Player.SlugSlamConditions += (orig, self, other) => {
            RedirectAnim(self);
            return orig(self, other);
        };
        On.Player.UpdateAnimation += (orig, self) => {
            RedirectAnim(self);
            orig(self);
        };
        On.PlayerGraphics.Update += (orig, self) => {
            RedirectAnim(self.player);
            orig(self);
        };
        On.SlugcatHand.EngageInMovement += (orig, self) => {
            RedirectAnim(self.owner.owner as Player);
            return orig(self);
        };
    };

    void RedirectAnim(Player player) {
        if (player.animation == Player.AnimationIndex.ZeroGSwim) player.animation = Player.AnimationIndex.DeepSwim;
    }
}

