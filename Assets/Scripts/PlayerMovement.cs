using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float playerChargeJumpForce = 8.0f;		 
	private float OnGroundTimer = 1f;
	private bool playerOnGround = false;
	private int playerCurrentJumps = 0;
	private int playerMaxJumpCombo = 3;
    private bool PlayerLongJumping = false;
	private float TotalPlayerOnGroundTime = 1f;

    public float JumpComboDecayRate = 0.15f;
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
        if (playerOnGround == false)
        {
            transform.Translate(playerHorizontalMovement, 0, 0);
        }
        else
        {
            transform.Translate(playerHorizontalMovement, 0, 0);
        }

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

            if (playerChargeJumpForce >= PlayerJumpMinHeight)
            {
                PlayerLongJumping = true;
            }

			Jump (playerHorizontalMovement);
			OnGroundTimer = TotalPlayerOnGroundTime;

			if (playerCurrentJumps == playerMaxJumpCombo) 
			{
                print("Jump Reset");
				playerCurrentJumps = 0;
			}
		
		}
	}
		
	void FixedUpdate()
	{

		if (playerOnGround == true) 
		{
            OnGroundTimer -= JumpComboDecayRate;
			if (OnGroundTimer <= 0 && playerCurrentJumps >= 1) 
			{
                print("Jump Reset because of TimeOut");
				playerCurrentJumps = 0;
			}
		}
			

	}
		
	void Jump(float HorizontalMotion)
		{
		if (playerOnGround == true)
			{
			playerOnGround = false;
			OnGroundTimer = TotalPlayerOnGroundTime;
            if (playerCurrentJumps == 0)
            {
                if (playerChargeJumpForce >= PlayerJumpMaxHeight)
                {
                    Mathf.Clamp(playerChargeJumpForce, 0, PlayerJumpMaxHeight);
                    playerCurrentJumps++;
                    rb.velocity = new Vector3(0, PlayerJumpMaxHeight, 0);
                    playerChargeJumpForce = PlayerJumpMinHeight;
                }

                else
                {
                    playerCurrentJumps++;
                    rb.velocity = new Vector3(0, playerChargeJumpForce, 0);
                    playerChargeJumpForce = PlayerJumpMinHeight;

                }
            }
            else
            {
                if (playerChargeJumpForce >= PlayerJumpMaxHeight)
                {
                    Mathf.Clamp(playerChargeJumpForce, 0, PlayerJumpMaxHeight);
                    playerCurrentJumps++;
                    if (PlayerLongJumping == true)
                    {
                        rb.velocity = new Vector3(0, (PlayerJumpMaxHeight * (playerCurrentJumps / 2.5f)), 0);
                        playerChargeJumpForce = PlayerJumpMinHeight;
                        PlayerLongJumping = false;
                    }
                    else
                    { 
                        rb.velocity = new Vector3(0, (PlayerJumpMaxHeight * (playerCurrentJumps / 1.5f)), 0);
                        playerChargeJumpForce = PlayerJumpMinHeight;
                    }
                }

                else
                {
                    playerCurrentJumps++;
                    if (PlayerLongJumping == true)
                    {
                        rb.velocity = new Vector3((HorizontalMotion * 80), (PlayerJumpMaxHeight * (playerCurrentJumps / 2.5f)), 0);
                        playerChargeJumpForce = PlayerJumpMinHeight;
                        PlayerLongJumping = false;
                    }
                    else
                    {
                        rb.velocity = new Vector3(0, (playerChargeJumpForce * (playerCurrentJumps / 1.5f)), 0);
                        playerChargeJumpForce = PlayerJumpMinHeight;
                    }

                }
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