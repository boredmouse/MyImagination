using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class SingleBattleController
    {
        private BattleT curBattleInfo;
        private PlayerController mainPlayer;

        // Update is called once per frame
        void Update()
        {

        }

        public void BattleStart(BattleT config)
        {
            this.curBattleInfo = config;
            ModelResourceT PlayerResourceInfo = ModelResourceConfig.GetConfigByID(1);
            /*
            this.mainPlayer = PlayerController.CreatePlayer(PlayerResourceInfo, this.curBattleInfo.PlayerBornPos, this.curBattleInfo.PlayerBornAngle, "MainPlayer");
            this.mainPlayer.InitAnimation();*/
        }
    }

}
