using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IAmSilK.ReputationCooldowns
{
    public class Range
    {
        [XmlAttribute("Min")]
        public string MinStr
        {
            get
            {
                return Min == int.MinValue ? null : Min.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Min = int.MinValue;
                }
                else
                {
                    Min = int.Parse(value);
                }
            }
        }

        // Inclusive minimum
        [XmlIgnore]
        public int Min = int.MinValue;

        [XmlAttribute("Max")]
        public string MaxStr
        {
            get
            {
                return Max == int.MaxValue ? null : Max.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Max = int.MaxValue;
                }
                else
                {
                    Max = int.Parse(value);
                }
            }
        }

        // Exclusive maximum
        [XmlIgnore]
        public int Max = int.MaxValue;

        public float Multiplier = 1f;

        public Range() { }

        public Range(int min, int max, float multiplier)
        {
            Min = min;
            Max = max;
            Multiplier = multiplier;
        }

        public override string ToString()
        {
            return $"Range(Min:{Min},Max:{Max},Multiplier:{Multiplier}";
        }

        public bool WithinRange(int num)
        {
            return num >= Min && (num < Max || Max == int.MaxValue);
        }
    }
}
