using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement Speed
    public float movementSpeed;
    //turn Speed
    public float turnSpeed;

    // identify players
    public enum Players {player1, player2 }
    public Players player;
    string xInput, zInput;

    //component variable
    private Rigidbody rb;
    private Vector3 inputDirection;

    //stretch goal: ball lock

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(player==Players.player1)
        {
            xInput="Horizontal";
            zInput="Vertical";
        }
        else
        {
            xInput="Horizontal2";
            zInput="Vertical2";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get the horizontal and vertical inputs from the keyboard
        float x = Input.GetAxis(xInput);
        float y = Input.GetAxis(zInput);

        //get forward and side directions from the camera
        Vector3 camX = Camera.main.transform.right;
        Vector3 camZ = Vector3.Cross(camX, Vector3.up);

        //combine the camera and input directions
        inputDirection = camX * x + camZ * y;
        inputDirection.Normalize();

        if(inputDirection != Vector3.zero)
        {
            RotatePlayer();
            MovePlayer();
        }
       
        
    }

    void MovePlayer()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
    void RotatePlayer()
    {
        //convert the direction to a quaternion
        Quaternion targetDir = Quaternion.LookRotation(inputDirection);
        //lerp towards the quaternion
        Quaternion lerpedDir = Quaternion.Lerp(transform.rotation, targetDir, turnSpeed * Time.deltaTime);
        //apply the rotation to the character
        transform.rotation = lerpedDir;
    }
    
}
