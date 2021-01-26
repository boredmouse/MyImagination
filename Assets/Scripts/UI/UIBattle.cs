using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class UIBattle : UIBase
    {
        public static UIInfo Info = new UIInfo("Prefabs/UI/UIBattle", "UIBattle");
        /*
        public delegate void OnPlayAnimDel(AnimID id, ClipMode mode, float fadeTime);
        public static OnPlayAnimDel OnPlayAnimEvent;
        [SerializeField]
        private Button closeButton;
        public Button WalkBtn;
        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(OnClickClose);
            WalkBtn.onClick.AddListener(OnClickWalk);
        }



        public void OnClickClose()
        {
            MySceneManager.EnterScene(SceneName.Start);
        }
        public void OnClickWalk()
        {
            OnPlayAnimEvent(AnimID.Walking,ClipMode.Loop,0.3f);
        }
        */
    }

}
