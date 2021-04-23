using UnityEngine;
using System.Collections;
using ZXing;
using UnityEngine.UI;

public class QRcode : MonoBehaviour
{
    //包含RGBA 
    public Color32[] data;
    //判断是否可以开始扫描 
    private bool isScan;
    // canvas上的RawImage，显示相机捕捉到的图像 
    public RawImage cameraTexture;
    // canvas上的Text，显示获取的二维码内部信息 
    public Text QRcodeText;
    // 相机捕捉到的图像 
    private WebCamTexture webCameraTexture;
    // ZXing中的方法，可读取二维码中的内容
    private BarcodeReader barcodeReader;
    // 计时，0.5s扫描一次
    private float timer = 0;

    GameObject m_obj;
    GameObject m_obj1;
    GameObject m_obj2;



    IEnumerator Start()
    {
        barcodeReader = new BarcodeReader();

        //请求授权使用摄像头
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //获取摄像头设备
            WebCamDevice[] devices = WebCamTexture.devices;
            string devicename = devices[0].name;
            //获取摄像头捕捉到的画面
            webCameraTexture = new WebCamTexture(devicename, 400, 300);
            //显示相机捕捉到的图像
            NewMethod();
            webCameraTexture.Play();
            isScan = true;
        }

    }

    private void NewMethod()
    {
        cameraTexture.texture = webCameraTexture;
    }


    // 循环扫描，0.5秒扫描一次
    void Update()
    {
        if (isScan)
        {
            timer += Time.deltaTime;

            //0.5秒扫描一次
            if (timer > 0.5f) 
            {
                //扫描
                StartCoroutine(ScanQRcode());
                timer = 0;
            }
        }
    }

    IEnumerator ScanQRcode()
    {
        //相机捕捉到的纹理
        data = webCameraTexture.GetPixels32();
        DecodeQR(webCameraTexture.width, webCameraTexture.height);
        yield return 0;
    }

    // 识别二维码并显示其中包含的文字、URL等信息
    // "width"相机捕捉到的纹理的宽度
    // "height"相机捕捉到的纹理的高度
    private void DecodeQR(int width, int height)
    {
        var br = barcodeReader.Decode(data, width, height);
        if (br != null)
        {
            QRcodeText.text = br.Text;
        }
        if (QRcodeText.text ==  "Mechanical Arm" )
        {
            GameObject root = GameObject.Find("MapRoot");
            m_obj   =  root.transform.Find("Stage").gameObject;
            m_obj1  =  GameObject.Find("Camera/Canvas");


            m_obj1.SetActive(false);
            GameObject.Find("Camera").GetComponent<QRcode>().enabled = false;
            m_obj.SetActive(true);
   
        }
        else if(QRcodeText.text == "Garage")
        {
            GameObject root = GameObject.Find("MapRoot");
            m_obj = root.transform.Find("Stage1").gameObject;
            m_obj1 = GameObject.Find("Camera/Canvas");


            m_obj1.SetActive(false);
            GameObject.Find("Camera").GetComponent<QRcode>().enabled = false;
            m_obj.SetActive(true);
        }
        else if (QRcodeText.text == "UAV")
        {
            GameObject root = GameObject.Find("MapRoot");
            m_obj = root.transform.Find("Stage2").gameObject;
            m_obj1 = GameObject.Find("Camera/Canvas");


            m_obj1.SetActive(false);
            GameObject.Find("Camera").GetComponent<QRcode>().enabled = false;
            m_obj.SetActive(true);
        }

    }
}
