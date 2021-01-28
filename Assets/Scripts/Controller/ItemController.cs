using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WHGame;

public class ItemController : MonoBehaviour
{
    public enum ItemType
    {
        normal = 0,
        cloth = 1,
        pat =2
    };
    

    public string ID = "00";
    public GameObject right;
    public GameObject left;
    public ItemType itemType = ItemType.normal;
    public CommonEnum.PartType part = CommonEnum.PartType.body;

    //手电筒
    private GameObject torch;

    private float startDistance = GameConfig.startDistance;
    private float endDistance = GameConfig.endDistance;
    private float backDistance = GameConfig.backDistance;

    private bool dangerous = true;
    // Start is called before the first frame update
    void Start()
    {
        this.torch = GameObject.FindGameObjectWithTag("torchPos");
        this.left.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x - this.torch.transform.position.x > startDistance)
        {
        }
        else if (this.transform.position.x - this.torch.transform.position.x > endDistance)
        {
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
            if (this.dangerous)
            {
                this.dangerous = false;
                if (this.itemType == ItemType.cloth)
                {
                    //派发获取衣物事件
                    BattleManager.OnGetClothEvent(this.ID,this.part);
                }
            }
            if (!left.activeSelf)
            {
                this.left.SetActive(true);
            }

        }
    }
}
