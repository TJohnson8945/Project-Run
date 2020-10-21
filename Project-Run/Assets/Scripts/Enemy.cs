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
    public float speed = 5;
    
    private float distToTarget;
    
    private void jump(){
        agent.enabled = false;
        isJumping = true;
        manualVely = (nextTarget.transform.position.y - transform.position.y + 4)/3;
        Debug.Log(manualVely);
        Vector3 distToTargetV = nextTarget.transform.position - transform.position;
        distToTargetV.y = 0;
        distToTarget = distToTargetV.magnitude;
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
            //this.transform.LookAt(nextTarget.transform.position);
            Vector3 dirOfTar = nextTarget.transform.position - transform.position;
            dirOfTar.y = 0;
            dirOfTar = dirOfTar.normalized;
            Debug.Log(dirOfTar);
            transform.position = new Vector3(transform.position.x + dirOfTar.x * speed, transform.position.y + manualVely*Time.deltaTime, transform.position.z + dirOfTar.z * speed);
            if((transform.position - nextTarget.transform.position).magnitude < 1){
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
            manualVely -= 0.1f;
        }

    }
}
