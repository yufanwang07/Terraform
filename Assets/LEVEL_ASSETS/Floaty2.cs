using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaty2 : MonoBehaviour {
    // User Inputs
    public float amplitude = 0.5f;
    public float frequency = 1f;
 
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
 
    // Use this for initialization
    void Start () {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }
     
    // Update is called once per frame
    void Update () {
 
        tempPos = posOffset;
        tempPos.y += Mathf.Cos (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }
}
