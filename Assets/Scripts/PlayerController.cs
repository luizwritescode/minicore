using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanMechaController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 5.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public float jetpackDuration = 1f;

    public Vector3 velocity;
    bool isGrounded;
    float jumpTimer;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private HumanMechaController HMC;  
    private bool inMechForm;

    public CharacterController controller;

    private void Awake()
    {
        if(HMC.currentState == "mecha")
            inMechForm = true;
        else 
            inMechForm = false;
    }

    void Update()
    {
       

        //Apply gravity
        velocity.y += gravity * Time.deltaTime;

        //Calculate movement velocity as 3D vector from Input and move player
        float _xv = Input.GetAxisRaw("Horizontal") * GetSpeed(speed);
        float _zv = Input.GetAxisRaw("Vertical") * GetSpeed(speed);

        velocity += transform.right * _xv + transform.forward * _zv;

        if (controller.isGrounded && velocity.y < 0)
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
        controller.Move(velocity * Time.deltaTime);
    }

    //alter attributes based on current form

    public bool EnterMecha() { jumpHeight = 5f; speed = speed * 0.8f; jetpackDuration = 1f; return inMechForm = true;}
    public bool ExitMecha() { jumpHeight = 1f; speed = 5.5f; jetpackDuration = 0f; return inMechForm = false;}

    public float GetSpeed(float s)
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
