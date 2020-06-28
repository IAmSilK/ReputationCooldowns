using HarmonyLib;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core.Commands;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmSilK.ReputationCooldowns
{
    [HarmonyPatch(typeof(RocketCommandManager), "GetCooldown")]
    public class CooldownPatch
    {
        public static Type RocketCommandCooldown;
        public static FieldInfo PlayerField;
        public static FieldInfo CommandRequestedField;
        public static FieldInfo CommandField;
        public static FieldInfo PermissionField;

        public static Type RocketCommandManager;
        public static FieldInfo CooldownField;

        static CooldownPatch()
        {
            RocketCommandCooldown = Type.GetType("Rocket.Core.Commands.RocketCommandCooldown, Rocket.Core", true);

            PlayerField =           RocketCommandCooldown.GetField("Player");
            CommandRequestedField = RocketCommandCooldown.GetField("CommandRequested");
            CommandField =          RocketCommandCooldown.GetField("Command");
            PermissionField =       RocketCommandCooldown.GetField("ApplyingPermission");


            RocketCommandManager = typeof(RocketCommandManager);

            CooldownField = RocketCommandManager.GetField("cooldown", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static double GetCooldown(RocketCommandManager manager, IRocketPlayer player, IRocketCommand command, int reputation)
        {
            float multiplier = ReputationCooldowns.Instance.GetCooldownMultiplier(reputation);
            
            IList cooldown = (IList)CooldownField.GetValue(manager);

            object rocketCommandCooldown = null;

            foreach (object iteration in cooldown)
            {
                IRocketCommand rocketCommand = (IRocketCommand)CommandField.GetValue(iteration);
                if (rocketCommand != command) continue;

                IRocketPlayer rocketPlayer = (IRocketPlayer)PlayerField.GetValue(iteration);
                if (rocketPlayer.Id != player.Id) continue;

                rocketCommandCooldown = iteration;
                break;
            }

            if (rocketCommandCooldown == null)
            {
                return -1.0;
            }

            DateTime commandRequested = (DateTime)CommandRequestedField.GetValue(rocketCommandCooldown);
            Permission applyingPermission = (Permission)PermissionField.GetValue(rocketCommandCooldown);

            double totalSeconds = (DateTime.Now - commandRequested).TotalSeconds;

            double trueCooldown = applyingPermission.Cooldown * multiplier;

            if (trueCooldown <= totalSeconds)
            {
                cooldown.Remove(rocketCommandCooldown);
                return -1.0;
            }
            return Math.Ceiling(trueCooldown - totalSeconds);
        }

        static bool Prefix(RocketCommandManager __instance, IRocketPlayer player, IRocketCommand command, ref double __result)
        {
            if (!(player is UnturnedPlayer uPlayer))
            {
                return true;
            }

            __result = (int)GetCooldown(__instance, player, command, uPlayer.Reputation);

            return false;
        }
    }
}
