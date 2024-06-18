using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Goall : MonoBehaviour
{
    public UnityEvent score;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Ball")
        {
            score.Invoke();
        }
    }
}
