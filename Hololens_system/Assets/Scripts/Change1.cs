using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change1 : MonoBehaviour {

    GameObject m_obj;
    GameObject m_obj1;


    void OnTap()
    {
        m_obj = transform.parent.Find("Stage1").gameObject;
        m_obj1 = GameObject.Find("Camera/Canvas");

        m_obj1.SetActive(true);
        GameObject.Find("Camera").GetComponent<QRcode>().enabled = true;
        m_obj.SetActive(false);
    }
}
