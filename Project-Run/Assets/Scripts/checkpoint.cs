using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public Vector3 respawnPoint;
    
    public void die(){
        this.transform.position = respawnPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("checkpoint")){
            respawnPoint = other.gameObject.transform.position;
        }
    }
}
