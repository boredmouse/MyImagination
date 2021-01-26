using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class BattleConfig
    {
        public static readonly List<BattleT> Configs = new List<BattleT>{
        new BattleT(
           1,new float[3]{0,0,0},new float[3]{0,180f,0}
           )
   };
        public static BattleT GetConfigByID(int id)
        {
            BattleT config = ConfigManager.GetConfigByID<BattleT>(Configs, id);
            return config;
        }
    }

    public class BattleT : ConfigBase
    {
        //public int ID;
        public float[] PlayerBornPos;
        public float[] PlayerBornAngle;
        public BattleT(int id, float[] bornPos, float[] bornAngle)
        {
            ID = id;
            PlayerBornPos = bornPos;
            PlayerBornAngle = bornAngle;
        }
    }

}
