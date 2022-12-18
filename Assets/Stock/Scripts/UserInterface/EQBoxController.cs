using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MPUIKIT;

public class EQBoxController : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemAmount;
    [SerializeField] private MPImage frame;
    private UserInterfaceProperties userInterfaceProperties;

    private void Start()
    {
        userInterfaceProperties = ScriptableManager.Instance.userInterfaceProperties;
    }

    public void UpdateItemAmount(int value, Sprite sprite)
    {
        itemAmount.text = value.ToString();
        if (value > 0)
        {
            itemImage.enabled = true;
            itemImage.sprite = sprite;
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
}
