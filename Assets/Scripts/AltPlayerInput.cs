using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Photon.Pun;
using Photon.Realtime;
public enum PlayerNumber { ONE, TWO }

public class AltPlayerInput : MonoBehaviour
{
    private PhotonView view;
    public PlayerNumber playerNumber;

    [HideInInspector]
    public Vector2 moveInput;

    private KeyCode shootKey;

    [HideInInspector]
    public bool shoot;
    [HideInInspector]
    public bool analogMovement;
    public bool isOnline = false;
    //script references
    ThirdPersonController TPC;
    Player p;
    StarterAssetsInputs SAI;

    void Start()
    {
         view = GetComponent<PhotonView>();
        if (playerNumber == PlayerNumber.ONE)
        {
            shootKey = KeyCode.G;
            moveInput = CalculateInputVector();
        }
        else
        {
            shootKey = KeyCode.Comma;
            moveInput = CalculateInputVector();
        }
        FindAndAlterScripts();
    }
    public void Update()
    {
        if (isOnline && view.IsMine == false) return; // cancel the inputs if we are online but this is not our player
        moveInput = CalculateInputVector();
        shoot = Input.GetKey(shootKey);
    }
    Vector2 CalculateInputVector()
    {
        if(playerNumber == PlayerNumber.ONE)
        {
            float x = -ToInt(Input.GetKey(KeyCode.A)) + ToInt(Input.GetKey(KeyCode.D));
            float y = -ToInt(Input.GetKey(KeyCode.S)) + ToInt(Input.GetKey(KeyCode.W));
            return new Vector2(x, y);
            
        }
        else
        { 
            float x = -ToInt(Input.GetKey(KeyCode.LeftArrow)) + ToInt(Input.GetKey(KeyCode.RightArrow));
            float y = -ToInt(Input.GetKey(KeyCode.DownArrow)) + ToInt(Input.GetKey(KeyCode.UpArrow));
            return new Vector2(x, y);
             
        }
    } 
    int ToInt(bool myBool)
    {
        return myBool ? 1 : 0;
    }
    void FindAndAlterScripts()
    {
        TPC = GetComponent<ThirdPersonController>();
        TPC.altInputs = true;
        TPC.inputScript = this;

        p = GetComponent<Player>();
        p.altInput = true;
        p.inputScript = this;

        SAI = GetComponent<StarterAssetsInputs>();
        SAI.enabled = false;
    }
}
