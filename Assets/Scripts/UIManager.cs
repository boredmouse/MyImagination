using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance; /* = new UIManager()*/
        public const int MaxDeactiveUINum = 10;

        private Stack<UIBase> activeUIStack = new Stack<UIBase>();

        //private List<UIBase> activeUIList = new List<UIBase>();
        private List<UIBase> deactiveUIList = new List<UIBase>();

        public void Init()
        {
            Debug.Log("UIManager Init");
            MySceneManager.OnSceneLoadedEvent += this._OnSceneLoaded;
            if (Global.IsTestScene)
            {
                this.ShowUIWithStack(UIBattle.Info);
            }
            else
            {
                this.ShowUIWithStack(UIStartGame.Info);
            }
        }

        public void ShowUIWithStack(UIInfo uiInfo)
        {
            UIBase forShowUI = null;
            //todo 如果已经打开了
            for (int i = 0; i < this.deactiveUIList.Count; i++)
            {
                if (deactiveUIList[i].UiInfo == uiInfo)
                {
                    forShowUI = deactiveUIList[i];
                    deactiveUIList.RemoveAt(i);
                    break;
                }
            }
            if (forShowUI == null)
            {
                var prefab = Resources.Load(uiInfo.path, typeof(GameObject)) as GameObject;
                GameObject newUI = Object.Instantiate(prefab, this.gameObject.transform) as GameObject;
                forShowUI = newUI.GetComponent<UIBase>();
                forShowUI.UiInfo = uiInfo;
            }
            else
            {
                this._SetUIToTop(forShowUI);
                forShowUI.gameObject.SetActive(true);
            }
            this.activeUIStack.Push(forShowUI);
            forShowUI.OnShow();
        }

        private void _SetUIToTop(UIBase ui)
        {
            if (ui.gameObject != null)
            {
                int index = this.gameObject.transform.childCount - 1;
                ui.gameObject.transform.SetSiblingIndex(index);
            }
        }

        public void HideUIFromStack(UIInfo uiInfo)
        {
            UIBase curUI = this.activeUIStack.Peek();
            if (curUI.UiInfo != uiInfo)
            {
                Debug.LogError(uiInfo.uiName + "Already Hide");
            }
            else
            {
                curUI = this.activeUIStack.Pop();
                curUI.gameObject.SetActive(false);
                curUI.OnHide();
                this._AddToDeactiveList(curUI);
            }

            if (this.activeUIStack.Count > 0)
            {
                curUI = this.activeUIStack.Peek();
                curUI.gameObject.SetActive(true);
            }
        }

        private void _AddToDeactiveList(UIBase uibase)
        {
            if (deactiveUIList.Count >= UIManager.MaxDeactiveUINum)
            {
                UIBase destroyUI = deactiveUIList[0];
                deactiveUIList.RemoveAt(0);
                GameObject.Destroy(destroyUI.gameObject);
            }
            deactiveUIList.Add(uibase);
        }

        public void HideAllAndShow(UIInfo uiInfo)
        {
            int curUICount = this.activeUIStack.Count;
            for (int i = 1; i <= curUICount; i++)
            {
                UIBase curUI = this.activeUIStack.Peek();
                if (curUI.UiInfo != null)
                {
                    curUI = this.activeUIStack.Pop();
                    curUI.gameObject.SetActive(false);
                    curUI.OnHide();
                    this._AddToDeactiveList(curUI);
                }
            }
            this.ShowUIWithStack(uiInfo);
        }

        private void _OnSceneLoaded(string sceneName)
        {
            if (sceneName == SceneName.Battle)
            {
                this.HideAllAndShow(UIBattle.Info);
            }
            else if (sceneName == SceneName.Start)
            {
                this.HideAllAndShow(UIStartGame.Info);
            }
            else if (sceneName == SceneName.Home)
            {
                this.HideAllAndShow(UIHome.Info);
            }
        }
    }

}
