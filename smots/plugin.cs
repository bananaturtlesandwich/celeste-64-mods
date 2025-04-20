global using Celeste64;
global using Foster.Framework;
global using Vec3 = System.Numerics.Vector3;
global using Vec2 = System.Numerics.Vector2;
global using System.Collections.Generic;
global using System.Numerics;
using MonoMod.RuntimeDetour;

namespace Smots;

public sealed class SmotsPlugin : Celeste64.Mod.GameMod {
    bool added = false;
    bool active = false;
    static bool updashing = false;
    public override void OnPreMapLoaded(World world, Map map) {
        AddActorFactory("NeedleBlock", new((map, entity) => new NeedleBlock()) { UseSolidsAsBounds = true });
        active = map.Filename == "Maps/smots.map";
        if (active) {
            world.Add(new ColouredSnow());
            if (!added) Assets.Models.Add("spike", Assets.Models["needle"], this);
            added = true;
        }
    }
    public override void Update(float deltaTime) {
        if (!active) return;
        var player = World.Get<Player>();
        updashing = player.DashesLocal > 0 && player.TDashCooldown <= 0 && (Settings as SmotsSettings).UpdashButton.ConsumePress();
        if (updashing) {
            player.DashesLocal--;
            player.StateMachine.State = Player.States.Dashing;
        }
    }
    public delegate void orig_SetDashSpeed(Player player, in Vec2 dir);
    [On.Celeste64.Player.SetDashSpeed]
    static void Up(orig_SetDashSpeed orig, Player player, in Vec2 dir) {
        if (updashing) {
            player.Velocity = player.Velocity with { Z = player.DashSpeed };
            player.CancelGroundSnap();
        } else orig(player, dir);
    }
}

public sealed class SmotsSettings : Celeste64.Mod.GameModSettings {
    [DefaultBinding(Keys.LeftControl)]
    [DefaultBinding(Axes.LeftTrigger, 0.4f, false)]
    [Celeste64.Mod.SettingName("Updash")]
    public VirtualButton UpdashButton { get; set; } = new VirtualButton("Crouch");
}