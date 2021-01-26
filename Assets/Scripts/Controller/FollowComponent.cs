using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private Vector3 newPos;
    private Vector3 offset;
    void Start()
    {
        player = GameObject.Find("player");
        offset = player.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if ((player.transform.position.x - this.transform.position.x) - offset.x > 0.1f)
        {
            newPos = this.transform.position;
            newPos.x = this.player.transform.position.x - offset.x;
            this.transform.position = Vector3.Lerp(newPos, this.transform.position, 0.1f);
        }
    }
}
