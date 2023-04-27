using UnityEngine;
using System.Collections;
    
public class CameraController : MonoBehaviour
{
    public enum CameraModes { Follow, Isometric, Free }

    private Transform cameraTransform;
    private Transform dummyTarget;

    public Transform CameraTarget;

    public float FollowDistance = 30.0f;
    public float MaxFollowDistance = 100.0f;
    public float MinFollowDistance = 2.0f;

    public float ElevationAngle = 30.0f;
    public float MaxElevationAngle = 85.0f;
    public float MinElevationAngle = 0f;

    public float OrbitalAngle = 0f;

    public CameraModes CameraMode = CameraModes.Follow;

    public bool MovementSmoothing = true;
    public bool RotationSmoothing = false;
    private bool previousSmoothing;

    public float MovementSmoothingValue = 25f;
    public float RotationSmoothingValue = 5.0f;

    public float MoveSensitivity = 2.0f;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 desiredPosition;
    private float mouseX;
    private float mouseY;
    private Vector3 moveVector;
    private float mouseWheel;

    private const string event_SmoothingValue = "Slider - Smoothing Value";
    private const string event_FollowDistance = "Slider - Camera Zoom";


    void Awake()
    {
        if (QualitySettings.vSyncCount > 0)
            Application.targetFrameRate = 60;
        else
            Application.targetFrameRate = -1;

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            Input.simulateMouseWithTouches = false;

        cameraTransform = transform;
        previousSmoothing = MovementSmoothing;
    }


    //初始化
    void Start()
    {
        if (CameraTarget == null)
        {
            dummyTarget = new GameObject("Camera Target").transform;
            CameraTarget = dummyTarget;
        }
    }

        
    void LateUpdate()
    {
        GetPlayerInput();


        // 检查是否有目标
        if (CameraTarget != null)
        {
            if (CameraMode == CameraModes.Isometric)
            {
                desiredPosition = CameraTarget.position + Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * new Vector3(0, 0, -FollowDistance);
            }
            else if (CameraMode == CameraModes.Follow)
            {
                desiredPosition = CameraTarget.position + CameraTarget.TransformDirection(Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * (new Vector3(0, 0, -FollowDistance)));
            }
            else
            {
                    
            }

            if (MovementSmoothing == true)
            {
                // 平滑阻尼
                cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref currentVelocity, MovementSmoothingValue * Time.fixedDeltaTime);
                //cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5.0f);
            }
            else
            {
                cameraTransform.position = desiredPosition;
            }

            if (RotationSmoothing == true)
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.LookRotation(CameraTarget.position - cameraTransform.position), RotationSmoothingValue * Time.deltaTime);
            else
            {
                cameraTransform.LookAt(CameraTarget);
            }
        }
    }

    void GetPlayerInput()
    {
        moveVector = Vector3.zero;

        // 鼠标滚轮操作
        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        float touchCount = Input.touchCount;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || touchCount > 0)
        {
            mouseWheel *= 10;

            if (Input.GetKeyDown(KeyCode.I))
                CameraMode = CameraModes.Isometric;

            if (Input.GetKeyDown(KeyCode.F))
                CameraMode = CameraModes.Follow;

            if (Input.GetKeyDown(KeyCode.S))
                MovementSmoothing = !MovementSmoothing;


            // 鼠标右键改变相机距离和方位角
            if (Input.GetMouseButton(1))
            {
                mouseY = Input.GetAxis("Mouse Y");
                mouseX = Input.GetAxis("Mouse X");

                if (mouseY > 0.01f || mouseY < -0.01f)
                {
                    ElevationAngle -= mouseY * MoveSensitivity;
                    ElevationAngle = Mathf.Clamp(ElevationAngle, MinElevationAngle, MaxElevationAngle);
                }

                if (mouseX > 0.01f || mouseX < -0.01f)
                {
                    OrbitalAngle += mouseX * MoveSensitivity;
                    if (OrbitalAngle > 360)
                        OrbitalAngle -= 360;
                    if (OrbitalAngle < 0)
                        OrbitalAngle += 360;
                }
            }

            // 移动端
            if (touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;

                if (deltaPosition.y > 0.01f || deltaPosition.y < -0.01f)
                {
                    ElevationAngle -= deltaPosition.y * 0.1f;
                    ElevationAngle = Mathf.Clamp(ElevationAngle, MinElevationAngle, MaxElevationAngle);
                }

                if (deltaPosition.x > 0.01f || deltaPosition.x < -0.01f)
                {
                    OrbitalAngle += deltaPosition.x * 0.1f;
                    if (OrbitalAngle > 360)
                        OrbitalAngle -= 360;
                    if (OrbitalAngle < 0)
                        OrbitalAngle += 360;
                }

            }

            if (touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                float prevTouchDelta = (touch0PrevPos - touch1PrevPos).magnitude;
                float touchDelta = (touch0.position - touch1.position).magnitude;

                float zoomDelta = prevTouchDelta - touchDelta;

                if (zoomDelta > 0.01f || zoomDelta < -0.01f)
                {
                    FollowDistance += zoomDelta * 0.25f;
                    FollowDistance = Mathf.Clamp(FollowDistance, MinFollowDistance, MaxFollowDistance);
                }
            }
            if (mouseWheel < -0.01f || mouseWheel > 0.01f)
            {

                FollowDistance -= mouseWheel * 5.0f;
                FollowDistance = Mathf.Clamp(FollowDistance, MinFollowDistance, MaxFollowDistance);
            }
        }
    }
}