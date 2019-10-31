﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnIndicator : MonoBehaviour
{    
    public bool blueTurn;
    public GameObject cameraPivot;
    public Animator animator;
    public GameObject turnID1;
    public GameObject turnID2;
    public GameObject turnID3;
    public GameObject turnID4;

	public static TurnIndicator indicator;

	private void Awake() {
		indicator = this;
	}


	void Start()
    {
        blueTurn = true;
        GetComponent<Renderer>().material.color = Color.blue;                 
    }

    void Update()
    {
        if (blueTurn == true)
        {
            turnID1.GetComponent<Renderer>().material.color = Color.blue;
            turnID2.GetComponent<Renderer>().material.color = Color.blue;
            turnID3.GetComponent<Renderer>().material.color = Color.blue;
            turnID4.GetComponent<Renderer>().material.color = Color.blue;

        }
        else
        {
            turnID1.GetComponent<Renderer>().material.color = Color.red;
            turnID2.GetComponent<Renderer>().material.color = Color.red;
            turnID3.GetComponent<Renderer>().material.color = Color.red;
            turnID4.GetComponent<Renderer>().material.color = Color.red;
        }
        
        
        
    }

    public void TurnChange()
    {
        if (blueTurn == true)
        {
            animator.SetInteger("TurnChange", 1);
            blueTurn = false;
        }
        else
        {
            animator.SetInteger("TurnChange", 2);
            blueTurn = true;
        }
    }
}
