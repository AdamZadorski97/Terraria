using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Stats : MonoBehaviour
{
    private float m_health = 1;
    private float m_oxygen = 1;

    private float oxygenLoseFrequency;
    private float oxygenLoseValue;
    private float oxygenLosetime;

    private float healthLoseFrequency;
    private float healthLoseValue;
    private float healthLosetime;

    private bool isDeath;

    private PlayerProperties playerProperties;
    private void Start()
    {
        playerProperties = ScriptableManager.Instance.playerProperties;
        oxygenLoseFrequency = playerProperties.oxygenLoseFrequency;
        oxygenLoseValue = playerProperties.oxygenLoseValue;

        healthLoseFrequency = playerProperties.healthLoseFrequency;
        healthLoseValue = playerProperties.healthLoseValue;
    }


    public void Update()
    {
        if(!isDeath)
        LoseOxygen();
    }

    private void LoseOxygen()
    {
        if (isDeath)
            return;
        if (Oxygen <= 0)
        {
            OnOxygenEmpty();
            return;
        }
        
       
        oxygenLosetime += Time.deltaTime;
        if (oxygenLosetime > oxygenLoseFrequency)
        {
            oxygenLosetime = 0;
            Oxygen -= oxygenLoseValue;

            if (Oxygen <= 0)
            {

            }
        }
    }

    private void OnOxygenEmpty()
    {
        healthLosetime += Time.deltaTime;
        if (healthLosetime > healthLoseFrequency)
        {
            healthLosetime = 0;
            Health -= healthLoseValue;
        }
    }

    private void LoseHealth()
    {
        if (isDeath)
            return;
        if (m_health <= 0)
        {
        
        }
    }



    public void Death()
    {
        isDeath = true;
    }



    public float Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            UserInterfaceController.Instance.UpdateHealthBar(value);
            if (value <=0)
            {
                Death();
            }
        }
    }

    public float Oxygen
    {
        get { return m_oxygen; }
        set
        {
            m_oxygen = value;
            UserInterfaceController.Instance.UpdateOxygenBar(value);
        }
    }
}
