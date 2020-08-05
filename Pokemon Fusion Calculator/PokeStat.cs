using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Fusion_Calculator
{
    class PokeStat
    {
        public string Name = "";
        public DexData.types PrimaryType;
        public DexData.types SecondaryType;
        public DexData.types TypeException1;
        public DexData.types TypeException2;
        public DexData.types TypeException3;
        public int DexNumber = 0;
        public int HP = 0;
        public int Attack = 0;
        public int Defense = 0;
        public int SpAtk = 0;
        public int SpDef = 0;
        public int Speed = 0;
        public int[] statlist;

        public PokeStat(string n, int d, int hp, int atk, int def, int spa, int spd, int spe, DexData.types pt, DexData.types st, DexData.types e1, DexData.types e2, DexData.types e3)
        {
            Name = n;
            DexNumber = d;
            PrimaryType = pt;
            SecondaryType = st;
            TypeException1 = e1;
            TypeException2 = e2;
            TypeException3 = e3;
            HP = hp;
            Attack = atk;
            Defense = def;
            SpAtk = spa;
            SpDef = spd;
            Speed = spe;
            statlist = new int[] {hp, atk, def, spa, spd, spe};
        }
    }
}