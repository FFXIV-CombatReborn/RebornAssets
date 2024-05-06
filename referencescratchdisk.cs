class GazeExample(BossModule module) : Components.CastGaze(module, ActionID.MakeSpell(AID.GazeExample));

class StatusDrivenForcedMarch(BossModule module) : Components.StatusDrivenForcedMarch(module, 2, (uint)SID.ForwardMarch, (uint)SID.AboutFace, (uint)SID.LeftFace, (uint)SID.RightFace, activationLimit: 8);

class SelfTargetedAOEs(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SelfTargetedAOEs), new AOEShapeCone(20, 60.Degrees()));
class SelfTargetedAOEs(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SelfTargetedAOEs), new AOEShapeCircle(9));
class SelfTargetedAOEs(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SelfTargetedAOEs), new AOEShapeRect(60, 7.5f));
class SelfTargetedAOEs(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SelfTargetedAOEs), new AOEShapeDonut(7, 60));

class LocationTargetedAOEs(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.LocationTargetedAOEs), 6);
class Towers(BossModule module) : Components.CastTowers(module, ActionID.MakeSpell(AID.Explosion), 6);

class SimpleProtean(BossModule module) : Components.SimpleProtean(module, ActionID.MakeSpell(AID.SimpleProtean), new AOEShapeCone(21, 15.Degrees()));

class ChargeAOEs(BossModule module) : Components.ChargeAOEs(module, ActionID.MakeSpell(AID.ChargeAOEs), 2.5f);

class Chains(BossModule module) : Components.Chains(module, (uint)TetherID.Chains, ActionID.MakeSpell(AID.Chains));

class SpreadFromCastTargets(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.SpreadFromCastTargets), 6);
class StackWithCastTargets(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.StackWithCastTargets), 6, 8);

class KnockbackFromCastTarget(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.KnockbackFromCastTarget), 30, stopAtWall: true);

class CastSharedTankbuster(BossModule module) : Components.CastSharedTankbuster(module, ActionID.MakeSpell(AID.CastSharedTankbuster), 6);
class SingleTargetCast(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.SingleTargetCast));
class Cleave(BossModule module) : Components.Cleave(module, ActionID.MakeSpell(AID.Cleave), new AOEShapeCone(12, 60.Degrees());

class RaidwideCast(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.RaidwideCast));

class PersistentVoidzone(BossModule module) : Components.PersistentVoidzone(module, 6, m => m.Enemies(OID.PersistentVoidzone).Where(z => z.EventState != 7));
class PersistentVoidzoneAtCastTarget(BossModule module) : Components.PersistentVoidzoneAtCastTarget(module, 6, ActionID.MakeSpell(AID.PersistentVoidzoneAtCastTarget), m => m.Enemies(OID.PersistentVoidzoneAtCastTarget).Where(z => z.EventState != 7), 0.8f);

class CastLineOfSightAOE(BossModule module) : Components.CastLineOfSightAOE(module, ActionID.MakeSpell(AID.CastLineOfSightAOE), 60, false)
{
    public override IEnumerable<Actor> BlockerActors() => Module.Enemies(OID.CastLineOfSightAOE).Where(a => !a.IsDead);
}
                                            
class StayMove(BossModule module) : Components.StayMove(module)
{
    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID is SID.Stay)
        {
            if (Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0 && slot < Requirements.Length)
                Requirements[slot] = Requirement.Stay;
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID is SID.None)
        {
            if (Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0 && slot < Requirements.Length)
                Requirements[slot] = Requirement.None;
        }
    }
}

class StayMove(BossModule module) : Components.StayMove(module)
{
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.Move)
        {
            if (Raid.FindSlot(caster.TargetID) is var slot && slot >= 0 && slot < Requirements.Length)
                Requirements[slot] = Requirement.Move;
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.None)
        {
            if (Raid.FindSlot(caster.TargetID) is var slot && slot >= 0 && slot < Requirements.Length)
                Requirements[slot] = Requirement.None;
        }
    }
}

class Nox : Components.StandardChasingAOEs
{
    public Nox(BossModule module) : base(module, new AOEShapeCircle(10), ActionID.MakeSpell(AID.NoxStart), ActionID.MakeSpell(AID.NoxEnd), 5.5f, 1.6f, 5) //float moveDistance, float secondsBetweenActivations, int maxCasts
    {
        ExcludedTargets = Raid.WithSlot(true).Mask();
    }

    public override void OnEventIcon(Actor actor, uint iconID)
    {
        if (iconID == (uint)IconID.Nox)
            ExcludedTargets.Clear(Raid.FindSlot(actor.InstanceID));
    }
}

class ExampleMobChaseAOE(BossModule module) : Components.GenericAOEs(module)
{
    private IReadOnlyList<Actor> _spirits = module.Enemies(OID.ExampleMobChaseAOE);

    private static readonly AOEShapeCircle _shape = new(3);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _spirits.Where(actor => !actor.IsDead).Select(b => new AOEInstance(_shape, b.Position));
}
