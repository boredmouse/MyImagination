using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject right;
    public GameObject left;
    //手电筒
    public GameObject torch;

    private float startDistance = 10;
    private float endDistance = 4;
    private float backDistance = -1;
    private SpriteRenderer renderer;
    private Color rightColor = Color.white;

    private bool dangerous = true;
    // Start is called before the first frame update
    void Start()
    {
        this.torch = GameObject.FindGameObjectWithTag("torchPos");
        this.renderer = this.right.GetComponent<SpriteRenderer>();
        this.left.SetActive(false);
        this.right.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x - this.torch.transform.position.x > startDistance)
        {
            rightColor.a = 1;
            this.renderer.color = rightColor;
        }
        else if (this.transform.position.x - this.torch.transform.position.x > endDistance)
        {
            rightColor.a = (this.transform.position.x - this.torch.transform.position.x - endDistance) / (startDistance - endDistance);
            this.renderer.color = rightColor;
        }
        else if (backDistance <= this.transform.position.x - this.torch.transform.position.x && this.transform.position.x - this.torch.transform.position.x <= endDistance)
        {
            //rightColor.a = 0;
            //this.renderer.color = rightColor;
            if (!dangerous)
            {
                this.left.SetActive(false);
            }
        }
        else if (this.transform.position.x - this.torch.transform.position.x < backDistance)
        {
            if (!left.activeSelf)
            {
                this.dangerous = false;
                this.left.SetActive(true);
                this.right.SetActive(false);
            }

        }
    }
}
