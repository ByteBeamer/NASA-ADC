using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrionAnimator : MonoBehaviour
{
    [SerializeField] Animator solarPanelA, solarPanelB, solarPanelC, solarPanelD;
    [SerializeField] Animator middlePart, bottomPart;
    [SerializeField] GameObject middleFire;
    public void OpenSolarPanels()
    {
        solarPanelA.SetTrigger("Start");
        solarPanelB.SetTrigger("Start");
        solarPanelC.SetTrigger("Start");
        solarPanelD.SetTrigger("Start");
    }

    public IEnumerator DetachBottom()
    {
        bottomPart.SetTrigger("Start");

        yield return new WaitForSeconds(2);
        bottomPart.transform.SetParent(null, true);
        bottomPart.GetComponent<Animator>().enabled = false;

        middleFire.SetActive(true);
    }

    public IEnumerator DetachMiddle()
    {
        middlePart.SetTrigger("Start");

        yield return new WaitForSeconds(3);
        middlePart.transform.SetParent(null, true);
        middlePart.GetComponent<Animator>().enabled = false;
    }
}
