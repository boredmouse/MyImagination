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
        public Button backButton;
        public GameObject helpPanel;


        // Start is called before the first frame update
        void Start()
        {
            startButton.onClick.AddListener(this.OnClickStart);
            helpButton.onClick.AddListener(this.OnClickHelp);
            backButton.onClick.AddListener(this.OnClickBack);
            this.helpPanel.SetActive(false);
        }

        public void OnClickStart()
        {
            MySceneManager.EnterScene(SceneName.Home);
            AudioManager.Instance.PlayAudioClip(AudioManager.ClipName.ClickStart);
        }

        public void OnClickHelp()
        {
            //UIManager.Instance.HideUIFromStack(UIStartGame.Info);
            AudioManager.Instance.PlayAudioClip(AudioManager.ClipName.ClickBtn);
            this.helpPanel.SetActive(true);
        }
        public void OnClickBack()
        {
            this.helpPanel.SetActive(false);
            AudioManager.Instance.PlayAudioClip(AudioManager.ClipName.ClickBtn);
        }
    }

}
