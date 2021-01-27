using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WHGame;

public class ItemController : MonoBehaviour
{
    public enum ItemType {
        normal = 0,
        cloth = 1
    };

    public int ID = 0;
    public GameObject right;
    public GameObject left;
    public ItemType itemType = ItemType.normal;

    //手电筒
    private GameObject torch;

    private float startDistance = 10;
    private float endDistance = 4;
    private float backDistance = -1;
    private SpriteRenderer spRenderer;
    private Color rightColor = Color.white;

    private bool dangerous = true;
    // Start is called before the first frame update
    void Start()
    {
        this.torch = GameObject.FindGameObjectWithTag("torchPos");
        this.spRenderer = this.right.GetComponent<SpriteRenderer>();
        this.left.SetActive(false);
        this.right.SetActive(true);
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
            if (!dangerous)
            {
                this.left.SetActive(false);
            }
        }
        else if (this.transform.position.x - this.torch.transform.position.x < backDistance)
        {
            if(this.dangerous)
            {
                this.dangerous = false;
                if (this.itemType == ItemType.cloth)
                {
                    BattleManager.OnGetClothEvent(this.ID);
                }
            }
            if (!left.activeSelf)
            {
                this.left.SetActive(true);
                this.right.SetActive(false);
            }

        }
    }
}
