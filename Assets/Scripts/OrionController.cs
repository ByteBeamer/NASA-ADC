using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static UnityEditor.PlayerSettings;

public class OrionController : MonoBehaviour
{
    protected float counter = 0;
    protected float lastFrame = 0;
    protected int timePassed = 1;
    protected string[][] data;
    public int positionScale = 100;
    public float timeScale = 10;
    public int pointFrequency = 1;
    public string filePath = @"D:\Documents\UnityProjects\NASA ADC\Assets\PathData.csv";
    //public float fixedSize = .001f;
    public Camera mainCamera;

    [SerializeField] private AnimationCurve easingCurve;

    public GameObject pointPrefab;

    void Start()
    {
        data = System.IO.File.ReadLines(filePath).Select(x => x.Split(',')).ToArray();
        transform.position = getPosition(timePassed);
    }

    void Update()
    {
        counter += Time.deltaTime * timeScale;
        TimeSpan lastTime;
        try
        {
            lastTime = TimeSpan.FromMinutes(float.Parse(data[timePassed - 1][0], CultureInfo.InvariantCulture));
        } catch {
            lastTime = TimeSpan.Zero;
        }
        TimeSpan currentTime = TimeSpan.FromMinutes(float.Parse(data[timePassed][0], CultureInfo.InvariantCulture));

        if (counter >= currentTime.TotalSeconds)
        {
            timePassed += 1;
            if (pointFrequency != 0 && timePassed % pointFrequency == 0) { 
                Vector3 pointPosition = getPosition(timePassed);
                GameObject point = GameObject.Instantiate(pointPrefab);
                point.transform.position = pointPosition;
                point.transform.localScale = Vector3.one * 100;
            }
        }
        Vector3 position = getPosition(timePassed), nextPosition = getPosition(timePassed + 1), lastPosition = getPosition(timePassed - 1);

        float factor = easingCurve.Evaluate(remap(counter, Convert.ToSingle(lastTime.TotalSeconds), Convert.ToSingle(currentTime.TotalSeconds), 0, 1));
        //Debug.Log(counter +  " " + Convert.ToSingle(lastTime.TotalSeconds) + " " + Convert.ToSingle(currentTime.TotalSeconds));
        //Debug.Log(factor);
        transform.position = Vector3.Lerp(position, nextPosition, factor);
        //Rotate along path
        transform.LookAt(Vector3.Lerp(nextPosition, getPosition(timePassed + 2), factor));
        transform.Rotate(90, 0, 0, Space.Self);
    }

    public Vector3 getPosition(int time)
    {
       return new Vector3(float.Parse(data[time][1], CultureInfo.InvariantCulture) / positionScale, float.Parse(data[time][2], CultureInfo.InvariantCulture) / positionScale, float.Parse(data[time][3], CultureInfo.InvariantCulture) / positionScale);
    }

    public float remap(float x, float oMin, float oMax, float nMin, float nMax) { 
        if (oMin == oMax) {
            return 0;
        }

        if (nMin == nMax){
            return 0;
        }

        bool reverseInput = false;
        float oldMin = Math.Min(oMin, oMax);
        float oldMax = Math.Max(oMin, oMax);
        if (oldMin != oMin) {
            reverseInput = true;
        }

        bool reverseOutput = false;
        float newMin = Math.Min(nMin, nMax);
        float newMax = Math.Max(nMin, nMax);
        if (newMin != nMin) { 
            reverseOutput = true;
        }

        float portion = (x - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        if (reverseInput) {
            portion = (oldMax - x) * (newMax - newMin) / (oldMax - oldMin);
        }
        float result = portion + newMin;
        if (reverseOutput) { 
            result = newMax - portion;
        }
        return result;
    }

    public void setTimeScale(string scale)
    {
        timeScale = float.Parse(scale);
    }

    public float getTimeScale()
    {
        return timeScale;
    }

    public TimeSpan getCurrentTime()
    {
        return TimeSpan.FromMinutes(float.Parse(data[0][timePassed], CultureInfo.InvariantCulture));
    }

    public float getPositionScale()
    {
        return positionScale;
    }

    public Vector3 getTruePosition()
    {
        return new Vector3(float.Parse(data[timePassed][1], CultureInfo.InvariantCulture), float.Parse(data[timePassed][2], CultureInfo.InvariantCulture), float.Parse(data[timePassed][3], CultureInfo.InvariantCulture));
    }
}
