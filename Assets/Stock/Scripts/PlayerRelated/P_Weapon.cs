using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletPool))]
public class P_Weapon : MonoBehaviour
{
    private BulletPool bulletPool;
    [SerializeField] private int bulletPoolCount = 10;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool canShoot;
    [SerializeField] private Transform BulletStartPivot;
    private float currentShootDelay;

    private P_InventoryController p_InventoryController;
    private void Awake()
    {
       
        bulletPool = GetComponent<BulletPool>();
    }
    private void Start()
    {
        p_InventoryController = P_InventoryController.Instance;
        bulletPool.Initialize(bulletPrefab, bulletPoolCount);
    }
    private void Update()
    {
        if (canShoot == false)
        {
            currentShootDelay -= Time.deltaTime;
            if(currentShootDelay<0)
            {
                canShoot = true;
            }
        }

        if(InputController.Instance.Actions.UseWeaponAction.WasPressed)
        {

            Shoot();
        }
    }

    [Button]
    public void Shoot()
    {
        if (p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlotNumber()].itemProperties.itemType != ItemType.weapon)
            return;


        if (canShoot)
        {
            canShoot = false;
            currentShootDelay = 0.5f;
            GameObject bullet = bulletPool.CreateObject();
            bullet.transform.position = BulletStartPivot.position;
            bullet.transform.rotation = BulletStartPivot.rotation;
            bullet.GetComponent<BulletController>().Initialize();


        }
    }
   
}
