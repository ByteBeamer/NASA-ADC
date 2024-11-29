using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySatellites : MonoBehaviour
{
    [SerializeField] OrionController orion;
    TMPro.TextMeshProUGUI textObj;
    void Start()
    {
        textObj = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        Dictionary<string, bool> available = orion.getCurrentAntennas();
        string[] strings = new string[4];
        strings[0] = "<color=" + (available["WPSA"] ? "#005500" : "\"red\"") + ">WPSA is " + (available["WPSA"] ? "available" : "unavailable") + "<br>";
        strings[1] = "<color=" + (available["DS54"] ? "#005500" : "\"red\"") + ">DS54 is " + (available["DS54"] ? "available" : "unavailable") + "<br>";
        strings[2] = "<color=" + (available["DS24"] ? "#005500" : "\"red\"") + ">DS24 is " + (available["DS24"] ? "available" : "unavailable") + "<br>";
        strings[3] = "<color=" + (available["DS34"] ? "#005500" : "\"red\"") + ">DS34 is " + (available["DS34"] ? "available" : "unavailable") + "<br>";
        textObj.text = strings[0] + strings[1] + strings[2] + strings[3];
    }
}
