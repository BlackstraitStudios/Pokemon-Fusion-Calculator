using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Fusion_Calculator
{
    class FusionData
    {
        public int HP;
        public int ATK;
        public int DEF;
        public int SPA;
        public int SPD;
        public int SPE;
        public DexData.types pType;
        public DexData.types sType;
        public int[] stats;

        public FusionData(int hp, int atk, int def, int spa, int spd, int spe, DexData.types pt, DexData.types st)
        {
            HP = hp;
            ATK = atk;
            DEF = def;
            SPA = spa;
            SPD = spd;
            SPE = spe;
            pType = pt;
            sType = st;
            stats = new int[] { hp, atk, def, spa, spd, spe};
        }

        public FusionData(int hp, int atk, int def, int spa, int spd, int spe, DexData.types pt) : this(hp, atk, def, spa, spd, spe, pt, DexData.types.none)
        {
            
        }
    }
}
