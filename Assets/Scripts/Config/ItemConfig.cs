using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class ItemConfig
    {
        public static readonly List<ItemT> Configs = new List<ItemT>{
        new ItemT(
           "001","桃心内裤","Textures/Items/neinei"
       ),
       new ItemT(
           "002","背心","Textures/Items/beixinicon"
       )
        };
        public static ItemT GetConfigByID(string id)
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

    public class ItemT
    {
        public string ID;
        public string Name;
        public string Icon;
        public ItemT(string id, string name, string icon)
        {
            ID = id;
            Name = name;
            Icon = icon;
        }
    }

}
