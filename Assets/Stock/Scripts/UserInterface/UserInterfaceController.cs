using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UserInterfaceController : MonoBehaviour
{
    public static UserInterfaceController Instance { get; private set; }

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider oxygenBar;
    [SerializeField] private List<EQBoxController> eQBoxControllers;
    private P_Sounds p_Sounds;
    private int currenteQBoxSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

 
    private void Start()
    {
        eQBoxControllers[0].ActivateBox();
        p_Sounds = P_Sounds.Instance;
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
            if (currenteQBoxSelected == eQBoxControllers.Count-2)
            { 
                currenteQBoxSelected = 0; 
            }
            else
            {
                currenteQBoxSelected++;
            }
            p_Sounds.PlaySound("EQScroll", 0.25f);
            eQBoxControllers[currenteQBoxSelected].ActivateBox();
        }


        if (InputController.Instance.Actions.scrollEQDown.WasPressed)
        {
            eQBoxControllers[currenteQBoxSelected].DeactivateBox();
            if (currenteQBoxSelected == 0)
            {
                currenteQBoxSelected = eQBoxControllers.Count-1;
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
        healthBar.DOValue(value, 0.5f);
    }

    public void UpdateOxygenBar(float value)
    {
        oxygenBar.DOValue(value, 0.5f);
    }
}
