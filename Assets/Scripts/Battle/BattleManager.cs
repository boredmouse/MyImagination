using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class BattleManager : MonoBehaviour
    {
        private static BattleT curConfig = BattleConfig.Configs[0];
        private static SingleBattleController battleControl;

        public delegate void OnGetCloth(int id);
        public static OnGetCloth OnGetClothEvent;

        public static void Init()
        {
            MySceneManager.OnSceneLoadedEvent += BattleManager._OnSceneLoaded;
        }

        private static void _OnSceneLoaded(string sceneName)
        {
            if (sceneName == SceneName.Battle)
            {
                StartBattle();
            }
        }

        //切场景之前设置战斗配置
        public static void SetBattleConfig(BattleT config)
        {
            curConfig = config;
        }

        public static void StartBattle()
        {
            battleControl = new SingleBattleController();
            battleControl.BattleStart(curConfig);
        }
    }

}
