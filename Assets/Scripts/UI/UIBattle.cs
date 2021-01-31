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

        public Text TimeText;

        private float time=0;

        void Start() 
        {
            BackBtn.onClick.AddListener(this.OnClickBack);
            this.BackBtn.gameObject.SetActive(false);
        }

        void Update()
        {
            if (BattleManager.BattleStop)
            {
                return;
            }
            this.time += Time.deltaTime;
            BattleManager.BattleTime = this.time;
            
            this.TimeText.text = this.time.ToString("F2")+"s";
        }
        
        public override void OnShow()
        {
            base.OnShow();
            this.LoseTipObj.SetActive(false);
            BattleManager.OnGetClothEvent += this.OnGetCloth;
            BattleManager.OnBattleLoseEvent += this.OnBattleLose;
            BattleManager.OnBattleWinEvent += this.OnBattleWin;
            StartCoroutine(FirstMeet());
            TimeText.gameObject.SetActive(true);
            time = 0;
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
    }

}
