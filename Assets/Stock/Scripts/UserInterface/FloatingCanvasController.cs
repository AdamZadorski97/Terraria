using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingCanvasController : MonoBehaviour
{
    public static FloatingCanvasController Instance { get; private set; }

    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text textMessage;
    [SerializeField] private GameObject floatingMessageCanvas;

    [SerializeField] private GameObject floatingTempPickCanvas;
    [SerializeField] private Image TempPickImage;
    [SerializeField] private TMP_Text TempPickAmount;
    [SerializeField] Vector2 floatingTempPickCanvasOffset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        floatingTempPickCanvas.transform.position = canvas.transform.TransformPoint(pos + floatingTempPickCanvasOffset);
        if(TempPickAmount.text == "" || TempPickAmount.text == "0")
        {
            HideTempPickVizualisation();
        }
    }

    public void TurnOnCanvas(string message, Vector3 messagePosition)
    {
        floatingMessageCanvas.SetActive(true);
        floatingMessageCanvas.transform.position = messagePosition;
        textMessage.text = message;
    }

    public void TurnOffCanvas()
    {
        floatingMessageCanvas.SetActive(false);
    }

    public void ShowTempPickVizualisation(Sprite sprite, int amount)
    {
        floatingTempPickCanvas.SetActive(true);
        TempPickImage.sprite = sprite;
        TempPickAmount.text = amount.ToString();
    }

    public void HideTempPickVizualisation()
    {
        floatingTempPickCanvas.SetActive(false);
        TempPickImage.enabled = true;
    }

}
