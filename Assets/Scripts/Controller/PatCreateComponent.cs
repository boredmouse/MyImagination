using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class PatCreateComponent : MonoBehaviour
    {
        public string patID;
        private GameObject torch;

        private float startDistance = GameConfig.startDistance;
        private float endDistance = GameConfig.endDistance;
        private float backDistance = GameConfig.backDistance;

        private bool lookRight = false;
        private Animation anim;

        // Start is called before the first frame update
        void Start()
        {
            this.torch = GameObject.FindGameObjectWithTag("torchPos");
            anim = this.gameObject.GetComponent<Animation>();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.transform.position.x - this.torch.transform.position.x > backDistance - 1)
            {
                if (lookRight)
                {
                    this.anim.Play("lookNorm");
                    this.lookRight = false;
                }
            }
            else if (this.transform.position.x - this.torch.transform.position.x < backDistance - 5)
            {
                BattleManager.CreatePat(this.patID);
                Destroy(this.gameObject);
            }
        }
    }
}

