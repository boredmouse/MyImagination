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
        public Button closeButton;

        // Start is called before the first frame update
        void Start()
        {
            startButton.onClick.AddListener(OnClickStart);
            closeButton.onClick.AddListener(OnClickClose);
        }

        public void OnClickStart()
        {
            MySceneManager.EnterScene(SceneName.Home);
        }

        public void OnClickClose()
        {
            //UIManager.Instance.HideUIFromStack(UIStartGame.Info);
        }
    }

}
