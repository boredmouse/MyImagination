using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftItemComponent : MonoBehaviour
{
    SpriteRenderer spRenderer;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        spRenderer = this.GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        spRenderer.material.SetFloat("_SetX",player.transform.position.x-1);
    }
}
