using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using WHGame;



public class PlayerController : MonoBehaviour
{
    public enum PalyerState
    {
        Idle = 0,
        Walk = 1,
        Jump = 2,
        Down = 3
    };
    public float Speed = 0.1f;
    private Animation animationCom;
    private Animator modelAnimator;
    private Vector3 cameraOffset;

    private PalyerState state = PalyerState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        this.animationCom = this.GetComponent<Animation>();
        this.modelAnimator = this.GetComponentInChildren<Animator>();
        cameraOffset = this.transform.position - Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Speed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.A) && this.state != PalyerState.Jump)
        {
            this.SetState(PalyerState.Walk);
        }
        if (Input.GetKeyUp(KeyCode.D) && !Input.GetKey(KeyCode.A) && this.state == PalyerState.Walk)
        {
            this.SetState(PalyerState.Idle);
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (this.transform.position.x - Camera.main.transform.position.x > cameraOffset.x - 2)
            {
                this.transform.Translate(-Speed, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && this.state != PalyerState.Down)
        {
            this.SetState(PalyerState.Jump, 0.6f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    //time 退出状态时间
    void SetState(PalyerState state, float time = 0)
    {
        this.SetStateAndAnim(state);
        if (time <= 0)
        {
            return;
        }
        else
        {
            StartCoroutine(ExitStateCor(state,time));
        }
    }

    IEnumerator ExitStateCor(PalyerState state, float time)
    {
        yield return new WaitForSeconds(time);
        if (state == PalyerState.Jump)
        {
            if(Input.GetKey(KeyCode.D))
            {
                this.SetStateAndAnim(PalyerState.Walk);
            }
            else
            {
                this.SetStateAndAnim(PalyerState.Idle);
            }
        }
    }

    void SetStateAndAnim(PalyerState state)
    {
        this.state = state;
        Debug.Log("state:" + state);

        if (state == PalyerState.Idle)
        {
            //this.animationCom.Play("idle");
            this.modelAnimator.Play("idle");
        }
        else if (state == PalyerState.Walk)
        {
            //this.animationCom.Play("walk");
            this.modelAnimator.Play("walk");
        }
        else if (state == PalyerState.Jump)
        {
            this.animationCom.Play("jump");
            //this.modelAnimator.Play("jump");
        }
    }
}

