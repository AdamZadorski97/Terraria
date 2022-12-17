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

    public void UpdateItemAmount(int value)
    {
        itemAmount.text = value.ToString();
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
