using UnityEngine;
using System.Collections;

public class ThirdPerson_Camera : MonoBehaviour 
{
    public static ThirdPerson_Camera Instance;

    private Transform TargetLookAt;

    public float Distance = 6.0f;
    public float DistanceMin = 3.0f;
    public float DistanceMax = 10.0f;
    public float DistanceSmooth = 0.05f;
    public float DistanceResumeSmooth = 1.0f;
    private float velocityDistance; //Set to 0f

    private float startDistance; //Distance after Validation of Distance
    private float desiredDistance;
    private float distanceSmotth = 0.0f;
    private float preOccludedDistance = 0.0f;
    private Vector3 desiredPosition = Vector3.zero;
    private Vector3 position = Vector3.zero;

    public float X_MouseSensitivity = 5.0f;
    public float Y_MouseSensitivity = 5.0f;
    private float mouseX; //Rotates camera left or right
    private float mouseY; //Rotates camera up or down

    //TODO: Make nullable in case camera can't zoom
    public float MouseWheel_Sensitivity = 5.0f;
    public float Y_MinLimit = -40.0f;
    public float Y_MaxLimit = 80.0f;

    public float OcclusionDistanceStep = 0.5f;
    public int MaxOcclusionChecks = 10;

    public float X_Smooth = 0.05f;
    public float Y_Smooth = 0.1f;
    private float velocityX = 0.0f;
    private float velocityY = 0.0f;
    private float velocityZ = 0.0f;

    public enum LookControls { UNLOCKED, LOCKED };
    public LookControls CameraControl = LookControls.LOCKED;

    public bool InvertMouseLook = false;

    public bool DebugLines = true;

	void Awake() 
    {
        assignReferences();
	}

    private void assignReferences()
    {
        Instance = this;
    }

    public static void AssignMainCamera()
    {
        GameObject tempCamera;
        if (Camera.main != null)
            tempCamera = Camera.main.gameObject;
        else
        {
            tempCamera = new GameObject("MainCamera");
            tempCamera.AddComponent<Camera>();
            tempCamera.tag = "MainCamera";
        }
        
        if (!tempCamera.GetComponent<ThirdPerson_Camera>())
            tempCamera.AddComponent<ThirdPerson_Camera>();

        assignTargetLookAt(tempCamera);
    }

    private static void assignTargetLookAt(GameObject tempCamera)
    {
        GameObject targetLookAt = GameObject.Find("targetLookAt");
        if(targetLookAt == null)
        {
            targetLookAt = new GameObject("targetLookAt");
            targetLookAt.transform.position = Vector3.zero;
        }

        ThirdPerson_Camera myCamera = tempCamera.GetComponent<ThirdPerson_Camera>();
        myCamera.TargetLookAt = targetLookAt.transform;
    }

    void Start()
    {
        //Validates Input

        //TODO: Remove and push into Reset() if it makes sense in the end
        Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
        startDistance = Distance;

        Reset();
    }

    //Sets Initial Values and Resets camera if character dies, etc
    public void Reset()
    {
        mouseX = 0.0f; //behind character
        mouseY = 10.0f; //Just above character

        //TODO: See if refactoring desiredDistance to be local to handlePlayerInput makes line below irrelevant
        desiredDistance = preOccludedDistance = Distance = startDistance; //Makes sure smoothing doesn't begin immediately
    }

    void LateUpdate()
    {
        //Look for a TargetLookAt and make sure it isn't null
        if (TargetLookAt == null)
            return;
       
        handlePlayerInput();

        var count = 0;
        do
        {
            calculateDesiredPosition();
            count++;
        } while (CheckIfOccluded(count));
        
        updatePosition();
    }

    //Processes Smoothing for camera movement
    private void updatePosition()
    {
        //Done this way to allow the x and y position to smooth independently
        var posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velocityX, X_Smooth);
        var posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velocityY, Y_Smooth);
        var posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velocityZ, X_Smooth);
        position = new Vector3(posX, posY, posZ);

        transform.position = position;

        transform.LookAt(TargetLookAt);
    }

    bool CheckIfOccluded(int count)
    {
        var isOccluded = false;

        var nearestDistance = checkCameraPoints(TargetLookAt.position, desiredPosition);

        if (nearestDistance != -1)
        {
            if (count < MaxOcclusionChecks)
            {
                isOccluded = true;
                Distance -= OcclusionDistanceStep;

                if (Distance < 0.25f)
                    Distance = 0.25f;
            }
            else
            {
                Distance = nearestDistance - Camera.main.nearClipPlane;
            }

            //Jumps camera to position without smoothing
            desiredDistance = Distance;
            distanceSmotth = DistanceResumeSmooth;
        }

        return isOccluded;
    }

    private float checkCameraPoints(Vector3 from, Vector3 to)
    {
        var nearestDistance = -1.0f;

        RaycastHit hitInfo;

        Helper.ClipPlane clipPlane = Helper.ClipPaneAtNear(to);

        if (DebugLines)
        {
            viewLines(from, to, clipPlane);
            clipPlaneLines(clipPlane);
        }

        if (Physics.Linecast(from, clipPlane.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player")
            nearestDistance = hitInfo.distance;
        if (Physics.Linecast(from, clipPlane.LowerLeft, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }
        if (Physics.Linecast(from, clipPlane.UpperRight, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }
        if (Physics.Linecast(from, clipPlane.LowerRight, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }
        if (Physics.Linecast(from, to + transform.forward * -camera.nearClipPlane, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }  

        return nearestDistance;
    }

    private void clipPlaneLines(Helper.ClipPlane clipPlane)
    {
        Debug.DrawLine(clipPlane.UpperLeft, clipPlane.UpperRight, Color.yellow);
        Debug.DrawLine(clipPlane.UpperRight, clipPlane.LowerRight, Color.yellow);
        Debug.DrawLine(clipPlane.LowerRight, clipPlane.LowerLeft, Color.yellow);
        Debug.DrawLine(clipPlane.LowerLeft, clipPlane.UpperLeft, Color.yellow);
    }

    private void viewLines(Vector3 from, Vector3 to, Helper.ClipPlane clipPlane)
    {
        Debug.DrawLine(from, to + transform.forward * -camera.nearClipPlane, Color.red);
        Debug.DrawLine(from, clipPlane.UpperLeft, Color.green);
        Debug.DrawLine(from, clipPlane.UpperRight, Color.green);
        Debug.DrawLine(from, clipPlane.LowerRight, Color.green);
        Debug.DrawLine(from, clipPlane.LowerLeft, Color.green);
    }

    //Uses data from HandlePlayerInput to update Position
    private void calculateDesiredPosition()
    {
        resetDesiredDistance();
        //Smoothes the curve the camera is moving along
        Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velocityDistance, distanceSmotth);

        desiredPosition = calculatePosition(mouseY, mouseX, Distance);
    }

    //Converts from mouse space to 3D space
    private Vector3 calculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0); //TODO: play with rolling the camera by setting the z value
        return (TargetLookAt.position + (rotation * direction));
    }

    private void resetDesiredDistance()
    {
        if(desiredDistance < preOccludedDistance)
        {
            var pos = calculatePosition(mouseY, mouseX, preOccludedDistance);

            var nearestDistance = checkCameraPoints(TargetLookAt.position, pos);

            if (nearestDistance == -1 || nearestDistance > preOccludedDistance)
                desiredDistance = preOccludedDistance;
        }
    }

    //Handles Player Input, Mouse Input, and processing input
    void handlePlayerInput()
    {
        var deadZone = 0.01f;

        if (Input.GetMouseButton(1) && CameraControl == LookControls.LOCKED)
        {
            //takes mouse input
            mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
            //look();
        }
        else if(CameraControl == LookControls.UNLOCKED)
        {
            //look();
        }

        mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

        //Applies sensitivity value of mouse wheel
//        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
//        {
//            desiredDistance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheel_Sensitivity,
//                                    DistanceMin, DistanceMax);
//            preOccludedDistance = desiredDistance;
//            distanceSmotth = DistanceSmooth;
//        }
            
        
        //Apply mouseWheel_sensitivity
        //Subtact the value from current distance
        //clamps this information at the end
    }

    //TODO: Make Return vector 2 at end once everything else is working.
    private float look()
    {
        if(InvertMouseLook)
        {
            //Multiply mouse values by -1
        }

        return 0.0f;
    }
}
