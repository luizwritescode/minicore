using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    //velocity vector
    public Vector3 velocity = Vector3.zero;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Sets Movement Vector
    public void setMoveVector(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //Execute Movement Based on velocity vector
    void Move() 
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
}
