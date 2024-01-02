using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Admin;
using System.Text.Json.Serialization;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace AntiTeamFlash;

public class ConfigGen : BasePluginConfig
{
    [JsonPropertyName("Enabled")] public bool Enabled { get; set; } = true;
    [JsonPropertyName("FlashOwner")] public bool FlashOwner { get; set; } = true;
    [JsonPropertyName("AccessFlag")] public string AccessFlag { get; set; } = "";
}

[MinimumApiVersion(50)]
public class AntiTeamFlash : BasePlugin, IPluginConfig<ConfigGen>
{
    public override string ModuleName => "Anti Team Flash";
    public override string ModuleAuthor => "Franc1sco Franug";
    public override string ModuleVersion => "0.0.2";

    public ConfigGen Config { get; set; } = null!;
    public void OnConfigParsed(ConfigGen config) { Config = config; }

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerBlind>((@event, info) =>
        {
            if (!Config.Enabled) return HookResult.Continue;

            var player = @event.Userid;
            if (!player.IsValid) return HookResult.Continue;
            if (!player.PlayerPawn.IsValid) return HookResult.Continue;
            var attacker = @event.Attacker;
            if (!attacker.IsValid) return HookResult.Continue;
            if (!attacker.PlayerPawn.IsValid) return HookResult.Continue;

            if (Config.AccessFlag != "" && !AdminManager.PlayerHasPermissions(player, Config.AccessFlag)) return HookResult.Continue;

            if (attacker.PlayerPawn.Value.TeamNum == player.TeamNum && (!Config.FlashOwner || attacker.UserId != player.UserId))
            {
                @event.BlindDuration = 0.1f;
                return HookResult.Changed;
            }

            return HookResult.Continue;
        }, HookMode.Pre);

        RegisterEventHandler<EventPlayerBlind>((@event, info) =>
        {
            if (!Config.Enabled) return HookResult.Continue;

            var player = @event.Userid;
            if (!player.IsValid) return HookResult.Continue;
            if (!player.PlayerPawn.IsValid) return HookResult.Continue;

            var attacker = @event.Attacker;
            if (!attacker.IsValid) return HookResult.Continue;
            if (!attacker.PlayerPawn.IsValid) return HookResult.Continue;

            if (Config.AccessFlag != "" && !AdminManager.PlayerHasPermissions(player, Config.AccessFlag)) return HookResult.Continue;

            if (attacker.PlayerPawn.Value.TeamNum == player.TeamNum && (!Config.FlashOwner || attacker.UserId != player.UserId))
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
