using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
   // private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    private void OnCollisionEnter(Collision collision)
    {

      
            Debug.Log("this is the player in the ground");
        
    }
}
