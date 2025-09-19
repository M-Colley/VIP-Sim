using UnityEngine;

public class FitPlaneToCameraView : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera

    private Transform planeTransform; // Reference to the plane's transform
    private MeshRenderer planeRenderer;

    private Vector3 lastCameraPosition;
    private Quaternion lastCameraRotation;
    private float lastCameraFieldOfView;
    private float lastCameraAspect;
    private int lastScreenWidth;
    private int lastScreenHeight;
    private Vector3 lastPlaneScale;
    private Vector3 lastPlanePosition;
    private int lastChildCount;
    private bool hasValidSetup;

    void Start()
    {
        InitialisePlane(true);
        CacheState();
        UpdatePlaneIfNeeded(true);
    }

    void LateUpdate()
    {
        UpdatePlaneIfNeeded();
    }

    private void UpdatePlaneIfNeeded(bool forceUpdate = false)
    {
        if (!hasValidSetup || transform.childCount != lastChildCount)
        {
            InitialisePlane(false);
            if (!hasValidSetup)
            {
                return;
            }
            forceUpdate = true;
        }

        bool cameraChanged = mainCamera.transform.position != lastCameraPosition ||
                             mainCamera.transform.rotation != lastCameraRotation ||
                             !Mathf.Approximately(mainCamera.fieldOfView, lastCameraFieldOfView) ||
                             !Mathf.Approximately(mainCamera.aspect, lastCameraAspect);

        bool screenChanged = Screen.width != lastScreenWidth || Screen.height != lastScreenHeight;

        bool planeChanged = planeTransform.hasChanged ||
                            planeTransform.position != lastPlanePosition ||
                            planeTransform.lossyScale != lastPlaneScale ||
                            transform.childCount != lastChildCount;

        if (forceUpdate || cameraChanged || planeChanged || screenChanged)
        {
            FitPlane();
            CacheState();
        }
    }

    private void InitialisePlane(bool logErrors)
    {
        planeTransform = transform.childCount > 0 ? transform.GetChild(0) : null;
        lastChildCount = transform.childCount;

        if (planeTransform == null)
        {
            hasValidSetup = false;
            if (logErrors)
            {
                Debug.LogError("FitPlaneToCameraView requires the parent to have at least one child containing the plane.");
            }
            return;
        }

        planeRenderer = planeTransform.GetComponent<MeshRenderer>();
        if (planeRenderer == null)
        {
            hasValidSetup = false;
            if (logErrors)
            {
                Debug.LogError("Plane does not have a MeshRenderer component.");
            }
            return;
        }

        if (mainCamera == null)
        {
            hasValidSetup = false;
            if (logErrors)
            {
                Debug.LogError("Main camera is not assigned in FitPlaneToCameraView.");
            }
            return;
        }

        hasValidSetup = true;
    }

    private void CacheState()
    {
        if (!hasValidSetup)
        {
            return;
        }

        lastCameraPosition = mainCamera.transform.position;
        lastCameraRotation = mainCamera.transform.rotation;
        lastCameraFieldOfView = mainCamera.fieldOfView;
        lastCameraAspect = mainCamera.aspect;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        lastPlaneScale = planeTransform.lossyScale;
        lastPlanePosition = planeTransform.position;
        lastChildCount = transform.childCount;
        planeTransform.hasChanged = false;
    }

    private void FitPlane()
    {
        if (!hasValidSetup)
        {
            return;
        }

        Bounds planeBounds = planeRenderer.bounds;
        float planeHeight = planeBounds.size.y;
        float planeWidth = planeBounds.size.x;

        float halfFovRadians = mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float tanHalfFov = Mathf.Tan(halfFovRadians);
        float requiredDistanceHeight = planeHeight / (2.0f * tanHalfFov);
        float requiredDistanceWidth = planeWidth / (2.0f * tanHalfFov / mainCamera.aspect);
        float requiredDistance = Mathf.Max(requiredDistanceHeight, requiredDistanceWidth);

        Vector3 direction = Vector3.forward;
        transform.position = mainCamera.transform.position + direction * requiredDistance;
    }
}
