using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class UIStartGame : UIBase
    {
        public static UIInfo Info = new UIInfo("Prefabs/UI/UIStartGame", "UIStartGame");
        public Button startButton;
        public Button helpButton;

        // Start is called before the first frame update
        void Start()
        {
            startButton.onClick.AddListener(this.OnClickStart);
            helpButton.onClick.AddListener(this.OnClickHelp);
        }

        public void OnClickStart()
        {
            Debug.Log("click start");
            MySceneManager.EnterScene(SceneName.Home);
        }

        public void OnClickHelp()
        {
            //UIManager.Instance.HideUIFromStack(UIStartGame.Info);
        }
    }

}
