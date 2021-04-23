// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// A Tagalong that stays at a fixed distance from the camera and always
    /// seeks to have a part of itself in the view frustum of the camera.
    /// </summary>
    [RequireComponent(typeof(BoxCollider), typeof(Interpolator))]
    public class SimpleTagalong : MonoBehaviour
    {
        // Simple Tagalongs seek to stay at a fixed distance from the Camera.

        //Tagalong在更新其位置时要寻找的距离（以米为单位）
        [Tooltip("The distance in meters from the camera for the Tagalong to seek when updating its position.")]
        public float TagalongDistance = 2.0f;
        //如果为真，则强制Tagalong与摄像机保持Tagalong距离，即时它不需要去移动
        [Tooltip("If true, forces the Tagalong to be TagalongDistance from the camera, even if it didn't need to move otherwise.")]
        public bool EnforceDistance = true;

        //更新Tagalong位置时移动Tagalong的速度（米/秒）
        [Tooltip("The speed at which to move the Tagalong when updating its position (meters/second).")]
        public float PositionUpdateSpeed = 9.8f;
        //当为真时，Tagalong的移动是平滑的
        [Tooltip("When true, the Tagalong's motion is smoothed.")]
        public bool SmoothMotion = true;
        //应用于平滑算法的因子，1.0f超光滑。但是速度会慢很多
        [Range(0.0f, 1.0f), Tooltip("The factor applied to the smoothing algorithm. 1.0f is super smooth. But slows things down a lot.")]
        public float SmoothingFactor = 0.75f;


        // The BoxCollider represents the volume of the object that is tagging
        // along. It is a required component.
        //BoxCollider表示被表示物体的体积，它是必要的组件
        protected BoxCollider tagalongCollider;

        // The Interpolator is a helper class that handles various changes to an
        // object's transform. It is used by Tagalong to adjust the object's
        // transform.position.
        //插值器`Interpolator`是一个帮助类，它处理对象变换的各种变化。它被Tagalong用来调整对象的transform.position。
        protected Interpolator interpolator;

        // This is an array of planes that define the camera's view frustum along
        // with some helpful indices into the array. The array is updated each
        // time through FixedUpdate().
        //这是一个平面数组，它定义了相机的视图截锥以及一些有用的索引。每次通过FixedUpdate()更新数组
        protected Plane[] frustumPlanes;
        protected const int frustumLeft = 0;
        protected const int frustumRight = 1;
        protected const int frustumBottom = 2;
        protected const int frustumTop = 3;

        protected virtual void Start()
        {
            // Make sure the Tagalong object has a BoxCollider.
            //确定Tagalong物体有一个BoxCollider
            tagalongCollider = GetComponent<BoxCollider>();

            
            // Get the Interpolator component and set some default parameters for
            // it. These parameters can be adjusted in Unity's Inspector as well.
            //获取插值器组件和为它设置一些默认的参数。这些参数也可以在Unity的控制面板设置
            interpolator = gameObject.GetComponent<Interpolator>();
            interpolator.SmoothLerpToTarget = SmoothMotion;
            interpolator.SmoothPositionLerpRatio = SmoothingFactor;
        }

        protected virtual void Update()
        {
            Camera mainCamera = CameraCache.Main;
            // Retrieve the frustum planes from the camera.
            //从摄像机检索截锥体平面 GeometryUtility:几何工具
            frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            // Determine if the Tagalong needs to move based on whether its
            // BoxCollider is in or out of the camera's view frustum.
            //判断平滑视图是否需要移动依赖于它的碰撞器是在摄像机截椎体平面的里面还是外面
            Vector3 tagalongTargetPosition;
            if (CalculateTagalongTargetPosition(transform.position, out tagalongTargetPosition))
            {
                // Derived classes will use the same Interpolator and may have
                // adjusted its PositionUpdateSpeed for some other purpose.
                // Restore the value we care about and tell the Interpolator 
                // to move the Tagalong to its new target position.
                //派生类将使用相同的插值器，并且可能出于其它目的调整了他的位置更新速度。 恢复关心的值，并告诉插值器将Tagalong移动到它的新目标位置
                interpolator.PositionPerSecond = PositionUpdateSpeed;
                interpolator.SetTargetPosition(tagalongTargetPosition);
            }
            else if (!interpolator.Running && EnforceDistance)
            {
                // If the Tagalong is inside the camera's view frustum, and it is
                // supposed to stay a fixed distance from the camera, force the
                // tagalong to that location (without using the Interpolator).
                //如果Tagalong在相机的截锥体视图里面，并且它应该与摄像机保持一段固定的距离，强制标记到该位置（不使用插值器）
                Ray ray = new Ray(mainCamera.transform.position, transform.position - mainCamera.transform.position);
                transform.position = ray.GetPoint(TagalongDistance);
            }
        }

        /// <summary>
        /// Determines if the Tagalong needs to move based on the provided
        /// position.
        /// 判断Tagalong是否需要移动依赖于提供的位置
        /// </summary>
        /// <param name="fromPosition">Where the Tagalong is.</param>
        /// <param name="toPosition">Where the Tagalong needs to go.</param>
        /// <returns>True if the Tagalong needs to move to satisfy requirements; false otherwise.</returns>
        //如果Tagalong需要移动以满足需求，则为真，否则为假

        protected virtual bool CalculateTagalongTargetPosition(Vector3 fromPosition, out Vector3 toPosition)
        {
            // Check to see if any part of the Tagalong's BoxCollider's bounds is
            // inside the camera's view frustum. Note, the bounds used are an Axis
            // Aligned Bounding Box (AABB).
            //检查是否Tagalong的BoxCollider的边界的任一部分在相机的截锥体视图的里面。注意，使用的边界是一个轴对齐的边界框（AABB)
            bool needsToMove = !GeometryUtility.TestPlanesAABB(frustumPlanes, tagalongCollider.bounds);
            Transform cameraTransform = CameraCache.Main.transform;

            // If we already know we don't need to move, bail out early.
            //如果我们已经知道我们不需要去移动，那就早点离开
            if (!needsToMove)
            {
                toPosition = fromPosition;
                return false;
            }

            // Calculate a default position where the Tagalong should go. In this
            // case TagalongDistance from the camera along the gaze vector.
            //计算Tagalong应该放在哪里的默认位置。在本例中，沿着凝视向量与摄像机保持距离。
            toPosition = cameraTransform.position + cameraTransform.forward * TagalongDistance;

            // Create a Ray and set it's origin to be the default toPosition that
            // was calculated above.
            //创建一条射线，并将其原点设置为上面计算过的默认拓扑
            Ray ray = new Ray(toPosition, Vector3.zero);
            Plane plane = new Plane();
            float distanceOffset = 0f;

            // Determine if the Tagalong needs to move to the right or the left
            // to get back inside the camera's view frustum. The normals of the
            // planes that make up the camera's view frustum point inward.
            //确定Tagalong是否需要移动到右或左，以回到相机的视图截锥体内，构成相机的视图截锥体的平面法线指向内
            bool moveRight = frustumPlanes[frustumLeft].GetDistanceToPoint(fromPosition) < 0;
            bool moveLeft = frustumPlanes[frustumRight].GetDistanceToPoint(fromPosition) < 0;
            if (moveRight)
            {
                // If the Tagalong needs to move to the right, that means it is to
                // the left of the left frustum plane. Remember that plane and set
                // our Ray's direction to point towards that plane (remember the
                // Ray's origin is already inside the view frustum.
                //如果Tagalong需要向右移动，这意味着它在左截锥体平面的左边。记住这个平面，并设置光线的方向指向这个平面（记住光线的原点已经在视图截锥体内）
                plane = frustumPlanes[frustumLeft];
                ray.direction = -cameraTransform.right;
            }
            else if (moveLeft)
            {
                // Apply similar logic to above for the case where the Tagalong
                // needs to move to the left.
                //对于Tagalong需要向左移动的情况，应用与上面类似的逻辑。
                plane = frustumPlanes[frustumRight];
                ray.direction = cameraTransform.right;
            }
            if (moveRight || moveLeft)
            {
                // If the Tagalong needed to move in the X direction, cast a Ray
                // from the default position to the plane we are working with.
                //如果Tagalong需要在X方向上移动，那么将一条射线从默认位置投射到我们正在工作的平面上。
                plane.Raycast(ray, out distanceOffset);

                // Get the point along that ray that is on the plane and update
                // the x component of the Tagalong's desired position.
                // 沿着平面上的射线得到这个点，并更新Tagalong所需位置的x分量
                toPosition.x = ray.GetPoint(distanceOffset).x;
            }

            // Similar logic follows below for determining if and how the
            // Tagalong would need to move up or down.
            bool moveDown = frustumPlanes[frustumTop].GetDistanceToPoint(fromPosition) < 0;
            bool moveUp = frustumPlanes[frustumBottom].GetDistanceToPoint(fromPosition) < 0;
            if (moveDown)
            {
                plane = frustumPlanes[frustumTop];
                ray.direction = cameraTransform.up;
            }
            else if (moveUp)
            {
                plane = frustumPlanes[frustumBottom];
                ray.direction = -cameraTransform.up;
            }
            if (moveUp || moveDown)
            {
                plane.Raycast(ray, out distanceOffset);
                toPosition.y = ray.GetPoint(distanceOffset).y;
            }

            // Create a ray that starts at the camera and points in the direction
            // of the calculated toPosition.
            // 创建一条从相机开始并指向计算拓扑方向的光线
            ray = new Ray(cameraTransform.position, toPosition - cameraTransform.position);

            // Find the point along that ray that is the right distance away and
            // update the calculated toPosition to be that point.
            // 沿着这条射线找到正确距离的点，并将计算出的拓扑结构更新为这一点
            toPosition = ray.GetPoint(TagalongDistance);

            // If we got here, needsToMove will be true.
            return needsToMove;
        }
    }
}