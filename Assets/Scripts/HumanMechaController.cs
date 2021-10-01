using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMechaController : MonoBehaviour
{

    public GameObject Player;
    public GameObject Camera;

    public double gravity = -9.81;

    public PlayerController controller;

    public Vector3 moveDirection = new Vector3(0,0,0);

    public string currentState;

    private Rigidbody rb;
    private Vector3 pos;
    private Vector3 velocity = new Vector3(0,0,0);

    private void Awake() 
    {
        controller = Player.GetComponent<PlayerController>();    
        EnableMecha();
    }

    void Start()
    {
        //PlayerController = Player.GetComponent<CharacterController>();    
    }

    void Update()
    {   

        if (Input.GetKeyDown( KeyCode.E ) && currentState != "mecha")
        {
            EnableMecha();
        } 
        else if (Input.GetKeyDown( KeyCode.E ) && currentState != "human")
        {
            DisableMecha();
        }
    }

    public void EnableMecha()
    {
        //change player speed and camera angle and gun

        Debug.Log("entering mecha...");
        controller.EnterMecha();
        controller.velocity = 15f * Vector3.up;
        //Player.transform.position.y = 2f;
        currentState = "mecha";
    }    

    public void DisableMecha()
    {   
        Debug.Log("leaving mecha...");
        controller.ExitMecha();
        currentState = "human";
    }

}
