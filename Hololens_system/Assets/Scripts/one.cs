using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class one : MonoBehaviour {


    protected float jump_speed = 2.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

       
    }

    public void OnHold()
    {
        if(!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void OnTap()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0 , 255) / 255f , Random.Range(0 , 255) / 255f ,Random.Range(0 , 255) / 255f );
    }

    public void OnDoubleTap()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.up * this.jump_speed;
    }
}
