using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class UIBattle : UIBase
    {
        public static UIInfo Info = new UIInfo("Prefabs/UI/UIBattle", "UIBattle");
        public Text GetItemName;
        public Image ItemIcon;
        public Text AchievementTipContent;


        public Animation AchievementTipAnim;
        public Animation GetItemTipAnim;
        
        public override void OnShow()
        {
            base.OnShow();
            BattleManager.OnGetClothEvent += this.OnGetCloth;
            AchievementManager.OnGetAchievementEvent += this.OnGetAchievement;
        }

        void OnGetCloth(string id, CommonEnum.PartType part)
        {
            var tableitem = ItemConfig.GetConfigByID(id);
            this.GetItemName.text = tableitem.Name;
            Sprite icon = Resources.Load<Sprite>(tableitem.Icon);
            ItemIcon.sprite = icon;
            GetItemTipAnim.Play("getItemTipIn");
        }

        void OnGetAchievement(string id)
        {
            var tableitem = AchievementConfig.GetConfigByID(id);
            this.AchievementTipContent.text = tableitem.Desc;
            this.AchievementTipAnim.Play("achievementIn");
        }

        /*
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
