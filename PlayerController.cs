using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    enum Directions {Up, Down, Left, Right}
    

    Directions prevDirection;      //this will store the previous direction so that the diagonal fixer knows which way to go in, the opposite of the previous direction

    public float collisionOffset = 0.05f;  

    public ContactFilter2D movementFilter;

    public SwordAttack swordAttack;

    Vector2 movementDirection;

    Vector2 movementInput; //2d vector of x and y 

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb; //rigid body component

    Animator animator; 

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); //initialises list for raycast collisions to store in

    bool canMove = true;

    //Vector2 vectorSqrt2 = new Vector2 (1.414213562373f, 1.414213562373f);   //a vector of square root 2 so that the diagonal movements are the same speed, f on the end is for float so it stops complaining, not needed anymore

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //when the script starts the rigid body component from the Player asset is gotten and stored in rb
        animator = GetComponent<Animator>();       //now we can use the animator component in our script
        spriteRenderer = GetComponent<SpriteRenderer>(); //unga bunga
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canMove)  //canMove is so that when we attack we cant move (which is a mechanic i hate)
        {
            if (movementInput == Vector2.zero)
            {
                animator.SetInteger("isMovingDirection", 5); // 5 is not moving
            }

            if (movementInput != Vector2.zero)//when the player is pressing a direction
            {
                bool success = TryMove(movementInput);

                /* part of the tutorial and it might be useful later on */

                if (!success)
                {          //if we cannot move in both directions (this whole bit is to mean that we slide if moving diagonally, it works on the fact that we dont move if we press two directions and one of them collides)
                    success = TryMove(new Vector2(movementInput.x, 0));     //try moving in just the x direction
                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));  // try moving in just the y
                    }
                }

                if (movementInput.x > 0) //moving to the right is the number 3
                {
                    spriteRenderer.flipX = false; // this should flip the right moving sprite to match the direction
                    animator.SetInteger("isMovingDirection", 3);
                }
                else if (movementInput.x < 0)  //moving left is 2
                {
                    spriteRenderer.flipX = true;  // this should flip the right moving sprite to match the direction
                    animator.SetInteger("isMovingDirection", 2);
                }
                else if (movementInput.y < 0) // moving down is 0
                {
                    animator.SetInteger("isMovingDirection", 0);
                }
                else if (movementInput.y > 0) // moving up is 1
                {
                    animator.SetInteger("isMovingDirection", 1);
                }
            }

        }

    }


    private bool TryMove(Vector2 direction) {     //function that checks if the player can move without colliding, input is a Vector2 which we are naming direction
        if (direction == Vector2.zero)
        {
                    //if there is no direction to move in then we cant move, previously if we were colliding with fence walk animation would be displayed, this isnt true for my game but in the tutorial thats what was happening
            return false;
        }
        int count = rb.Cast(   //if there is a count of 0 then there are no collisions and move is valid
            direction, // x and y value that are the direction to look for collisions in
            movementFilter, //settings which determine where a collision can occur on such as layers which are collidable
            castCollisions, // list of collisions to store the found collisions into when the cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // the amount of distance to cast + an offset to help go a bit further, can be useful apparently
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);    //fixed delta time ensures more fps does not equal faster movement, notice that movementInput makes it a 2d vector, so it can sum with rb.position, and also speed * time is distance
            return true;
        } else {
            return false;

        }
    }

    void OnMove(InputValue movementValue) {    //on move it takes the inputvalue passes it as movementValue, inputvalue is wasd and such, from the player input package 
        movementInput = movementValue.Get<Vector2>(); // converting it to a vector and storing that vector as movementInput
    }

    void OnFire()   //from the player input module, its currently bound to left click
    {
        animator.SetTrigger("swordAttack");    //sets off the trigger for sword attack, initiates el animation
    }

    public void SwordAttack()
    {
        //LockMovement();
        if(spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        } else {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        //UnlockMovement();
        swordAttack.AttackStop();
    }

    public void LockMovement()
    {
        canMove = false;
    }
    public void UnlockMovement()
    {
        canMove= true;
    }


}


