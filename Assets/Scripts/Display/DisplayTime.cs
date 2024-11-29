using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTime : MonoBehaviour
{
    [SerializeField] OrionController orion;
    TMPro.TextMeshProUGUI textObj;
    void Start()
    {
        textObj = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        TimeSpan time = orion.getCurrentTime();
        textObj.text = time.ToString();
    }
}
