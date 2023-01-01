using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MPUIKIT;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EQBoxController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemAmount;
    [SerializeField] private MPImage frame;
    private UserInterfaceProperties userInterfaceProperties;
    [SerializeField] UnityEvent OnRightClick;
    private void Start()
    {
        userInterfaceProperties = ScriptableManager.Instance.userInterfaceProperties;
    }

    public void UpdateItemAmount(int value, ItemProperties itemProperties)
    {
        if(itemProperties.itemType == ItemType.none)
        {
            itemAmount.text = "0";
            itemImage.enabled = false;
            return;
        }

        itemAmount.text = value.ToString();
        if (value > 0)
        {
            itemImage.enabled = true;
            itemImage.sprite = itemProperties.sprite;
        }
        else
        {
            itemImage.enabled = false;
        }
    }

    public void ActivateBox()
    {
        frame.color = userInterfaceProperties.EQBoxActive;
    }

    public void DeactivateBox()
    {
        frame.color = userInterfaceProperties.EQBoxDeactive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick.Invoke();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick.Invoke();
        }
    }


}
