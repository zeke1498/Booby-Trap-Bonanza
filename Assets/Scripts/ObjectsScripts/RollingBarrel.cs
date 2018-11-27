﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrel : MonoBehaviour
{
    public int rollDirection = 1;
    public float rollSpeed = 15f;
    private Rigidbody2D barrelRigidBody;
    private bool canMove = false;
    private ScoreBarScripts scoreBarInfo;
    [Range(0, 1)]
    public float scoreIncreaseAmount;
    // Use this for initialization


    void Start()
    {
        scoreBarInfo = GameObject.Find("ScoreBar").GetComponent<ScoreBarScripts>();
    
    barrelRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            Move();
    }

    public void Move()
    {
        barrelRigidBody.velocity = new Vector3(rollSpeed * rollDirection * Time.deltaTime, barrelRigidBody.velocity.y, 0);
    }

    public void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.layer == 8 || trig.gameObject.layer == 9)
        {
            Destroy(gameObject);
            scoreBarInfo.IncreaseScoreBar(scoreIncreaseAmount);
        }

        if (trig.tag == "Ground")
        {
            canMove = true;
        }
        if(trig.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
