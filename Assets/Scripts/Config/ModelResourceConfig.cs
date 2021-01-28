using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class ModelResourceConfig
    {
        public static readonly List<ModelResourceT> Configs = new List<ModelResourceT>{
        new ModelResourceT(
           "000000000000000","naked/hero"
       ),
       new ModelResourceT(
           "000000000001000","neinei/pants"
       )
   };
        public static ModelResourceT GetConfigByID(string id)
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

    public class ModelResourceT
    {
        public string ID;
        public string Path;
        public ModelResourceT(string id, string path)
        {
            ID = id;
            Path = path;
        }
    }

}
