using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class RightItemComponent : MonoBehaviour
    {
        private GameObject torch;

        private float startDistance = GameConfig.startDistance;
        private float endDistance = GameConfig.endDistance;
        private float backDistance = GameConfig.backDistance;
        private SpriteRenderer spRenderer;
        private Color rightColor = Color.white;

        // Start is called before the first frame update
        void Start()
        {
            this.torch = GameObject.FindGameObjectWithTag("torchPos");
            this.spRenderer = this.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.transform.position.x - this.torch.transform.position.x > startDistance)
            {
                rightColor.a = 1;
                this.spRenderer.color = rightColor;
            }
            else if (this.transform.position.x - this.torch.transform.position.x > endDistance)
            {
                rightColor.a = (this.transform.position.x - this.torch.transform.position.x - endDistance) / (startDistance - endDistance);
                this.spRenderer.color = rightColor;
            }
            else if (backDistance <= this.transform.position.x - this.torch.transform.position.x && this.transform.position.x - this.torch.transform.position.x <= endDistance)
            {
                rightColor.a = 0;
                this.spRenderer.color = rightColor;
            }
            else if (this.transform.position.x - this.torch.transform.position.x < backDistance)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

