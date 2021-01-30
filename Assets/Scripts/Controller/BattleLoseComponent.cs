﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class BattleLoseComponent : MonoBehaviour
    {
        public string DieType = "吐死";
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("player"))
            {
                BattleManager.BattleLose(this.DieType);

            }
        }
    }
}

