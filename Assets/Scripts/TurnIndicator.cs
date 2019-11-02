using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour
{    
    public bool blueTurn;
    public GameObject cameraPivot;
    public Animator animator;
    public Image turnID;
    public GameObject TurnText;

	public static TurnIndicator indicator;

	private void Awake() {
		indicator = this;
	}


	void Start()
    {
        blueTurn = true;                 
    }

    void Update()
    {
        if (blueTurn == true)
        {
            turnID.GetComponent<Image>().color = Color.blue;            
            TurnText.GetComponent<TextMeshProUGUI>().text = "PLAYER 1'S TURN";
            TurnText.GetComponent<TextMeshProUGUI>().color = Color.blue;
        }
        else
        {
            turnID.GetComponent<Image>().color = Color.red;
            TurnText.GetComponent<TextMeshProUGUI>().text = "PLAYER 2'S TURN";
            TurnText.GetComponent<TextMeshProUGUI>().color = Color.red;
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
