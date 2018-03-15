using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, IPooledObject
{
    Transform player;

    public void OnObjectSpawn()
    {
        player = GameManager.player.transform;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!GameState.isAlive)
            gameObject.SetActive(false);
        transform.position = player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin") || other.CompareTag("DoubleCoin"))
        {
            other.GetComponent<MagnetToPlayer>().enabled = true;
        }
            
    }
    public void DestroyMagnet()
    {
        gameObject.SetActive(false);

    }



}
