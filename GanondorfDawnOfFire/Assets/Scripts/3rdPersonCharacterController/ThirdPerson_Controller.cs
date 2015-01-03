using UnityEngine;
using System.Collections;

public class ThirdPerson_Controller : MonoBehaviour 
{
    public static CharacterController CharacterController;

    public static ThirdPerson_Controller Instance;

    void Awake()
    {
        assignReferences();

        initializeCamera();
    }

    private void initializeCamera()
    {
        ThirdPerson_Camera.AssignMainCamera();
    }

    private void assignReferences()
    {
        CharacterController = GetComponent<CharacterController>();

        Instance = this;
    }
	
	void Update() 
    {
        if(cameraExists())
        {
            getLocomotionInput();
            handleActionInput();
            updateThirdPersonMotor();
        }
	}

    private void updateThirdPersonMotor()
    {
        ThirdPerson_Motor.Instance.UpdateMotor();
    }

    private void getLocomotionInput()
    {
        var deadZone = 0.1f;

        ThirdPerson_Motor.Instance.VerticalVelocity = ThirdPerson_Motor.Instance.Move.y;
        ThirdPerson_Motor.Instance.Move = Vector3.zero;

        //TODO test with math.abs
        if (Input.GetAxis("Vertical") > deadZone || Input.GetAxis("Vertical") < -deadZone)
            ThirdPerson_Motor.Instance.Move += new Vector3(0, 0, Input.GetAxis("Vertical"));

        if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone)
            ThirdPerson_Motor.Instance.Move += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    }

    private void handleActionInput()
    {
        //if (Input.GetButton("Jump"))
        //    jump();
    }

    private void jump()
    {
        ThirdPerson_Motor.Instance.Jump();
    }

    private bool cameraExists()
    {
        if (Camera.main == null)
            return false;
        else
            return true;
    }
}
