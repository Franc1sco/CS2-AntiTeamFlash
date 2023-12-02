using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace AntiTeamFlash;


[MinimumApiVersion(50)]
public class AntiTeamFlash : BasePlugin
{
    public override string ModuleName => "Anti Team Flash";
    public override string ModuleAuthor => "Franc1sco Franug";
    public override string ModuleVersion => "0.0.1";

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerBlind>((@event, info) =>
        {
            var player = @event.Userid;
            if (!player.IsValid) return HookResult.Continue;
            if (!player.PlayerPawn.IsValid) return HookResult.Continue;
            var attacker = @event.Attacker;
            if (!attacker.IsValid) return HookResult.Continue;
            if (!attacker.PlayerPawn.IsValid) return HookResult.Continue;

            if (attacker.PlayerPawn.Value.TeamNum == player.TeamNum)
            {
                @event.BlindDuration = 0.1f;
                return HookResult.Changed;
            }

            return HookResult.Continue;
        }, HookMode.Pre);

        RegisterEventHandler<EventPlayerBlind>((@event, info) =>
        {
            var player = @event.Userid;
            if (!player.IsValid) return HookResult.Continue;
            if (!player.PlayerPawn.IsValid) return HookResult.Continue;

            var attacker = @event.Attacker;
            if (!attacker.IsValid) return HookResult.Continue;
            if (!attacker.PlayerPawn.IsValid) return HookResult.Continue;

            if (attacker.PlayerPawn.Value.TeamNum == player.TeamNum)
            {
                player.PlayerPawn.Value.FlashMaxAlpha = 0.5f;
                new Timer(player.PlayerPawn.Value.FlashDuration, () =>
                {
                    if (!player.IsValid) return;
                    if (!player.PlayerPawn.IsValid) return;
                    player.PlayerPawn.Value.FlashMaxAlpha = 0.5f;
                });
            }

            return HookResult.Continue;
        });
    }
}
