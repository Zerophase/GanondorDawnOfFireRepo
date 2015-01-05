using UnityEngine;
using System.Collections;

public class ThirdPerson_Motor : MonoBehaviour 
{
    public static ThirdPerson_Motor Instance;

    public float MoveSpeed = 10f;
    
    public float JumpSpeed = 6f;
    public float Gravity = 21f;
    public float TerminalVelocity = 20f;

    public float SlideThreshold = 0.6f;
    public float MaxControllableSlideMag = 0.4f;

    private Vector3 slideDirection;

    public Vector3 Move { get; set; }
    public float VerticalVelocity { get; set; }

	void Awake() 
    {
        assignReferences();
	}
	
    private void assignReferences()
    {
        Instance = this;
    }

	public void UpdateMotor() 
    {
        snapCharacterToCameraFacing();

        processMotion();
	}

    private void processMotion()
    {
        Move = transform.TransformDirection(Move);

        if (Move.magnitude > 1)
            Move = Vector3.Normalize(Move);

        Move *= MoveSpeed;
        Move = new Vector3(Move.x, VerticalVelocity, Move.z);

        applyGravity();
        ThirdPerson_Controller.CharacterController.Move(Move * Time.deltaTime);
    }

    private void applyGravity()
    {
        if (Move.y > -TerminalVelocity)
            Move = new Vector3(Move.x, Move.y - Gravity * Time.deltaTime, Move.z);

        if (ThirdPerson_Controller.CharacterController.isGrounded && Move.y < -1)
            Move = new Vector3(Move.x, -1, Move.z);
    }

    private void applySlide()
    {
        if (ThirdPerson_Controller.CharacterController.isGrounded)
            return;

        slideDirection = Vector3.zero;

        RaycastHit hitInfo;

        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo))
        {
            if (hitInfo.normal.y < SlideThreshold)
                slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);

        }

        if (slideDirection.magnitude < MaxControllableSlideMag)
            Move += slideDirection;
        else
            Move = slideDirection;
    }

    public void Jump()
    {
        if (ThirdPerson_Controller.CharacterController.isGrounded)
            VerticalVelocity = JumpSpeed;
    }

    //TODO see if snapCharacterToCamerFacing should go in ThirdPerson_Camera
    private void snapCharacterToCameraFacing()
    {
        //if move vector x or z != 0 rotate character to camera facing
        if(Move.x != 0 || Move.z != 0)
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 
                Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
