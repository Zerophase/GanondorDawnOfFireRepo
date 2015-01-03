using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
	public float MaxSpeed = 10.0f;
	private bool facingRight = true;

	private Animator anim;

	private bool grounded = false;
	public Transform GroundCheck;
	private float groundRadius = 0.125f;
	public LayerMask WhatIsGround;
	public float JumpForce = 700;

	bool doubleJump = false;

	public Transform WallCheck;
	private bool walled;
	public LayerMask WhatIsWall;
	private float wallRadius = .5f;

	float move;

	float distanceVertical = .7f;
	float distanceHorizontal = .5f;

	void Start () 
	{
		anim = GetComponent<Animator>();

	}

	void Update()
	{
		if((grounded  || !doubleJump || walled) && Input.GetKeyDown(KeyCode.Space))
		{
			if((!grounded && walled) || (grounded && walled)) //&& (move > 0 || move < 0)))
			{ 
				anim.SetBool("Wall", false);
				Debug.Log("Wall Jump.");
				if(facingRight)
  					rigidbody2D.AddForce(new Vector2(-150, JumpForce));
				else if(!facingRight)
      					rigidbody2D.AddForce(new Vector2(150, JumpForce));

				flip ();
			}
			else
			{
				Debug.Log("Normal Jump.");
				anim.SetBool("Ground", false);
				rigidbody2D.AddForce(new Vector2(0, JumpForce));

				if(!doubleJump && !grounded)
					doubleJump = true;
			}
		}
	}

	void FixedUpdate () 
	{
		walled = Physics2D.OverlapCircle(WallCheck.position, wallRadius, WhatIsWall);
		anim.SetBool("Wall", walled); 

		grounded = Physics2D.OverlapCircle(GroundCheck.position, groundRadius, WhatIsGround);
		//Added to see if I can use a raycast to fix bug with standing on walls
		Debug.DrawRay(new Vector2(transform.position.x + .4f, transform.position.y), Vector2.up * -1  * distanceVertical, Color.black);
		Debug.DrawRay(new Vector2(transform.position.x - .4f, transform.position.y), Vector2.up * -1  * distanceVertical, Color.black);
		Debug.DrawRay(transform.position, Vector2.right * distanceHorizontal, Color.blue);
		if(!grounded && (Physics2D.Raycast(new Vector2(transform.position.x + .4f, transform.position.y), Vector2.up * -1, distanceVertical, WhatIsGround) 
         	|| Physics2D.Raycast(new Vector2(transform.position.x + .4f, transform.position.y), Vector2.up * -1, distanceVertical, WhatIsWall) 
         	|| Physics2D.Raycast(new Vector2(transform.position.x - .4f, transform.position.y), Vector2.up * -1, distanceVertical, WhatIsWall) 
         	|| Physics2D.Raycast(new Vector2(transform.position.x - .4f, transform.position.y), Vector2.up * -1, distanceVertical, WhatIsGround)) 
		   	&& (!Physics2D.Raycast(transform.position, Vector2.right, distanceHorizontal, WhatIsWall)
		    || !Physics2D.Raycast(transform.position, Vector2.right * -1, distanceHorizontal, WhatIsWall)))
		{
			grounded = true;
		}

		anim.SetBool("Ground", grounded);

		if(grounded)
		{
			doubleJump = false;
			walled = false;
		}
			

		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

		if(!grounded)
			return;

		move = Input.GetAxis("Horizontal");

		anim.SetFloat("Speed", Mathf.Abs(move));

		rigidbody2D.velocity = new Vector2(move * MaxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && !facingRight)
			flip();
		else if(move < 0 && facingRight)
			flip();
	}

	void flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
