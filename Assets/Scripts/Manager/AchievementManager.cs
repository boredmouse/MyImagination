using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class AchievementManager
    {
        public delegate void OnGetAchievement(string id);
        public static OnGetAchievement OnGetAchievementEvent;

        public static List<string> GotList = new List<string>();
        public static void GetAchievement(string id)
        {
            if (GotList.Contains(id))
            {
                return;
            }
            GotList.Add(id);
            if (OnGetAchievementEvent != null)
            {
                OnGetAchievementEvent(id);
            }
        }
    }
}

