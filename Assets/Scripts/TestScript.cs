using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScript : MonoBehaviour
{
    public Text NameLab;

    public TMP_Text ContentLab;
    // Start is called before the first frame update
    void Start()
    {
        NameLab.text = "Hades";
        ContentLab.text = "Welcome Home!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
