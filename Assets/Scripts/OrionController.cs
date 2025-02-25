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
    protected string[][] data;
    public int timePassed = 1;
    public int positionScale = 100;
    public float timeScale = 10;
    public int pointFrequency = 1;
    public string filePath = @"D:\Documents\UnityProjects\NASA ADC\Assets\PathData.csv";
    public Camera mainCamera;
    OrionAnimator orionAnimator;
    public GameObject pathParent;

    bool openedPanels = false, startedLanding = false, detachedBottom = false;

    [SerializeField] private AnimationCurve easingCurve;

    public GameObject pointPrefab;

    void Start()
    {
        data = System.IO.File.ReadLines(filePath).Select(x => x.Split(',')).ToArray();
        transform.position = getPosition(timePassed);
        orionAnimator = GetComponent<OrionAnimator>();
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
        TimeSpan currentTime;
        try { 
            currentTime = TimeSpan.FromMinutes(float.Parse(data[timePassed][0], CultureInfo.InvariantCulture));
        } catch
        {
            currentTime = TimeSpan.FromMinutes(float.Parse(data[data.Length - 1][0], CultureInfo.InvariantCulture));
            if (!startedLanding) StartCoroutine(orionAnimator.DetachMiddle()); startedLanding = true;
        }

        if (counter >= currentTime.TotalSeconds)
        {
            timePassed += 1;
            if (pointFrequency != 0 && timePassed % pointFrequency == 0) {
                GameObject point = Instantiate(pointPrefab, pathParent.transform);
                PathScript pathScript = point.GetComponent<PathScript>();
                pathScript.setPoints(getPosition(timePassed - pointFrequency), getPosition(timePassed));
                if (getCurrentTime().TotalMinutes >= 1492.277)
                {
                    //pathScript.setColor(new Color(24, 167, 235) / 255);
                } else if (getCurrentTime().TotalMinutes >= 196.6495)
                {
                    if (!detachedBottom) StartCoroutine(orionAnimator.DetachBottom()); detachedBottom = true;
                    //pathScript.setColor(new Color(138, 189, 62) / 255);
                } else if (getCurrentTime().TotalMinutes >= 118.0945)
                {
                    if (!openedPanels) orionAnimator.OpenSolarPanels(); openedPanels = true;
                    //pathScript.setColor(new Color(254, 146, 11) / 255);
                }
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
        return TimeSpan.FromMinutes(float.Parse(data[timePassed][0], CultureInfo.InvariantCulture));
    }

    public float getPositionScale()
    {
        return positionScale;
    }

    public Vector3 getTruePosition()
    {
        return new Vector3(float.Parse(data[timePassed][1], CultureInfo.InvariantCulture), float.Parse(data[timePassed][2], CultureInfo.InvariantCulture), float.Parse(data[timePassed][3], CultureInfo.InvariantCulture));
    }

    public Dictionary<string, bool> getCurrentAntennas()
    {
        var opts = new Dictionary<string, bool>();
        opts.Add("WPSA", (data[timePassed][8] == "1") ? true : false);
        opts.Add("DS54", (data[timePassed][10] == "1") ? true : false);
        opts.Add("DS24", (data[timePassed][12] == "1") ? true : false);
        opts.Add("DS34", (data[timePassed][14] == "1") ? true : false);
        return opts;
    }
}
