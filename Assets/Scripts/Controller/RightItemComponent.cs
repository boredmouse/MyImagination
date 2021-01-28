using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WHGame
{
    public class RightItemComponent : MonoBehaviour
    {
        private GameObject torch;

        private float startDistance = 10;
        private float endDistance = 4;
        private float backDistance = -1;
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
            }
            else if (this.transform.position.x - this.torch.transform.position.x < backDistance)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

