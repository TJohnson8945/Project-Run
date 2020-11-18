using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] names = Input.GetJoystickNames();
        print(names[0].Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
