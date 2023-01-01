using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private bool useGravity;
    [SerializeField] private float bulletDestroyTime = 2;
    [SerializeField] private float bulletVelocity;

    [SerializeField] private LayerMask contactLayer;

    private float currentBulletTime;
    private Vector2 direction;
    private Rigidbody2D rb;
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        currentBulletTime = 0;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (Vector2)((worldMousePos - transform.position));
        direction.Normalize();
        if(useGravity)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = direction * bulletVelocity;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
 
    }

    private void Update()
    {
        currentBulletTime += Time.deltaTime;
        if(currentBulletTime>bulletDestroyTime)
        {
            gameObject.SetActive(false);
        }

        if(useGravity == false)
        {
            transform.position += (Vector3)direction * Time.deltaTime * bulletVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((contactLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            gameObject.SetActive(false);
        }
    }
}
