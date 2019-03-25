﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidTumbler : MonoBehaviour {

    public float tumbleSpeed = 1f;
    Rigidbody rb;   
    int aPoints = 20;

    public static AudioClip hitSound;
    public AudioClip ChangeSound;
    private AudioSource source;
    private float volLowRange = 1.0f;
    private float volHighRange = 2.0f;
    public static int score = 0;
    public static float health = 100;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward);
        rb.angularVelocity = Random.insideUnitSphere * tumbleSpeed;
        source = GetComponent<AudioSource>();
	}

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            health -= 10f;
            // Remove Asteroid from game
            Destroy(this.gameObject);

            
        }
        if (collision.tag == "Bullet")
        {
            float volume = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(hitSound, volume);

            score += aPoints;

            // Remove the asteroid from the game
            Destroy(this.gameObject);

            //Remove the Bullet
            Destroy(collision.gameObject);
        }
    }
    

}
