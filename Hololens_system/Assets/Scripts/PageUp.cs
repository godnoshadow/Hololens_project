using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUp : MonoBehaviour {

    public GameObject info1;
    public GameObject info2;
    // Called by GazeGestureManager when the user performs a Select gesture
    void OnTap()
    {
        GameObject inf1 = GameObject.Find("Canvas");
        info1 = inf1.transform.Find("information1").gameObject;
        info1.SetActive(true);

        info2.SetActive(false);
    }
}
