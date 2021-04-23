using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class three : MonoBehaviour {


	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        //改变字体大小
        GUI.skin.label.fontSize = 20;
        //定位显示（左边距x,上边距y,宽，高）
        GUI.Label(new Rect(400, 50, 500, 120), "用手拖动瓶子，单击它可以回归原位");


    }


}
