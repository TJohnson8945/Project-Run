using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "PlayerObject")
        {
            Debug.Log("We ran into it");
            GameObject[] vanish = GameObject.FindGameObjectsWithTag("Vanish");
            foreach (GameObject hide in vanish)
            GameObject.Destroy(hide);
        }
    }
}
