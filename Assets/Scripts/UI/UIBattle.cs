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
        
        public override void OnShow()
        {
            base.OnShow();
            BattleManager.OnGetClothEvent += this.OnGetCloth;
            StartCoroutine(FirstMeet());
        }

        void OnGetCloth(string id, CommonEnum.PartType part)
        {
            var tableitem = ItemConfig.GetConfigByID(id);
            this.GetItemName.text = tableitem.Name;
            Sprite icon = Resources.Load<Sprite>(tableitem.Icon);
            ItemIcon.sprite = icon;
            GetItemTipAnim.Play("getItemTipIn");
        }

         IEnumerator FirstMeet()
        {
            yield return new WaitForSeconds(0.5f);
            AchievementManager.GetAchievement("001");
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
