using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    // get the target within code but for now, do it by a*
    public GameObject nextTarget;
    public NavMeshAgent agent;
    private float manualVelx;
    private float manualVely;
    private bool isJumping = false;
    public GameObject currentPlat;

    private void jump(){
        agent.enabled = false;
        isJumping = true;
        manualVely = nextTarget.transform.position.y-this.transform.position.y;
        manualVely *= 1.2f;
        manualVelx = .4f;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = currentPlat.GetComponent<Link>().jumpStart;
        nextTarget = currentPlat.GetComponent<Link>().jumpEnd;
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if((this.transform.position - target.transform.position).magnitude < 1.2f && !isJumping){
            agent.ResetPath();
            this.transform.LookAt(nextTarget.transform.position);
            jump();
        }
        if(isJumping){
            transform.position += transform.forward * manualVelx;
            transform.position += new Vector3(0, manualVely, 0);
            this.transform.LookAt(nextTarget.transform.position);
            Vector3.MoveTowards(transform.position, nextTarget.transform.position, 3);
            if((transform.position - nextTarget.transform.position).magnitude < 1){
                transform.position = nextTarget.transform.position;
                //agent.enabled = true;
                try{
                    transform.position = nextTarget.transform.position;
                    currentPlat = currentPlat.GetComponent<Link>().plat;
                    target = currentPlat.GetComponent<Link>().jumpStart;
                    nextTarget = currentPlat.GetComponent<Link>().jumpEnd;
                    isJumping = false;
                    agent.enabled = true;
                    agent.SetDestination(target.transform.position);
                }
                catch(Exception e){
                    Debug.Log("Tried, no next target");
                }
            }
            manualVely -= .07f;
        }

    }
}
