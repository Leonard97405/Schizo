using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using InventorySystem.Items.Usables.Scp330;
using LabApi.Events.Handlers;
using MEC;

namespace Schizo
{
    using Interactables.Interobjects.DoorUtils;
    using LabApi.Events.Arguments.PlayerEvents;
    using LabApi.Events.CustomHandlers;
    using LabApi.Features.Wrappers;
    using PlayerRoles;

    using NetRoleManager;
    public class Main : CustomRole
    {
        
        public override string RoleId { get; set; } = "schizo";
        public override string Name { get; set; } = "The Schizo";
        public override string Description { get; set; } = "Non riesci a smettere di sentire delle voci";
        public override RoleTypeId RoleTypeId { get; set; } = RoleTypeId.ClassD;
        public override int SpawnChance { get; set; } = Schizo.Singleton?.Config?.ChanceSpawn ?? 100;
        public override int MinPlayersRequired { get; set; } = Schizo.Singleton?.Config?.MinPlayer ?? 0;
        public override int MaxSpawnsPerRound { get; set; } = Schizo.Singleton?.Config?.MaxSpawn ?? 1;
        public override string Color { get; set; } = "red";
        public override void OnRoleAdded(Player player)
        {
            player.EnableEffect<Scp1576>(1,2400,false);
        }
    }

    public class RoleEvents : CustomEventsHandler
    {
        private DynamicHint dH = new DynamicHint()
        {
            Id = "schizo",
            TargetY = 900,
            FontSize = 30,
            Text = "<b>Le <color=red> voci </color> %status% </b>",
            SyncSpeed = HintSyncSpeed.UnSync,
        };

        private DynamicHint sH = new DynamicHint()
        {
            Id = "schizogramm",
            TargetY = 900,
            FontSize = 30,
            Text = "<b> Le voci non ti permettono di usare quest'item </b>"
        };
        
        public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
        {
            if (!NetRoleManager.Instance.HasCustomRole(ev.Player,Schizo.Singleton.Config.Sz.RoleId) ||Schizo.Singleton.Config.NoVoci <= 5) return;
            PlayerDisplay pD = PlayerDisplay.Get(ev.Player);
            if (ev.UsableItem.Type == ItemType.Painkillers)
            {
                DynamicHint tH = dH;
                if (!ev.Player.HasEffect<Scp1576>()) return;
                tH.Text = tH.Text.Replace("%status%", "si sono fermate!!");
                Timing.CallDelayed(1f,()=>pD.AddHint(tH));
                ev.Player.DisableEffect<Scp1576>();
                Timing.CallDelayed(3f, () => pD.RemoveHint(tH));
                tH = dH;
                Timing.CallDelayed(Schizo.Singleton.Config.NoVoci, () =>
                {
                    ev.Player.EnableEffect<Scp1576>(1, 2400, false);
                    tH.Text =tH.Text.Replace("%status%", "sono tornate..");
                    tH.Id = "stopVociSchizo";
                    Timing.CallDelayed(1f,()=>pD.AddHint(tH));
                    Timing.CallDelayed(3f, () => pD.RemoveHint(tH));
                });
            }
            else if (ev.UsableItem.Type == ItemType.SCP1576)
            {
                ev.IsAllowed = false;
                pD.AddHint(sH);
                Timing.CallDelayed(3f, () => pD.RemoveHint(sH));
                
            }
        }

       
    }
}