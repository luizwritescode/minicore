using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    //References to GunRaycastController.cs  Script
    GameObject Gun;
    GunRaycastController grc;
    public int _maxAmmo;
    public int _ammo; 

    bool _isReloading;
    int _currentAmmoShownInUI;

    public Transform ammoBar;
    private Vector3 ammoBarSize = new Vector3(5, 2, 1);
    private Vector3 ammoBarPosition = new Vector3(1624,50,0);

    public bool isReloading;
    public float reloadTime;

    void Start()
    {
        Gun = GameObject.Find("Gun");
        grc = Gun.GetComponent<GunRaycastController>();

        _isReloading = grc.isReloading;
        _maxAmmo = grc.maxAmmo;
        _ammo = grc.ammo;
        _currentAmmoShownInUI = _ammo;
    }

    void Update()
    {
        grc = Gun.GetComponent<GunRaycastController>();
        _ammo = grc.ammo;

        if( _ammo < _currentAmmoShownInUI && _currentAmmoShownInUI > 0)
        {
            // subtract size of the ammoBar
            ammoBar.localScale += new Vector3(-0.05f ,  0f, 0f );
            ammoBar.position += new Vector3(-2.5f, 0f, 0f);
            _currentAmmoShownInUI--;

        }
        
    }
}
