using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    private static int playerScore = 0; // I made the playerscore static so it keepss its value instead of being reset everytime it hit the collectible
    
    private void OnCollisionEnter(Collision other) // So when my player collides with the coin, it enters this stage
    {
        if (other.gameObject.tag == "Player") // if object is tagged with 'player'
        {
            Debug.Log("Coin Collected!"); // the coin will be collected
            Destroy(gameObject); // and destroyed

            playerScore++;
            Debug.Log("Player Score: " + playerScore); //this keeps the score updated
        }
    }
}