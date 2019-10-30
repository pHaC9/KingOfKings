using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public GameObject turnID;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {

        Debug.Log("Collision");
        if (turnID.GetComponent<TurnIndicator>().blueTurn == true && collider.tag == "Player2")
        {
            Destroy(collider.gameObject);
        }
        else if (turnID.GetComponent<TurnIndicator>().blueTurn == false && collider.tag == "Player")
        {
            Destroy(gameObject);
        }
        
    }

}
