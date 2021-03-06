﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairScript : ObjectsScript {
    public GameObject chairLeg;
    private ScoreBarScripts scoreBarInfo;
    [Range(0, 1)]
    public float scoreIncreaseAmount;
    // Use this for initialization


    void Start()
    {
        scoreBarInfo = GameObject.Find("ScoreBar").GetComponent<ScoreBarScripts>();  
    }
    // Update is called once per frame
    void Update ()
    {
        if(isActive)
        {
            isActive = false;
            if(isPossessed)
            {
                GameObject spawnedChairLeg1 = Instantiate(chairLeg, transform.position, transform.rotation);
                spawnedChairLeg1.GetComponent<ChairLegScript>().direction = gameObject.transform.localScale.x;
            }
           GameObject spawnedChairLeg = Instantiate(chairLeg, transform.position, transform.rotation);
            spawnedChairLeg.GetComponent<ChairLegScript>().direction = -gameObject.transform.localScale.x; //do breaking animation here
            //call Destroy at the end of the anim if it doesnt leave debris
            Destroy(gameObject);
        }
       
    }


    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Interactable" && col.gameObject.name != "WaterPuddle(Clone)")
        {
            isActive = true;
            scoreBarInfo.IncreaseScoreBar(scoreIncreaseAmount);
        }

    }



    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Interactable" && col.gameObject.name != "WaterPuddle(Clone)")
        {
            isActive = true;
            scoreBarInfo.IncreaseScoreBar(scoreIncreaseAmount);
        }

    }

}
