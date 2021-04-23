using System;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class Zero : MonoBehaviour {

    public static Zero Instance { get; private set; }

    public GameObject FocusedObject { get; private set; }

    //public bool IsManipulating { get; private set; }

    public Vector3 ManipulationPosition { get; private set; }

    GestureRecognizer recognizer;
	// Use this for initialization
	void Start () {
        Instance = this;

        recognizer = new GestureRecognizer();

        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.Hold | GestureSettings.ManipulationTranslate);

        recognizer.Tapped += Recognizer_Tapped;
        recognizer.HoldStartedEvent += Recognizer_HoldStartedEvent;
        //recognizer.ManipulationStartedEvent += Recognizer_ManipulationStartedEvent;
        //recognizer.ManipulationUpdatedEvent += Recognizer_ManipulationUpdatedEvent;
        //recognizer.ManipulationCompletedEvent += Recognizer_ManipulationCompletedEvent;
        //recognizer.ManipulationCanceledEvent += Recognizer_ManipulationCanceledEvent;

        recognizer.StartCapturingGestures();
	}

    private void Recognizer_Tapped(TappedEventArgs obj)
    {
        if (obj.tapCount == 1)
        {
            OnTap();
        }
        else
        {
            OnDoubleTap();
        }
    }

    //   private void Recognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    //   {
    //       IsManipulating = false;
    //   }

    //   private void Recognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    //   {
    //       IsManipulating = false;
    //   }

    //   private void Recognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    //   {
    //       if (FocusedObject != null)
    //       {
    //           IsManipulating = true;
    //           ManipulationPosition = cumulativeDelta;

    //           FocusedObject.SendMessageUpwards("PerformManipulationUpdate", cumulativeDelta);
    //       }
    //   }

    //   private void Recognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    //   {
    //       if(FocusedObject != null)
    //       {
    //           IsManipulating = true;
    //           ManipulationPosition = cumulativeDelta;

    //           FocusedObject.SendMessageUpwards("PerformManipulationStart",cumulativeDelta);
    //       }
    //   }

    private void Recognizer_HoldStartedEvent(InteractionSourceKind source, Ray headRay)
    {
        if (FocusedObject != null)
        {
            FocusedObject.SendMessage("OnHold");
        }
    }



    private void OnTap()
    {
        if (FocusedObject != null)
        {
            FocusedObject.SendMessage("OnTap");
        }
    }
    private void OnDoubleTap()
    {
        if (FocusedObject != null)
        {
            FocusedObject.SendMessage("OnDoubleTap");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject oldFocusObject = FocusedObject;

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            FocusedObject = null;
        }

        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}
