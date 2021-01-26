using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class PlayerController : MonoBehaviour
{
    public float Speed = 0.1f;
    private Animation animationCom;
    private Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        this.animationCom = this.GetComponent<Animation>();
        cameraOffset = this.transform.position - Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (this.transform.position.x - Camera.main.transform.position.x > cameraOffset.x - 5)
            {
                this.transform.Translate(-Speed, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.animationCom.Play("jump");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
