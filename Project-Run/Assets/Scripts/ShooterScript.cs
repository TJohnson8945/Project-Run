using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public GameObject target;
    public GameObject bulletPrefab;
    public GameObject shooter;
    public float maxTurningSpeed = 15.0f;
    private Quaternion qTo;
    private float cooldown;
    public float tba = 5;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = tba;
        qTo = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        var v3t = target.transform.position - transform.position;
        qTo = Quaternion.LookRotation(v3t, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, maxTurningSpeed * Time.deltaTime);
        cooldown -= Time.deltaTime;
        if(cooldown < 0){
            cooldown = tba;
            shoot();
        }
    }

    private void shoot(){
        //create new child with rotation and stuff
        Instantiate(bulletPrefab, shooter.transform.position, shooter.transform.rotation);
    }
}
