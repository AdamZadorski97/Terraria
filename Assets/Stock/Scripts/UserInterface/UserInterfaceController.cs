using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UserInterfaceController : MonoBehaviour
{
    public static UserInterfaceController Instance { get; private set; }

    public List<EQBoxController> eQBoxControllers;
    public List<EQBoxController> eQBoxCraftingControllers;
    public EQBoxController EQBoxReadyRecipieController;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image oxygenBar;
    [SerializeField] private GameObject hurtEffect;

    private P_InventoryController p_InventoryController;
    private P_Sounds p_Sounds;
    private int currenteQBoxSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    IEnumerator Start()
    {
        p_InventoryController = P_InventoryController.Instance;
        p_Sounds = P_Sounds.Instance;
        yield return new WaitForEndOfFrame();
        eQBoxControllers[0].ActivateBox();
    }

    public void Update()
    {
        CheckEQScroll();
    }

    private void CheckEQScroll()
    {
        if (InputController.Instance.Actions.scrollEQUp.WasPressed)
        {
            eQBoxControllers[currenteQBoxSelected].DeactivateBox();
            if (currenteQBoxSelected >= eQBoxControllers.Count - 1)
            {
                currenteQBoxSelected = 0;
            }
            else
            {
                currenteQBoxSelected++;
            }
            p_Sounds.PlaySound("EQScroll", 0.25f);
            Debug.Log("Activate: " + currenteQBoxSelected);
            eQBoxControllers[currenteQBoxSelected].ActivateBox();
            P_InventoryController.Instance.SetupPlayerInventory();
        }

        if (InputController.Instance.Actions.scrollEQDown.WasPressed)
        {
            eQBoxControllers[currenteQBoxSelected].DeactivateBox();
            if (currenteQBoxSelected <= 0)
            {
                currenteQBoxSelected = eQBoxControllers.Count - 1;
            }
            else
            {
                currenteQBoxSelected--;
            }
            p_Sounds.PlaySound("EQScroll", 0.25f);
            eQBoxControllers[currenteQBoxSelected].ActivateBox();
            P_InventoryController.Instance.SetupPlayerInventory();
        }

       
    }

    public void UpdateHealthBar(float value)
    {
        healthBar.DOFillAmount(value, 0.5f);
    }

    public void UpdateOxygenBar(float value)
    {
        oxygenBar.DOFillAmount(value, 0.5f);
    }

    public int GetCurrentSlotNumber()
    {
        return currenteQBoxSelected;
    }

    public void HurtEffect()
    {
        StartCoroutine(HurtEffectCoroutine());
    }
    IEnumerator HurtEffectCoroutine()
    {
        hurtEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hurtEffect.SetActive(false);
    }
}
