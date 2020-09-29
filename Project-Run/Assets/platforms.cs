using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platforms
{
    GameObject[] allPlats;

    public platforms(){
        allPlats = GameObject.FindGameObjectsWithTag("platform");
    }

    public GameObject[] getPlatforms(){
        return allPlats;
    }
    
    public GameObject[] getJumpFromPlat(GameObject plat){
        GameObject[] jumps = new GameObject[2];
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("jumpPos")){
            if(g.transform.parent.gameObject == plat){
                if(jumps[0] == null){
                    jumps[0] = g;
                }
                else{
                    jumps[1] = g;
                }
            }
        }
        return jumps;
    }


}
