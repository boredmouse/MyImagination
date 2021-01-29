using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace WHGame
{
    public class UIAchievementTip : MonoBehaviour
    {
        public Text AchievementTipContent;
        public Animation AchievementTipAnim;
        // Start is called before the first frame update
        void Start()
        {
            AchievementManager.OnGetAchievementEvent += this.OnGetAchievement;
            //AchievementTipAnim = this.gameObject.GetComponent<Animation>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGetAchievement(string id)
        {
            var tableitem = AchievementConfig.GetConfigByID(id);
            this.AchievementTipContent.text = tableitem.Desc;
            this.AchievementTipAnim.Play("achievementIn");
        }
    }
}

