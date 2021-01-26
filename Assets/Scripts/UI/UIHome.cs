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
        /*
        public Button animBtn;
        public Button battleBtn;
        public Button fashionBtn;
        public Button settingBtn;

        public GameObject dragObj;
        private float _dragBeginX;
        private float _dragBeginAngleY;
        private float _minAngleY = -35f;
        private float _maxAngleY = 15f;
        private GameObject mainCamera;

        private PlayerController playerModel;
        // Start is called before the first frame update
        void Start()
        {
            animBtn.onClick.AddListener(OnClickAnim);

            EventTriggerLisener.Get(dragObj).OnDragBegin += this._OnDragBegin;
            EventTriggerLisener.Get(dragObj).OnDragEvent += this._OnDrag;
            //battleBtn.onClick.AddListener(OnClickBattle);
        }

        public override void OnShow()
        {
            this.InitPlayerModel();
            this.mainCamera = GameObject.Find("Main Camera");
        }

        public override void OnHide()
        {
            this.mainCamera = null;
            this.playerModel.Dispose();
            this.playerModel = null;
        }

        void OnEnable()
        {
            Animation uiAnim = this.gameObject.GetComponent<Animation>();
            if (uiAnim != null)
            {
                uiAnim.Play("UIHomeIn");
            }
        }


        private void InitPlayerModel()
        {
            float[] bornPos = new float[3] { -0.68f, 0.12f, -0.04f };
            float[] bornAngle = new float[3] { 0, 150f, 0 };
            ModelResourceT PlayerResourceInfo = ModelResourceConfig.GetConfigByID(1);
            this.playerModel = PlayerController.CreatePlayer(PlayerResourceInfo, bornPos, bornAngle);
            this.playerModel.InitAnimation();
        }



        public void OnClickAnim()
        {
        }

        public void _OnDragBegin(GameObject o, PointerEventData data)
        {
            //Debug.LogError("_OnDragBegin data:"+data.position.x+","+data.position.y);
            this._dragBeginX = data.position.x;
            if (this.mainCamera != null)
            {
                this._dragBeginAngleY = this.mainCamera.transform.eulerAngles.y;
                if (this._dragBeginAngleY > 300f)
                {
                    this._dragBeginAngleY -= 360f;
                }
            }
        }
        public void _OnDrag(GameObject o, PointerEventData data)
        {
            //Debug.LogError("_OnDrag data:"+data.position.x+","+data.position.y);
            float change = this._dragBeginAngleY - (data.position.x - this._dragBeginX) / 20f;
            change = Mathf.Clamp(change, this._minAngleY, this._maxAngleY);
            this.mainCamera.transform.eulerAngles = new Vector3(0, change, 0);

        }
    */}

}
