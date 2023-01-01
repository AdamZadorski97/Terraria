using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public Vector2 centreOffset = new Vector2(0f, 0f);
    [Serializable]  public class DamageEvent : UnityEvent<DamageMessage, Damagable> { }

    [SerializeField] private DamageEvent OnTakeDamage;

    [HideInInspector] public DamageMessage message;

    public bool TakeDamage(DamageMessage damageMessage)
    {
        message = damageMessage;
        OnTakeDamage.Invoke(message, this);
        return false;
    }

    public struct DamageMessage
    {
        public MonoBehaviour damager;
        public int amount;
        public float throwPower;
        public Vector2 direction;
        public Vector3 damageSource;
        public Vector2 throwVector;
    }
}
