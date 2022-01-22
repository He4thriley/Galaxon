using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoming : MonoBehaviour
{
   
    private Transform target;
    [SerializeField]
    private string targetTag;
    public Rigidbody2D rigidBody;
    public float angleChangingSpeed;
    public float movementSpeed;
    public bool playerExists = true;

   

    private void Start()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (target == null)
        {
           
        }
        playerExists = true;

    }


    void Update()
    { 
        if (playerExists == false)
        {
            Destroy(this.gameObject);
         
        }
        else if (playerExists == true)
        {
            HomingMovement();
        }


    }


    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
            Destroy(this.gameObject);
        }
    }

    public void HomingMovement()
    {
        Vector2 direction = (Vector2)target.position - rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = angleChangingSpeed * rotateAmount;
        Vector3 transformDown = transform.up * (-1);
        rigidBody.velocity = transformDown * movementSpeed;



        if (transform.position.y > 7 || transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.x > 11.3f || transform.position.x < -11.3f)
        {
            Destroy(this.gameObject);
        }
    }



}
