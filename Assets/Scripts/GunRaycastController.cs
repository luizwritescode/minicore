using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*

    Ripped off learn.unity.com Shooting with Raycasts tutorial
    @TODO
    - Audio is missing
    - Make both guns shoot

*/
public class GunRaycastController : MonoBehaviour {

    public int gunDamage = 1;                                            // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                        // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                        // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                        // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEndRight;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
    public Transform gunEndLeft;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun
    public WaitForSeconds shotDuration = new WaitForSeconds(0.1f);          // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible

    private Camera fpsCam;                                                // Holds a reference to the first person camera
    private AudioSource gunAudio;                                        // Reference to the audio source which will play our shooting sound effect
    public LineRenderer laserLineRight;                                        // Reference to the LineRenderer component which will display our laserline
    public LineRenderer laserLineLeft;                                        // Reference to the LineRenderer component which will display our laserline
    private float nextFire;                                                // Float to store the time the player will be allowed to fire again, after firing

    public ParticleSystem muzzleFlashRight;
    public ParticleSystem muzzleFlashLeft;

    public int maxAmmo;
    public int ammo;

    public UnityEvent isReloadingEvent;
    public bool isReloading;
    public float reloadTime;

    public Animator animator;

    void Start () 
    {

        // Get and store a reference to our AudioSource component
        //gunAudio = GetComponent<AudioSource>();

        //store particle system reference
        muzzleFlashRight = muzzleFlashRight.GetComponent<ParticleSystem>();
        muzzleFlashLeft = muzzleFlashLeft.GetComponent<ParticleSystem>();
   
        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInParent<Camera>();

        // starts listening for the Reload function in UnityEvent
        isReloading = false;

    }


    void Update () 
    {
        if( ammo <= 0 || Input.GetKey("r"))
        {
            StartCoroutine(nameof(Reload));
        }
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired  and check if they have enough ammo
        else if (Input.GetButton("Fire1") && Time.time > nextFire && ammo > 0) 
        {
            // subtract current ammo
            ammo--;
        
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine (ShotEffect());
    
            // Create a vector at the center of our camera's viewport
             Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            //Vector3 rayOrigin = gunEnd.position;

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLineRight.SetPosition (0, gunEndRight.position);
            laserLineLeft.SetPosition (0, gunEndLeft.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                laserLineRight.SetPosition (1, hit.point);
                laserLineLeft.SetPosition (1, hit.point);

                // Get a reference to a health script attached to the collider we hit
                ShootableObject health = hit.collider.GetComponent<ShootableObject>(); 

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
                }
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLineRight.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
                laserLineLeft.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }

    public IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        
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
        muzzleFlashRight.Play();
        muzzleFlashLeft.Play();

        // Turn on our line renderer
        laserLineRight.enabled = true;
        laserLineLeft.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLineRight.enabled = false;
        laserLineLeft.enabled = false;


        muzzleFlashRight.Stop();
        muzzleFlashLeft.Stop();
    }
}