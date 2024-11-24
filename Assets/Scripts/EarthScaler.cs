using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthScaler : MonoBehaviour
{
    [SerializeField] OrionController orion;
    void Start()
    {
        float scale = orion.getPositionScale();
        transform.localScale = (transform.localScale * 1000) / scale;
    }
}
