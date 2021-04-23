using UnityEngine;

public class PageDown : MonoBehaviour
{
    public GameObject info1;
    public GameObject info2;
    // Called by GazeGestureManager when the user performs a Select gesture
    void OnTap()
    {

        info1.SetActive(false);

        GameObject inf2 = GameObject.Find("Canvas");
        info2 = inf2.transform.Find("information2").gameObject;

        info2.SetActive(true);
    }
}