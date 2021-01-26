using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class Global : MonoBehaviour
    {
        public static Global Instance;
        public static bool IsTestScene = true;


        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            if (IsTestScene)
            {
                return;
            }
            //全局控制类
            GameObject go = new GameObject("Global");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<Global>();

            //场景控制类
            MySceneManager.Init();

            BattleManager.Init();

            //全局Canvas，UI控制类
            var prefab = Resources.Load("Prefabs/UI/Canvas", typeof(GameObject)) as GameObject;
            GameObject GlobalCanvas = Object.Instantiate(prefab) as GameObject;
            GlobalCanvas.name = "GlobalCanvas";
            DontDestroyOnLoad(GlobalCanvas);

            GlobalCanvas.AddComponent<UIManager>();
            UIManager.Instance = GlobalCanvas.GetComponent<UIManager>();
            UIManager.Instance.Init();
        }
        void Update()
        {

        }
    }

}
