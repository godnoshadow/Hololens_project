using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class two : MonoBehaviour {
    private Vector3 manipulationOriginalPosition = Vector3.zero;
    private Vector3 OriginalPosition = Vector3.zero;

    // Use this for initialization
    void Start () {

        OriginalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
    

    }

    void PerformManipulationStart (Vector3 position)
    {
        manipulationOriginalPosition = transform.position;
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        transform.position = manipulationOriginalPosition + Zero.Instance.ManipulationPosition;
    }


    public void OnTap()
    {
            transform.position = OriginalPosition;
    }


}
