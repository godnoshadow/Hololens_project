using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CubeManipulation : MonoBehaviour,IManipulationHandler {

    public bool IsManipulating { get;  set; }

    private Vector3 manipulationOriginalPosition = Vector3.zero;
    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        IsManipulating = false;
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        IsManipulating = false;
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        IsManipulating = true;
        InputManager.Instance.PushModalInputHandler(gameObject);
        manipulationOriginalPosition = transform.position;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        IsManipulating = true;
        transform.position = manipulationOriginalPosition + eventData.CumulativeDelta;
    }
}
