﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonAI : MonoBehaviour {

    public enum State{
        IDLE,
        PATROL,
        ALERT,
        BURNING,
        SLIDING,
        DYING
    }
    public State state;

    private bool alive;

    private float idleTimer = 0f;
    private float IDLETIME = 2f;

    public  Vector2[] waypoints;
    private Vector2 target;
    private float speed = 2;
    private int currentWaypoint = 0;
    private int patrolDirection = -1;
    private int currentDirection;

    private bool moving = false;
    private bool burning = false;
    private bool sliding = false;

    private Animator animatorInfo;

	// Use this for initialization
	void Start () {
        alive = true;
        state = State.IDLE;
        RandomizePosition();
        animatorInfo = gameObject.GetComponent<Animator>();

        StartCoroutine("FSM");
	}
	
    /* Finite State Machine and State Functions */

    IEnumerator FSM()
    {
        while(alive)
        {
            print(state);

            switch (state)
            {
                case State.IDLE:
                    Idle();
                    break;
                case State.PATROL:
                    Patrol();
                    break;
                case State.ALERT:
                    Alert();
                    break;
                case State.BURNING:
                    Burning();
                    break;
                case State.SLIDING:
                    Sliding();
                    break;
                case State.DYING:
                    Dying();
                    break;
            }
            yield return null;
        }
    }

    private void Idle()
    {
        animatorInfo.SetBool("isIdle", true);
        animatorInfo.SetBool("moveRight", false);
        animatorInfo.SetBool("moveLeft", false);
        moving = false;
        idleTimer += Time.deltaTime;

        if(idleTimer >= IDLETIME)
        {
            state = State.PATROL;
            idleTimer = 0;
        }
    }

    private void Patrol()
    {
        float distance = Vector2.Distance(transform.position, target);
        float dirDist = transform.position.x - target.x;
        float step = speed * Time.deltaTime;
        moving = true;

        //TODO Set animation here.
        transform.position = Vector2.MoveTowards(transform.position, target, step);


        if(distance < .001f)
        {
            if(patrolDirection < 0)
            {
                DecrementTarget();
            }
            else
            {
                IncrementTarget();
            }
            state = State.IDLE;
        }

        if (dirDist < 0)
        {
            currentDirection = 1;
            //animatorInfo.SetBool("moveLeft", false);
            animatorInfo.SetBool("isIdle", false);
            animatorInfo.SetBool("moveRight", true);
        }
        else
        {
            currentDirection = -1;
            //  animatorInfo.SetBool("moveRight", false);
            animatorInfo.SetBool("isIdle", false);
            animatorInfo.SetBool("moveLeft", true);
          
        }
           

    }

    private void Alert()
    {

    }

    private void Burning()
    {
       // animatorInfo.SetBool("isBurning", true);
        speed = .1f;
        Invoke("BurningDeathAnim", 4);
        burning = true;

        if(currentDirection > 0)
        {
            Debug.Log("DId this hApeend");
            //animatorInfo.SetFloat("speed", 1);
            animatorInfo.SetBool("moveLeft", false);
            animatorInfo.SetBool("moveRight", true);

            transform.Translate(Vector3.right * speed);
        }
            
        else
        {
            //animatorInfo.SetFloat("speed", -1);
            animatorInfo.SetBool("moveRight", false);
            animatorInfo.SetBool("moveLeft", true);

            transform.Translate(-Vector3.right * speed);
        }
            

        gameObject.layer = 0;
        gameObject.tag = "Interactable";

    }

    private void Sliding()
    {
       // animatorInfo.SetBool("isSlip", true);
    }

    private void Dying()
    {
       // animatorInfo.SetBool("isKill", true);
    }

    /* Other Member Functions to be used */

    private void RandomizePosition()
    {
        currentWaypoint = Random.Range(0, waypoints.Length - 1);
        target = waypoints[currentWaypoint];
        int temp = currentWaypoint + 1;
        transform.position = waypoints[temp];
    }

    private void IncrementTarget()
    {
        print("Current Waypoint Before: " + currentWaypoint);
        if (target == waypoints[waypoints.Length - 1])
        {
            target = waypoints[0];
            currentWaypoint = 0;
        }
        else
        {
            target = waypoints[currentWaypoint++];
           
        }
        print("Current Waypoint After: " + currentWaypoint);
    }

    private void DecrementTarget()
    {
        if (target == waypoints[0])
        {
            target = waypoints[waypoints.Length - 1];
            currentWaypoint = waypoints.Length - 1;
        }
        else
        {
            target = waypoints[currentWaypoint--];
        }
    }

    private void BurningDeathAnim()
    {
        //TODO burning death anim
        //animatorInfo.SetBool("isAsh", true);
       // Destroy(gameObject);
    }

    //private void StayOnGround()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 10);

    //    if(hit.collider.tag == "Ground")
    //    {
    //        float groundDist = hit.distance;
    //        float tempY = hit.distance - transform.GetComponent<Collider2D>().bounds.extents.y;
    //        transform.position = new Vector3(transform.position.x, tempY, transform.position.z);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D col)
    {

        //if (burning || sliding)
        //{

        //
    
            if (col.gameObject.name == "WaterPuddle(Clone)")
            {
                //TODO Slide animation
                animatorInfo.SetBool("isSlip", true);
                state = State.SLIDING;
               
            }
            else if (col.gameObject.name == "FlameWoosh(Clone)")
            {
                //TODO Running on fire animation
                animatorInfo.SetBool("isBurning", true);
                state = State.BURNING;
            }
            else if (col.gameObject.name == "BarrelExplosion(Clone)")
            {
                //TODO Explosion death animation
                animatorInfo.SetBool("isKill", true);
            }
            else if (col.gameObject.name == "ChairLeg(Clone)")
            {
                //TODO Chair leg death here
                animatorInfo.SetBool("isImpale", true);
            }
            else if (col.gameObject.name == "Chandelier(Clone)")
            {
                //TODO Chandelier death here
                animatorInfo.SetBool("isCrush", true);
            }
            else if (col.gameObject.name == "RollingBarrel(Clone)") 
            {
            Debug.Log("I WAAANT TO DIEEE");
            animatorInfo.SetBool("isKill", true);
            }
        }


    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("This collision happened");
        
            if (col.gameObject.name == "WaterPuddle(Clone)")
            {
                //TODO Slide animation
                animatorInfo.SetBool("isSlip", true);
                state = State.SLIDING;

            }
            else if (col.gameObject.name == "FlameWoosh(Clone)")
            {
                //TODO Running on fire animation
                animatorInfo.SetBool("isBurning", true);
                state = State.BURNING;
            }
            else if (col.gameObject.name == "BarrelExplosion(Clone)")
            {
                //TODO Explosion death animation
                animatorInfo.SetBool("isKill", true);
            }
            else if (col.gameObject.name == "ChairLeg(Clone)")
            {
                //TODO Chair leg death here
                animatorInfo.SetBool("isImpale", true);
            }
            else if (col.gameObject.name == "Chandelier(Clone)")
            {
                //TODO Chandelier death here
                animatorInfo.SetBool("isCrush", true);
            }
            else if (col.gameObject.name == "RollingBarrel(Clone)")
            {
                Debug.Log("I WAAANT TO DIEEE");
                animatorInfo.SetBool("isKill", true);
            }
        
    }
}
