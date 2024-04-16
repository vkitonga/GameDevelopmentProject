using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]private Transform transformPlayer;
    private bool stickToPlayer;
    private Transform playerBallPosition;
    float speed;
    Vector3 previousLocation;
    Player scriptPlayer;

    public bool StickToPlayer{get=>stickToPlayer;set=>stickToPlayer=value;}
    // Start is called before the first frame update
    void Start()
    {
        playerBallPosition=transformPlayer.Find("Geometry").Find("BallLocation");
        scriptPlayer=transformPlayer.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!stickToPlayer)
        {
            float distanceToPlayer=Vector3.Distance(transformPlayer.position,transform.position);
        if(distanceToPlayer<0.5)
        {
            stickToPlayer=true;
            scriptPlayer.ballAttachedToPlayer=this;
        }
        }
        else
        {
            Vector2 currentLocation=new Vector2(transform.position.x,transform.position.z);
            speed=Vector2.Distance(currentLocation,previousLocation)/Time.deltaTime;

            transform.position=playerBallPosition.position;
            transform.Rotate(new Vector3(transformPlayer.right.x,0,transformPlayer.right.z),speed,Space.World);
            previousLocation=currentLocation;
        }
        
    }
}
