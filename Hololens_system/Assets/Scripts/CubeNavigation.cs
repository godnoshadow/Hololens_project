using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CubeNavigation : MonoBehaviour, INavigationHandler
{
    [Tooltip("旋转速度")]
    public float RotationSensitivity = 5.0f;


    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        CubeManipulation cm = new CubeManipulation();

        if (cm.IsManipulating == false) {

            // 计算旋转值，其中：eventData的CumulativeDelta返回手势导航差值，值域[-1, 1]
            float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
        else
        {
            return;
        }

    }
}
