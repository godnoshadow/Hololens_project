using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour {

    GameObject m_obj;
    GameObject m_obj1;


    void OnTap()
    {

        m_obj = transform.parent.Find("Stage").gameObject;
        m_obj1 = GameObject.FindGameObjectWithTag("Canvas");

        m_obj.SetActive(false);
        m_obj1.SetActive(true);
        GameObject.Find("Camera").GetComponent<QRcode>().enabled = true;
        
    }
}
