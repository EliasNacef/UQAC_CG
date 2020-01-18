using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController controller;
    public Animator animator;

    public float mapXMin;
    public float mapYMin;
    public float mapXMax;
    public float mapYMax;

    public bool canMove;
    private float movingX;
    private float movingY;
    private Vector3 currentPosition;
    private Vector3 futurePosition;

    public float runSpeed = 1f;

	float horizontalMove = 0f;
    float verticalMove = 0f;


    private void Start()
    {
        canMove = false;
        movingX = 0;
        movingY = 0;
        currentPosition = this.transform.position;
        futurePosition = this.transform.position;
        StartCoroutine(ChangeCell());
    }


    // Update is called once per frame
    void Update () {


        if (canMove)
        {
            runSpeed = 1f;
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

	}

    void OnLosing()
    {
        //TODO : Make the player go to replace the spectater (animation/animator)
    }

    public IEnumerator ChangeCell()
    {
        while(true)
        {
            var move = new Vector2(horizontalMove, verticalMove);
            currentPosition = this.transform.position;   
            if (move.x != 0)
            {
                movingX = Mathf.Sign(move.x);
            }
            else if (move.y != 0)
            {
                movingY = Mathf.Sign(move.y);
            }

            if (movingX != 0)
            {
                if(currentPosition.x + movingX < mapXMax && currentPosition.x + movingX > mapXMin) transform.position = new Vector3(currentPosition.x + movingX, currentPosition.y, currentPosition.z);
                movingX = 0;
                yield return new WaitForSeconds(0.2f);

            }
            else if (movingY != 0)
            {
                if (currentPosition.y + movingY < mapYMax && currentPosition.y + movingY > mapYMin) transform.position = new Vector3(currentPosition.x, currentPosition.y + movingY, currentPosition.z);
                movingY = 0;
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForEndOfFrame();
        }
    }


        void FixedUpdate ()
	{
        // Move our character

        /*currentPosition = this.transform.position;
        var move = new Vector2(horizontalMove, verticalMove);
        if(move.x != 0)
        {
            movingX = true;
            futurePosition = this.transform.position;
            futurePosition.x += Mathf.Sign(move.x);
        }
        else if(move.y != 0)
        {
            movingY = true;
            futurePosition = this.transform.position;
            futurePosition.y += Mathf.Sign(move.y);

        }

        if (movingX)
        {
            //controller.Move(new Vector2(runSpeed, 0.0f) * Time.fixedDeltaTime);
            if (currentPosition == futurePosition)
            {
                movingX = false;
                movingY = false;
            }
        }
        else if (movingY)
        {
            //controller.Move(new Vector2(0.0f, runSpeed) * Time.fixedDeltaTime);
            if(currentPosition == futurePosition)
            {
                movingX = false;
                movingY = false;
            }
        }
        Debug.Log("Move" + move.ToString());
        Debug.Log("Future" + futurePosition.ToString());
        Debug.Log("Current" + currentPosition.ToString());*/


    }
}
