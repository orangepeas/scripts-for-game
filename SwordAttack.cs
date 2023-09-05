using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    Vector2 rightAttackOffset;   
    public Collider2D swordCollider;
    public float damage = 3;

    private void Start()
    {
        rightAttackOffset = transform.position;    //base value of where the hitbox is
    }


    public void AttackRight()
    {
        print("Attack Right");
        swordCollider.enabled = true;
    }
    public void AttackLeft()
    {
        print("Attack Left");
        swordCollider.enabled = true;                  
        transform.position = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);   //flips the x axis and then places in the same y axis of the hitbox
    }
    public void AttackStop()
    {
        swordCollider.enabled = false;
    }

    

    private void OnTriggerEnter2D(Collider2D other)   //pass in a collider named "other", which is the enemy that is hit
    {
        if (other.tag  == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();//deal damage to enemy

            if (enemy != null)   
            {
                enemy.Health -= damage;
            }
        }
    }
}
