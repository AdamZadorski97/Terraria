using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCanvasController : MonoBehaviour
{
    public static FloatingCanvasController Instance { get; private set; }
   
    public GameObject floatingCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void TurnOnCanvas()
    {
        floatingCanvas.SetActive(true);
    }

    public void TurnOffCanvas()
    {
        floatingCanvas.SetActive(false);
    }

}
