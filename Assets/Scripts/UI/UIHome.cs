using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace WHGame
{
    public class UIHome : UIBase
    {
        public static UIInfo Info = new UIInfo("Prefabs/UI/UIHome", "UIHome");
        
        public Button JoinBtn;


        // Start is called before the first frame update
        void Start()
        {
            JoinBtn.onClick.AddListener(this.OnClickJoin);
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        void OnEnable()
        {
        }
        void OnClickJoin()
        {
            MySceneManager.EnterScene(SceneName.Battle);
        }

    }

}
