using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootableEntity))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyRaycastController : MonoBehaviour
{
    public EnemyAI ai;

    public int gunDamage = 1;                                            
    public float fireRate = 0.25f;                                        
    public float weaponRange = 50f;                                      
    public float hitForce = 100f;                                       
    public Transform gunEnd;                                          
    public WaitForSeconds shotDuration = new WaitForSeconds(0.7f);

    public LineRenderer laserLine;
    public float nextFire;

    public ParticleSystem muzzleFlash;

    public ParticleSystem arrivalDust;

    public int maxAmmo;
    public int ammo;

    public bool isReloading;
    public float reloadTime;

    public bool AIpressedShoot; 

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isReloading = false;
        muzzleFlash.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        AIpressedShoot = ai.IsAttacking();

        if (ammo <= 0 )
        {
            StartCoroutine(nameof(Reload));
        } 
        else if ( Time.time > nextFire && ammo > 0 && AIpressedShoot)
        {
            // subtract current ammo
            ammo--;
        
            // Update the time when our enemy can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine (ShotEffect());
    
            // Create a vector at the center of our camera's viewport
             Vector3 rayOrigin = gunEnd.position;
            //Vector3 rayOrigin = gunEnd.position;

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition (0, gunEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast (rayOrigin, gunEnd.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition (1, hit.point);

                // Get a reference to a health script attached to the collider we hit
                ShootableEntity health = hit.collider.GetComponent<ShootableEntity>(); 

                // If there was a health script attached
                if (health != null)
                {
                    health.Damage(gunDamage);
                } 

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce (-hit.normal * hitForce);

                    StartCoroutine(nameof(ArrivalEffect), "SoftBody");
                }
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition (1, rayOrigin + (gunEnd.forward * weaponRange));
            }
        }   
    }

    public IEnumerator Reload()
    {
        
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        ammo = maxAmmo;

        isReloading = false;

       yield break;
    }

    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        //gunAudio.Play ();
        muzzleFlash.Play();

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;

        muzzleFlash.Stop();
    }

    public IEnumerator ArrivalEffect()
    {
        arrivalDust.Play();

        yield return new WaitForSeconds(2.5f);

        arrivalDust.Stop();
    }
}
