using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
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
            GameObject[] Player = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject hide in Player)
            GameObject.Destroy(hide);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
