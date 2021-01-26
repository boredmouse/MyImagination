using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class UIBase : MonoBehaviour
    {
        public UIInfo UiInfo;

        public virtual void OnShow()
        {
            Debug.Log(this.UiInfo.uiName + " OnShow");
        }

        public virtual void OnHide()
        {
            Debug.Log(this.UiInfo.uiName + " OnHide");
        }
    }

}
