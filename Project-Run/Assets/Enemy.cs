using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    // get the target within code but for now, do it by a*
    public NavMeshAgent agent;

    private void jump(){

    }

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
