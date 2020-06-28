using HarmonyLib;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmSilK.ReputationCooldowns
{
    public class ReputationCooldowns : RocketPlugin<Configuration>
    {
        public static ReputationCooldowns Instance { get; private set; }

        public static Harmony HarmonyInstance { get; private set; }

        public Range[] RepCooldowns => Configuration.Instance.RepCooldowns;

        protected override void Load()
        {
            Instance = this;

            Logger.Log("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Logger.Log("");
            Logger.Log("Loading Reputation Cooldowns by SilK (https://github.com/IAmSilK)");
            Logger.Log("");
            Logger.Log("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Logger.Log("");

            // Check for conflicts
            foreach (Range range in RepCooldowns)
            {
                if (range.Min >= range.Max)
                {
                    throw new Exception("Range is invalid " + range.ToString());
                }
            }

            for (int i = 0; i < RepCooldowns.Length - 1; i++)
            {
                int min1 = RepCooldowns[i].Min;
                int max1 = RepCooldowns[i].Max;

                for (int j = i + 1; j < RepCooldowns.Length; j++)
                {
                    int min2 = RepCooldowns[j].Min;
                    int max2 = RepCooldowns[j].Max;

                    // <---- Rep ---->


                    // Min1 ------------ Max1
                    //       Min2 ------ Max2

                    // Min1 ------ Max1
                    // Min2 ------------Max2

                    //       Min1 ------ Max1
                    // Min2 ------- Max2

                    // Min1 ------- Max1
                    //       Min2 -------- Max2

                    if (min1 == min2 || max1 == max2 ||
                        (min1 > min2 && min1 < max2) ||
                        (max1 > min2 && max1 < max2))
                    {
                        throw new Exception("Ranges conflict " + RepCooldowns[i].ToString() + " " + RepCooldowns[j].ToString());
                    }
                }
            }

            HarmonyInstance = new Harmony("com.iamsilk.reputationcooldowns");
            HarmonyInstance.PatchAll();

            Logger.Log("Loaded defined ranges:");
            foreach (Range range in RepCooldowns.OrderBy(x => x.Min))
            {
                string min = range.MinStr;
                string max = range.MaxStr;

                min = min == null ? "-Infinity" : min;
                max = max == null ? "Infinity" : max;

                Logger.Log("\t" + min + " to " + max + " has multiplier of " + range.Multiplier);
            }
        }

        protected override void Unload()
        {
            if (HarmonyInstance != null)
            {
                HarmonyInstance.UnpatchAll();
                HarmonyInstance = null;
            }

            Instance = null;
        }

        public float GetCooldownMultiplier(int rep)
        {
            Range range = RepCooldowns.FirstOrDefault(x => x.WithinRange(rep));

            return range == null ? 1f : range.Multiplier;
        }
    }
}
