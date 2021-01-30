using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITest : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(this.OnClickBtn);
        int a = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickBtn()
    {
        Debug.Log("OnClick!!!");
    }
}
