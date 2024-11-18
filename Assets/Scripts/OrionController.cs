using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class OrionController : MonoBehaviour
{
    protected float counter = 0;
    protected int timePassed = 0;
    protected string[][] data;
    public int scale = 10000;
    public string filePath = @"D:\Documents\UnityProjects\NASA ADC\Assets\PathData.csv";

    public float fixedSize = .001f;
    public Camera camera;

    void Start()
    {
        data = System.IO.File.ReadLines(filePath).Select(x => x.Split(',')).ToArray();
    }

    // Update is called once per frame
    void Update()
    {

        fixedSize += UnityEngine.Input.mouseScrollDelta.y / 10000;

        if (fixedSize < .0001f) fixedSize = .0001f;

        counter += Time.deltaTime;

        if (counter >= 5)
        {
            timePassed += 1;
            //RESET Counter
            counter = 0;
        }

        if (!(timePassed < data[1].Length)) return;
        //these indexes are for velocity (pretty sure)
        float x = float.Parse(data[1][timePassed], CultureInfo.InvariantCulture) / scale, y = float.Parse(data[2][timePassed], CultureInfo.InvariantCulture) / scale, z = float.Parse(data[3][timePassed], CultureInfo.InvariantCulture) / scale;
        float xNext = float.Parse(data[1][timePassed + 1], CultureInfo.InvariantCulture) / scale, yNext = float.Parse(data[2][timePassed + 1], CultureInfo.InvariantCulture) / scale, zNext = float.Parse(data[3][timePassed + 1], CultureInfo.InvariantCulture) / scale;
        transform.position += new Vector3(x, y, z);
        
        //Rotate along path
        transform.LookAt(new Vector3(xNext, yNext, zNext));
        transform.Rotate(-90, 0, 0, Space.Self);
        //Debug.Log(data[1][timePassed] + ":" + data[2][timePassed] + ":" + data[3][timePassed]);


        var distance = (camera.transform.position - transform.position).magnitude;
        var size = distance * fixedSize * camera.fieldOfView;
        transform.localScale = Vector3.one * size;
    }
}
