using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class BattleManager : MonoBehaviour
    {
        private static BattleT curConfig = BattleConfig.Configs[0];
        private static SingleBattleController battleControl;
        public static bool BattleStop = false;

        public static GameObject PatObj;



        #region 战斗事件
        public delegate void OnGetCloth(string id, CommonEnum.PartType part);
        public static OnGetCloth OnGetClothEvent;

        public delegate void OnBattleLose(string loseTip);
        public static OnBattleLose OnBattleLoseEvent;

        public delegate void OnCreatePat(string id);
        public static OnCreatePat OnCreatePatEvent;
        #endregion



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
            BattleStop = false;
            battleControl = new SingleBattleController();
            battleControl.BattleStart(curConfig);
        }

        public static void BattleLose(string loseTip)
        {
            Debug.Log("GameLose");
            BattleStop = true;
            OnBattleLoseEvent(loseTip);
            AudioManager.Instance.PlayAudioClip(AudioManager.ClipName.Lose);
        }

        public static void CreatePat(string id)
        {
            OnCreatePatEvent(id);
            //todo 生成宠物
            if (PatObj != null)
            {
                Destroy(PatObj, 0);
            }
            var tableitem = PatConfig.GetConfigByID(id);
            string finalPath = tableitem.Path;
            var prefab = Resources.Load(finalPath, typeof(GameObject)) as GameObject;
            GameObject go = Object.Instantiate(prefab,new Vector3(GameConfig.PatBornX,0,0),Quaternion.identity) as GameObject;
            
            PatObj = go;
        }
    }

}
