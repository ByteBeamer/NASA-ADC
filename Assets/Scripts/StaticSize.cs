using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSize : MonoBehaviour
{

    void Update()
    {
        var distance = (Camera.main.transform.position - transform.position).magnitude;
        var size = distance * 0.001f * Camera.main.fieldOfView;
        transform.localScale = Vector3.one * size;
    }
}
