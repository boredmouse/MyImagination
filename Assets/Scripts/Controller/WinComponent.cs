using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class WinComponent : MonoBehaviour
    {
        public string WinType = "回家喽！";
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
                BattleManager.BattleStop = true;
                StartCoroutine(win());
            }
        }

        IEnumerator win() {
            yield return new WaitForSeconds(2f);
             BattleManager.BattleWin(this.WinType);
        }
    }
}

