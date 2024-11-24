using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSpeedDisplay : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        float position = cameraController.getMoveSpeed();
        text.text = position.ToString();
    }
}
