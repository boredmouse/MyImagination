using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class BattleLoseComponent : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("OnTriggerEnter2D  "+col.gameObject.name);
            if (col.gameObject.CompareTag("player"))
            {
                BattleManager.BattleLose("吐死");

            }
        }
    }
}

