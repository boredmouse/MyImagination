using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class ModelResourceConfig
    {
        public static readonly List<ModelResourceT> Configs = new List<ModelResourceT>{
        new ModelResourceT(
           1,"UnityChan","Prefabs/GameObj/SD_unitychan_generic","Animations/UnityChan/"
       ),
       new ModelResourceT(
           2,"a","b","c"
       )
   };
        public static ModelResourceT GetConfigByID(int id)
        {
            ModelResourceT config = ConfigManager.GetConfigByID<ModelResourceT>(Configs, id);
            return config;
        }
    }

    public class ModelResourceT : ConfigBase
    {
        //public int ID;
        public string Name;
        public string Path;
        public string AnimPath;
        public ModelResourceT(int id, string name, string path, string animPath)
        {
            ID = id;
            Name = name;
            Path = path;
            AnimPath = animPath;
        }
    }

}
