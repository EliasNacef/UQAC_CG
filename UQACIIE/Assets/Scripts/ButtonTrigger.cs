using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonTrigger : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<AudioManager>().Play("ClickButton");
    }

}