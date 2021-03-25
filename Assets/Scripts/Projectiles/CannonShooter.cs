using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShooter : MonoBehaviour
{
    public Transform player = null;
    public GameObject cannonball = null;
    public GameObject play;
    private Player the_Player;

    private float destroyTime = 3.0f;

    public float delay = 5.0f;
    private float lastFired = 0.0f;
    private bool targetSafe = false;

    private int exitSafeZone = 0; 
    private float exitSafeZoneTimer = 0.0f;
    public float exitSafeZoneDelay = 4.0f;

    // Hit by bullet variables
    private float disabledTimer = 0.0f;
    public float disabledTimeLimit = 6.0f;

    void Start(){
        PlayerPrefs.SetInt("disabled", 0);
        play = GameObject.Find("PlayerController");
        the_Player = play.GetComponent<Player>();
    }


    private void Update()
    {
        targetSafe = the_Player.safe;

        if(PlayerPrefs.GetInt("disabled") == 1){
            disabledTimer += Time.deltaTime;
            if(disabledTimer >= disabledTimeLimit){
                disabledTimer = 0.0f; 
                PlayerPrefs.SetInt("disabled", 0);
            }
        }
        if(targetSafe){
            exitSafeZone = 1;
        }
        if(!targetSafe && exitSafeZone == 1){
            exitSafeZone = 2;
        }
        if(exitSafeZone == 2){
            exitSafeZoneTimer += Time.deltaTime;
            if(exitSafeZoneTimer >= exitSafeZoneDelay){
                exitSafeZone = 0;
                exitSafeZoneTimer = 0.0f;
            }
        }

        if(!targetSafe && exitSafeZone == 0 && PlayerPrefs.GetInt("disabled") == 0){
            TrackPlayer();
            ShootCannon();
        }
    }

    void TrackPlayer ()
    {
        this.transform.LookAt(player);
    }

    void ShootCannon()
    {
        if (Time.time > lastFired + delay)
        {
            lastFired = Time.time;
            GameObject obj;
            obj = Instantiate(cannonball, this.transform.position, this.transform.rotation) as GameObject;
            obj.GetComponent<Rigidbody>().AddForce(transform.forward*10000.0f);
            Destroy(obj, destroyTime);
        }
    }

}
