using UnityEngine;

public class TriggerContactDamager : Damager
{
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!enabled || (hittableLayers.value & 1 << collider.gameObject.layer) == 0)
            return;

        Debug.Log("Damage");
        Damage(collider);
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!enabled || (hittableLayers.value & 1 << collider.gameObject.layer) == 0)
            return;

        Damage(collider);
    }
}