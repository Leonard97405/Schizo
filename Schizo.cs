using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;

namespace Schizo
{
    using LabApi.Events.CustomHandlers;
    using LabApi.Features;
    using LabApi.Loader.Features.Plugins;


    public class Schizo : Plugin<Config>
    {
        public static Schizo Singleton = null;
    
        public override void Enable()
        {
            Singleton = this;
            NetRoleManager.NetRoleManager.Instance.RegisterRole(Config.Sz);
            CustomHandlersManager.RegisterEventsHandler(new RoleEvents());

        }

        public override void Disable()
        {
            Singleton = null;
            CustomHandlersManager.UnregisterEventsHandler(new RoleEvents());
        }

        public override string Name { get; } = "Lo Schizo";
        public override string Description { get; } = "Plugin per ruolo custom Schizo";
        public override string Author { get; } = "Lenard";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        
    }
}