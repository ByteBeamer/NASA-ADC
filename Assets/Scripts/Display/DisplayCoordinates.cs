using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCoordinates : MonoBehaviour
{
    [SerializeField] OrionController orion;
    TMPro.TextMeshProUGUI textObj;
    void Start()
    {
        textObj = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        Vector3 position = orion.getTruePosition();
        textObj.text = "X : " + position.x.ToString() + "<br>" + "Y : " + position.y.ToString() + "<br>" + "Z : " + position.z.ToString();
    }
}
