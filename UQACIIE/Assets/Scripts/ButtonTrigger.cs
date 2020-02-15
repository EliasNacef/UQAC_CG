using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Classe qui va permettre de jouer les sons des boutons au bon moment
/// </summary>
public class ButtonTrigger : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<AudioManager>().Play("ClickButton");
    }

}