﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class PatConfig
    {
        public static readonly List<PatT> Configs = new List<PatT>{
        new PatT(
           "000000000000000","naked/hero"
       ),
       new PatT(
           "000000000001000","neinei/pants"
       )
   };
        public static PatT GetConfigByID(string id)
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

    public class PatT
    {
        public string ID;
        public string Path;
        public PatT(string id, string path)
        {
            ID = id;
            Path = path;
        }
    }

}