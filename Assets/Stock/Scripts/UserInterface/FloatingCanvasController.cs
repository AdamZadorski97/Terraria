using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FloatingCanvasController : MonoBehaviour
{
    public static FloatingCanvasController Instance { get; private set; }

    [SerializeField] private TMP_Text textMessage;
    [SerializeField] private GameObject floatingCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void TurnOnCanvas(string message, Vector3 messagePosition)
    {
        floatingCanvas.SetActive(true);
        floatingCanvas.transform.position = messagePosition;
        textMessage.text = message;
    }

    public void TurnOffCanvas()
    {
        floatingCanvas.SetActive(false);
    }

}
