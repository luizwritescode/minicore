using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 5.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public float jetpackDuration = 1f;

    //Enables and disables jetpack and weapon abilities
    public bool inMechForm = true;

    Vector3 velocity;
    bool isGrounded;
    float jumpTimer;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private PlayerMotor motor;

    public CharacterController controller;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Movement modifier checks
        

        //Calculate movement velocity as 3D vector from Input and move player
        float _xv = Input.GetAxisRaw("Horizontal");
        float _zv = Input.GetAxisRaw("Vertical");

        Vector3 _move = transform.right * _xv + transform.forward * _zv;

        controller.Move(_move * getSpeed(speed) * Time.deltaTime);

        //Ground and Jump check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //jumps, but seriously how tf does this work
            jumpTimer = Time.time;
        } 
        else if (Input.GetButton("Jump") && !isGrounded && Time.time - jumpTimer < jetpackDuration && inMechForm)
        {
            velocity.y = 5f;
        }

        //Mech controls

        if (Input.GetKeyDown("e") && !inMechForm)
        {
            enterMech();
        } 
        else if (Input.GetKeyDown("e") && inMechForm)
        {
           exitMech(); 
        }
 
    }

    public bool enterMech() { return inMechForm = true;}
    public bool exitMech() { return inMechForm = false;}

    public float getSpeed(float s)
    {   
        float ret = s;
        if (Input.GetButton("Sprint"))
        {   
            ret = (s*1.5f);
        } else 
        {
            ret = s;
        }
        return ret;
    }
}
