using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class AchievementConfig
    {
        public static readonly List<AchievementT> Configs = new List<AchievementT>{
        new AchievementT(
           "001","初次见面"
       ),
       new AchievementT(
           "002","吐死"
       ),
       new AchievementT(
           "003","像个人样"
       )
        };
        public static AchievementT GetConfigByID(string id)
        {
            int cnt = Configs.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (Configs[i].ID == id)
                {
                    return Configs[i];
                }
            }
            return null;
        }
    }

    public class AchievementT
    {
        public string ID;
        public string Desc;
        public AchievementT(string id, string desc)
        {
            ID = id;
            Desc = desc;
        }
    }

}
