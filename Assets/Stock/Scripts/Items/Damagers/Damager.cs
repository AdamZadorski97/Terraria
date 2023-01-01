using System;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    [Serializable]
    public class DamagableEvent : UnityEvent<Damagable.DamageMessage, Damagable> { }
   
    public LayerMask hittableLayers;
    
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 throwVector;
    [SerializeField] private float damageAngle;
    [SerializeField] private int damage = 1;
    [SerializeField] private float throwPower;
    [SerializeField] private DamagableEvent OnDamageableHit;
    [SerializeField] private UnityEvent OnNonDamageableHit;
    [SerializeField] private DamagableEvent OnDamageableCantHit;
    [SerializeField] private Vector2 centreOffset = new Vector2(0f, 1f);

    private Transform damageSource;
    protected int hits;
    protected Collider2D[] colliders = new Collider2D[16];
    private void Awake()
    {
     
    }

    private void Start()
    {
        IntializeDamager();
    }
    private void IntializeDamager()
    {
        damageSource = transform;
    }
    public virtual bool Damage()
    {
        hits = Physics2D.OverlapBoxNonAlloc(transform.position, size, damageAngle, colliders, hittableLayers);
#if UNITY_EDITOR
        DebugDrawBox(transform.position, size, damageAngle, Color.red, 0.1f);
#endif
        if (colliders[0] != null)
        {

            for (int i = 0; i < hits; i++)
            {
                Damage(colliders[i]);
            }
            return hits > 0;
        }
        return false;

    }
    public void Damage(Collider2D collider)
    {
        Damagable damageable = collider.GetComponent<Damagable>();
        if (damageable && damageable.enabled)
        {

            Damagable.DamageMessage message = new Damagable.DamageMessage
            {
                damageSource = damageSource.position,
                damager = this,
                amount = damage,
                direction = GetDirection(damageable),
                throwPower = throwPower,
                throwVector = throwVector
            };

            if (damageable.TakeDamage(message))
            {
                if (OnDamageableHit != null)
                {
                    OnDamageableHit.Invoke(message, damageable);
                }
            }
            else
            {
                if (OnDamageableCantHit != null)
                    OnDamageableCantHit.Invoke(message, damageable);
            }
        }
        else
        {
            if (OnNonDamageableHit != null)
                OnNonDamageableHit.Invoke();
        }
    }

    public Vector2 GetDirection(Damagable damagable)
    {
        if (throwVector == new Vector2(0, 0))
        {
            return ((damagable.transform.position + (Vector3)damagable.centreOffset) - damageSource.position).normalized;
        }
        else
        {
            return throwVector;
        }
    }




#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);

    }



    void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color, float duration)
    {

        var orientation = Quaternion.Euler(0, 0, angle);

        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
#endif
}
