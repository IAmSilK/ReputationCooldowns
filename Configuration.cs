using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmSilK.ReputationCooldowns
{
    public class Configuration : IRocketPluginConfiguration
    {
        public Range[] RepCooldowns;

        public void LoadDefaults()
        {
            RepCooldowns = new Range[]
            {
                new Range(int.MinValue, -10, 1.5f),
                new Range(-10, 10, 1),
                new Range(10, int.MaxValue, 0.5f),
            };
        }
    }
}
