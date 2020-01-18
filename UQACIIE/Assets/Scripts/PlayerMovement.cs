using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController controller;
    public Animator animator;

    public bool canMove;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
    float verticalMove = 0f;

    bool jump = false;
	bool crouch = false;



    private void Start()
    {
        canMove = true;
    }


    // Update is called once per frame
    void Update () {


        if (canMove)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove) + Mathf.Abs(verticalMove));
        }
        else
        {
            horizontalMove = 0f;
            verticalMove = 0f;
            animator.SetFloat("Speed", 0f);
        }
        /*if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}*/

		/*if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}*/

	}

    void OnLosing()
    {
        //TODO : Make the player go to replace the spectater (animation/animator)
    }


	void FixedUpdate ()
	{
        // Move our character
        var move = new Vector2(horizontalMove, verticalMove);
        controller.Move(move * Time.fixedDeltaTime, crouch, jump);
        jump = false;
	}
}
