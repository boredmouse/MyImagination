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
       
        public Animation GetItemTipAnim;

        public GameObject LoseTipObj;
        public Text LoseContent;

        public GameObject WinTipObj;
        public Text WinContent;

        public Button BackBtn;

        void Start() 
        {
            BackBtn.onClick.AddListener(this.OnClickBack);
            this.BackBtn.gameObject.SetActive(false);
        }
        
        public override void OnShow()
        {
            base.OnShow();
            this.LoseTipObj.SetActive(false);
            BattleManager.OnGetClothEvent += this.OnGetCloth;
            BattleManager.OnBattleLoseEvent += this.OnBattleLose;
            BattleManager.OnBattleWinEvent += this.OnBattleWin;
            StartCoroutine(FirstMeet());
        }

        public override void OnHide()
        {
            base.OnHide();
            BattleManager.OnGetClothEvent -= this.OnGetCloth;
            BattleManager.OnBattleLoseEvent -= this.OnBattleLose;
            BattleManager.OnBattleWinEvent -= this.OnBattleWin;
            
        }

        void OnGetCloth(string id, CommonEnum.PartType part)
        {
            var tableitem = ItemConfig.GetConfigByID(id);
            this.GetItemName.text = tableitem.Name;
            Sprite icon = Resources.Load<Sprite>(tableitem.Icon);
            ItemIcon.sprite = icon;
            GetItemTipAnim.Play("getItemTipIn");
        }

        void OnBattleLose(string loseTip)
        {
            this.LoseTipObj.SetActive(true);
            this.LoseContent.text = loseTip;
            this.BackBtn.gameObject.SetActive(true);
        }

         void OnBattleWin(string winTip)
        {
            this.WinTipObj.SetActive(true);
            this.WinContent.text = winTip;
            this.BackBtn.gameObject.SetActive(true);
        }

         IEnumerator FirstMeet()
        {
            yield return new WaitForSeconds(0.5f);
            AchievementManager.GetAchievement("001");
        }

        void OnClickBack()
        {
            MySceneManager.EnterScene(SceneName.Home);
            AudioManager.Instance.PlayAudioClip(AudioManager.ClipName.ClickBtn);
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
