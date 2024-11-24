using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelDragger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject panel;
    public RectTransform menuCanvas;
    public RectTransform panelTransform;
    public RectTransform draggerTransform;
    float screenWidth;
    float screenHeight;

    private void Start()
    {
        screenWidth = menuCanvas.rect.width * menuCanvas.transform.localScale.x;
        screenHeight = menuCanvas.rect.height * menuCanvas.transform.localScale.y;
    }

    void Update()
    {
        panelTransform.sizeDelta = new Vector2(panelTransform.sizeDelta.x, Screen.height);
        draggerTransform.sizeDelta = new Vector2(draggerTransform.sizeDelta.x, Screen.height);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, screenWidth-100), transform.position.y, transform.position.z);
        //panel.transform.position = new Vector3(Input.mousePosition.x, panel.transform.position.y, panel.transform.position.z);
        panelTransform.sizeDelta = new Vector2((float)screenWidth - transform.position.x, Screen.height);
        draggerTransform.sizeDelta = new Vector2(draggerTransform.sizeDelta.x, Screen.height);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
