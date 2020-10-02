using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public GameObject jumpStart;
    public GameObject jumpEnd;
    public GameObject plat;

    Link(){

    }

    public Vector3 getFrom(){
        return plat.GetComponent<Link>().jumpStart.transform.position;
    }
    public Vector3 getTo(){
        return jumpEnd.transform.position;
    }
}
