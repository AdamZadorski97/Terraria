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
            if (currenteQBoxSelected >= eQBoxControllers.Count - 2)
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
        }

        if (InputController.Instance.Actions.scrollEQDown.WasPressed)
        {
            eQBoxControllers[currenteQBoxSelected].DeactivateBox();
            if (currenteQBoxSelected <= 0)
            {
                currenteQBoxSelected = eQBoxControllers.Count - 2;
            }
            else
            {
                currenteQBoxSelected--;
            }
            p_Sounds.PlaySound("EQScroll", 0.25f);
            eQBoxControllers[currenteQBoxSelected].ActivateBox();
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

    public int GetCurrentSlot()
    {
        return currenteQBoxSelected;
    }
}
