using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WHGame
{
    public class DistanceRuler : MonoBehaviour
    {
        private GameObject player;
        public Text Distance1;
        public Text Distance2;
        public Text Distance3;
        public Text Distance4;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("player");
        }

        // Update is called once per frame
        void Update()
        {
            float playerD = player.transform.position.x * GameConfig.distanceZoom;
            Distance2.text = playerD.ToString("F2") + "m";
            Distance1.text = (playerD - 3).ToString("F2") + "m";
            Distance3.text = (playerD + 3).ToString("F2") + "m";
            Distance4.text = (playerD + 3).ToString("F2") + "m";
        }
    }
}

