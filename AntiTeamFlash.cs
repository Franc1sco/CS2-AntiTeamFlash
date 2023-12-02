using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Utils;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace AntiTeamFlash;


[MinimumApiVersion(50)]
public class AntiTeamFlash : BasePlugin
{
    public override string ModuleName => "Anti Team Flash";
    public override string ModuleAuthor => "Franc1sco Franug";
    public override string ModuleVersion => "0.0.1";

    //private List<CHandle<CCSPlayerPawn>> flashPlayers = new List<CHandle<CCSPlayerPawn>>();
    private CCSPlayerController? currentFlashPlayer;

    public override void Load(bool hotReload)
    {
        /*
        RegisterListener<Listeners.OnEntitySpawned>(entity =>
        {
            var designerName = entity.DesignerName;

            switch (designerName)
            {
                case "flashbang_projectile":
                    var flashbang = (CBaseGrenade) entity;

                    new Timer(0.1f, () => {
                        flashPlayers.Add(flashbang.Thrower);
                    });
                    return;
            }
        });*/

        RegisterEventHandler<EventFlashbangDetonate>((@event, info) =>
        {
            /*new Timer(1.0f, () =>
            {
                flashPlayers.RemoveAt(0);
            });*/
            if (!@event.Userid.IsValid) return HookResult.Continue;
            if (!@event.Userid.PlayerPawn.IsValid) return HookResult.Continue;

            currentFlashPlayer = @event.Userid;
            new Timer(0.1f, () =>
            {
                //if (currentFlashPlayer == @event.Userid)
                //{
                    currentFlashPlayer = null;
                //}
            });
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

            Server.PrintToChatAll("comprobando...");
            //if (flashPlayers[0].Value.TeamNum == player.TeamNum)
            if (attacker.PlayerPawn.Value.TeamNum == player.TeamNum)
            {
                Server.PrintToChatAll("cogido");
                @event.BlindDuration = 0.1f;
                return HookResult.Changed;
                //player.PlayerPawn.Value.FlashMaxAlpha = 0.5f;
                //player.PlayerPawn.Value.FlashDuration = 0.0f;
                //player.PrintToChat("descegado");*/
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

            Server.PrintToChatAll("comprobando2...");
            //if (flashPlayers[0].Value.TeamNum == player.TeamNum)
            if (attacker.PlayerPawn.Value.TeamNum == player.TeamNum)
            {
                Server.PrintToChatAll("cogido2");
                player.PlayerPawn.Value.FlashMaxAlpha = 0.5f;
                new Timer(player.PlayerPawn.Value.FlashDuration, () =>
                {
                    if (!player.IsValid) return;
                    if (!player.PlayerPawn.IsValid) return;
                    player.PlayerPawn.Value.FlashMaxAlpha = 0.5f;
                    //player.PlayerPawn.Value.FlashDuration = 0.0f;
                    Server.PrintToChatAll("descegado2");
                });
                //player.PlayerPawn.Value.FlashMaxAlpha = 0.5f;
                //player.PlayerPawn.Value.FlashDuration = 0.0f;
                //player.PrintToChat("descegado");*/
            }

            return HookResult.Continue;
        });


    }
}

