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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
