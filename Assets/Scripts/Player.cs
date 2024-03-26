using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private StarterAssetsInputs  starterAssetsInputs ;

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
            Debug.Log("Shoot!");
        }
    }
}
