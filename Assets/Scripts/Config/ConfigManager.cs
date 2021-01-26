using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class ConfigManager
    {
        public static T GetConfigByID<T>(List<T> configs, int id) where T : ConfigBase
        {
            int cnt = configs.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (configs[i].ID == id)
                {
                    return configs[i];
                }
            }
            return null;
        }
    }
    public class ConfigBase
    {
        public int ID;
    }

}
