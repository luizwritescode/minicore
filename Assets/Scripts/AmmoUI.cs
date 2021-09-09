using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmmoUI : MonoBehaviour
{
    //References to GunRaycastController.cs  Script
    GameObject Gun;
    GunRaycastController grc;
    public int _maxAmmo;
    public int _ammo; 

    
    bool _isReloading;
    int _currentAmmoShownInUI;

    public Slider ammoBarSlider;
    public GameObject ammoBar;
    private Vector3 ammoBarSize = new Vector3(5, 2, 1);
    private Vector3 ammoBarPosition = new Vector3(1624,50,0);

    public bool isReloading;
    public float reloadTime;

    void Start()
    {
        Gun = GameObject.Find("Gun");
        grc = Gun.GetComponent<GunRaycastController>();

        ammoBarSlider = ammoBar.GetComponent<Slider>();

        Debug.Log(ammoBarSlider);

        _isReloading = grc.isReloading;
        _maxAmmo = grc.maxAmmo;
        _currentAmmoShownInUI = grc.maxAmmo;
        
    }

    //HIT POINTS/ HIT POINTS

    //SLIDER.Value = hit points / max hit points 
    //

    void Update()
    {
        //refresh reference is this needed?
        grc = Gun.GetComponent<GunRaycastController>();

        if(_currentAmmoShownInUI != grc.ammo)
        {   
            // subtract size of the ammoBar
            _currentAmmoShownInUI = grc.ammo;
           ammoBarSlider.value = ((float)_currentAmmoShownInUI / (float)_maxAmmo);
            Debug.Log(ammoBarSlider.value);

        }
        
    }
}
