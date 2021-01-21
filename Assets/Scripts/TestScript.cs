using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScript : MonoBehaviour
{
    public Text FirstText;

    public TMP_Text text1;
    public GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        FirstText.text = "Welocome Home!";
        text1.text = "what>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
