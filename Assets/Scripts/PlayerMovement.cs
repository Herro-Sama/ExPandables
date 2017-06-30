using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float playerChargeJumpForce = 8.0f;		 
	private float OnGroundTimer = 0.5f;
	private bool playerOnGround = false;
	private int playerCurrentJumps = 0;
	private int playerMaxJumpCombo = 3;

	public float TotalPlayerOnGroundTime = 0.5f;
	public float PlayerSpeed = 10.0f;
	public float PlayerJumpChargeSpeed = 0.2f;
	public float PlayerJumpMinHeight = 8.0f;
	public float PlayerJumpMaxHeight = 15.0f;




	public Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update ()
	{
		float playerHorizontalMovement = Input.GetAxis ("Horizontal");


		playerHorizontalMovement *= Time.deltaTime * PlayerSpeed;
		transform.Translate (playerHorizontalMovement, 0, 0);

		if (Input.GetButton ("Jump")) 
		{
			playerChargeJumpForce += PlayerJumpChargeSpeed;

		}
			
		if (Input.GetButtonUp ("Jump")) 
		{
			if (OnGroundTimer == 0) 
			{
				playerCurrentJumps = 0;
				OnGroundTimer = TotalPlayerOnGroundTime;
			}
			Jump ();
			OnGroundTimer = TotalPlayerOnGroundTime;

			if (playerCurrentJumps == playerMaxJumpCombo) 
			{
				playerCurrentJumps = 0;
			}
		
		}
	}
		
	void FixedUpdate()
	{

		if (playerOnGround == true) 
		{
			OnGroundTimer -= Time.fixedDeltaTime;
			if (OnGroundTimer <= 0 && playerCurrentJumps >= 1) 
			{
				print ("Jumping Reset");
				playerCurrentJumps = 0;
			}
		}
			

	}
		
	void Jump()
		{
		if (playerOnGround == true)
			{
			playerOnGround = false;
			OnGroundTimer = TotalPlayerOnGroundTime;
				
			if (playerChargeJumpForce >= PlayerJumpMaxHeight)
			{
				Mathf.Clamp (playerChargeJumpForce, 0, PlayerJumpMaxHeight);
				playerCurrentJumps++;
				rb.velocity = new Vector3 (0, (PlayerJumpMaxHeight * playerCurrentJumps), 0);
				playerChargeJumpForce = PlayerJumpMinHeight;
			}
				
			else 
			{
				playerCurrentJumps++;		
				rb.velocity = new Vector3 (0, (playerChargeJumpForce*playerCurrentJumps), 0);
				playerChargeJumpForce = PlayerJumpMinHeight;

			}
			
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground") 
		{
			playerOnGround = true;
		} 

	}

	//End of Script		
}