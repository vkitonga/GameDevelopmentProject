using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Player : MonoBehaviour
{
   [SerializeField]private TextMeshProUGUI textScore;
   [SerializeField]private TextMeshProUGUI textGoal;
   private StarterAssetsInputs  starterAssetsInputs ;
   private Animator animator;
   private Ball ballAttachedToPlayer; //Updated Line
   private float timeShot=-1f;
   public const int ANIMATION_LAYER_SHOOT=1;
   private int myScore,otherScore;
   private float goalTextColorAlpha;

    //NEW CODE
    [HideInInspector]
    public bool altInput;
    [HideInInspector]
    public AltPlayerInput inputScript;

    //Updated Line
   public Ball BallAttachedToPlayer {get => ballAttachedToPlayer; set => ballAttachedToPlayer = value;}

    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs =GetComponent<StarterAssetsInputs >();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.shoot || altInput && inputScript.shoot)
        {
            starterAssetsInputs.shoot=false;
            timeShot=Time.time;
            animator.Play("SHOOT",ANIMATION_LAYER_SHOOT,0f);
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT,1f);
           
        }
        if(timeShot>0)
        {
            //shoot Ball
            if (ballAttachedToPlayer != null && Time.time-timeShot > 0.2)
            {
                ballAttachedToPlayer.StickToPlayer=false;

                Rigidbody rigidbody= ballAttachedToPlayer.transform.gameObject.GetComponent<Rigidbody>();
                Vector3 shootdirection=transform.forward;
                shootdirection.y += 0.2f;
                rigidbody.AddForce(transform.forward*50f,ForceMode.Impulse);

                ballAttachedToPlayer = null;
            }
            //finish kicking animation
            if(Time.time-timeShot>0.5)
            {
                timeShot=-1f;
            }
        }
        else
        {
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT,Mathf.Lerp(animator.GetLayerWeight(ANIMATION_LAYER_SHOOT),0f,Time.deltaTime*10f));
        }
        if(goalTextColorAlpha>0)
        {
            goalTextColorAlpha-=Time.deltaTime;
            textGoal.alpha=goalTextColorAlpha;
            textGoal.fontSize=90-(goalTextColorAlpha * 1-0);
        }
    }

    public void IncreasemyScore()
    {
        myScore++;
        UpdateScore();
    }
    
    public void IncreaseotherScore()
    {
        otherScore++;
        UpdateScore();
    }
    private void UpdateScore()
    {
        textScore.text="Score:"+myScore.ToString()+"-"+otherScore.ToString();
        goalTextColorAlpha=1f;
         TurnManager.instance.ChangeTurn();
    }
   
}

