using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public GameObject turnID;

    private void OnTriggerEnter(Collider collider)
    {

        //if (turnID.GetComponent<TurnIndicator>().blueTurn == true && collider.tag == "Player2")
        //{
        //    Debug.Log("Collision with Player2");

        //    Destroy(gameObject);
        //}

    }

}
