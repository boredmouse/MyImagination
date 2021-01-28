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
    public Transform modelParent;
    private Animation animationCom;

    private GameObject aseModel;
    private Animator modelAnimator;
    private Vector3 cameraOffset;


    private PalyerState state = PalyerState.Idle;

    private Dictionary<CommonEnum.PartType, string> bodyParts = new Dictionary<CommonEnum.PartType, string>(5);
    // Start is called before the first frame update
    void Start()
    {
        this.animationCom = this.GetComponent<Animation>();
        this.InitBodyParts();
        this.LoadHeroModel("neinei/pants");
        cameraOffset = this.transform.position - Camera.main.transform.position;
        this.AddEventListener();

        AchievementManager.GetAchievement("001");
    }

    void InitBodyParts()
    {
        this.bodyParts[CommonEnum.PartType.body] = "000";
        this.bodyParts[CommonEnum.PartType.coat] = "000";
        this.bodyParts[CommonEnum.PartType.hat] = "000";
        this.bodyParts[CommonEnum.PartType.leg] = "000";
        this.bodyParts[CommonEnum.PartType.shooes] = "000";
    }

    void AddEventListener()
    {
        BattleManager.OnGetClothEvent += this.OnGetCloth;
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
        //Debug.Log(collision.gameObject.name);
    }

    #region 状态与动画
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
            StartCoroutine(ExitStateCor(state, time));
        }
    }

    IEnumerator ExitStateCor(PalyerState state, float time)
    {
        yield return new WaitForSeconds(time);
        if (state == PalyerState.Jump)
        {
            if (Input.GetKey(KeyCode.D))
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
        //Debug.Log("state:" + state);

        if (state == PalyerState.Idle)
        {
            this.animationCom.Play("idle");
            if (this.modelAnimator != null)
            {
                this.modelAnimator.Play("idle");
            }
        }
        else if (state == PalyerState.Walk)
        {
            this.animationCom.Play("walk");
            if (this.modelAnimator != null)
            {
                this.modelAnimator.Play("walk");
            }
        }
        else if (state == PalyerState.Jump)
        {
            this.animationCom.Play("jump");
            if (this.modelAnimator!=null)
            {
            this.modelAnimator.Play("jump");
            }
        }
    }
    #endregion


    #region 资源
    void OnGetCloth(string id, CommonEnum.PartType part)
    {
        this.bodyParts[part] = id;
        this.LoadHeroModel(this.GetModelPathByBodyParts());
    }

    string GetModelPathByBodyParts()
    {
        string partIdStr = this.bodyParts[CommonEnum.PartType.body]
        + this.bodyParts[CommonEnum.PartType.coat]
        + this.bodyParts[CommonEnum.PartType.hat]
        + this.bodyParts[CommonEnum.PartType.leg]
        + this.bodyParts[CommonEnum.PartType.shooes]
        ;
        var tableItem = ModelResourceConfig.GetConfigByID(partIdStr);
        if(tableItem == null)
        {
            Debug.LogError("ModelResourceConfig找不到id："+partIdStr);
        }
        var path =tableItem.Path;
        return path;
    }
    void LoadHeroModel(string path)
    {
        if (this.aseModel != null)
        {
            Destroy(this.aseModel, 0);
        }
        string finalPath = "Models/player/ase/" + path;
        //Debug.Log("加载资源路径："+finalPath);
        var prefab = Resources.Load(finalPath, typeof(GameObject)) as GameObject;
        GameObject go = Object.Instantiate(prefab, this.modelParent) as GameObject;
        this.modelAnimator = go.GetComponent<Animator>();
        this.aseModel = go;
    }
    #endregion
}

/*public class BodyPart{
    public int id = 0;
    public CommonEnum.PartType part = CommonEnum.PartType.body;
    
}*/

