using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Player : MonoBehaviour
{
   [SerializeField]private TextMeshProUGUI textScore;
   private StarterAssetsInputs  starterAssetsInputs ;
   private Ball ballAttachedToPayer;
   private Animator animator;
   private float timeShot=-1f;
   public const int ANIMATION_LAYER_SHOOT=1;
   private int myScore,otherScore;

   public Ball ballAttachedToPlayer{get=>ballAttachedToPlayer;set=>ballAttachedToPlayer=value;}

    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs =GetComponent<StarterAssetsInputs >();
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.shoot )
        {
            starterAssetsInputs.shoot=false;
            timeShot=Time.time;
            animator.Play("Shoot",ANIMATION_LAYER_SHOOT,0F);
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT,1F);
           
        }
        if(timeShot>0)
        {
            //shoot Ball
            if(ballAttachedToPlayer!=null && Time.time-timeShot>0.2)
            {
                ballAttachedToPlayer.StickToPlayer=false;

                Rigidbody rigidbody=ballAttachedToPlayer.transform.gameObject.GetComponent<Rigidbody>();
                Vector3 shootdirection=transform.forward;
                shootdirection.y+=0.2f;
                rigidbody.AddForce(transform.forward*20f,ForceMode.Impulse);

                ballAttachedToPlayer=null;
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
    }
}

