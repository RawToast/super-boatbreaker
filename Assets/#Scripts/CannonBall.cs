using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using Util;

public class CannonBall : MonoBehaviour {
    
    public float speed;

    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody2D rig;
    
    //this sets the width of walls, these wont move the scene walls YET so just dont fuck with it yet - carlo 
    [SerializeField] private float wallBounds = 5f;

    [SerializeField] private int onhitSpinDivisor;
    
    [SerializeField] private float airSpinDivisor;

    public Vector2 direction;
    private bool goingLeft;
    private bool goingDown;

    private float spin = 0;


    // There has to be a better way of doing this (using the values from the prefab?)
    public void SetValuesToDefaults() {
        direction = Vector2.down;
        spin = 0f;
        goingDown = true;
        goingLeft = false;
        onhitSpinDivisor = 80;
        airSpinDivisor = 1.003f;
    }
    
    public void Start() {
        PlaceBallInCenterHeadingDown();
       
        StartCoroutine("ResetBallWaiter");
    }
    
    void Update() {
//        print("Direction: " + direction + " spin: " + spin + " div: " + onhitSpinDivisor);
        direction = new Vector2(direction.x + (spin / onhitSpinDivisor), direction.y).normalized;
        rig.velocity = direction * speed * Time.deltaTime;
        
        ReduceSpin(airSpinDivisor);
        
        if (transform.position.x > wallBounds && !goingLeft) {
            //Is the ball at the right border and is not going left (heading towards the right border)
            direction = new Vector2(-direction.x, direction.y + (spin / onhitSpinDivisor));
            ReduceSpin(1.8f);

            goingLeft = true;
        }
        
        if (transform.position.x < -wallBounds && goingLeft) {
            //Is the ball at the left border and is going left (heading towards the left border)
            direction = new Vector2(-direction.x, direction.y - (spin / onhitSpinDivisor));
            ReduceSpin(1.8f);
            goingLeft = false;
        }
        
        if (transform.position.y > 16 && !goingDown) {
            //Is the ball at the top border and not going down (heading towards the top border)
            direction = new Vector2(direction.x + (spin / onhitSpinDivisor), -direction.y);
            ReduceSpin(1.8f);
            goingDown = true;
        }
        
        if (transform.position.y < -5) {
            //This works but may take a long time, the paddle is not always at y 0
            Destroy(this);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag(Tags.BOAT_PADDLE)) {
            Vector2 ndir = NormalizedDifference(coll.collider);

            var contacts = coll.contacts[0].normal;
            
            IncreaseSpeed();

            var standardBounceDir = new Vector2(direction.x + (ndir.x / 2), ndir.y).normalized;

            if (contacts.x <= -0.7f && !goingLeft) {
                standardBounceDir = new Vector2(-standardBounceDir.x, standardBounceDir.y + (spin / onhitSpinDivisor));
            }
            else if (contacts.x >= 0.7f && goingLeft) {
                standardBounceDir = new Vector2(-standardBounceDir.x, standardBounceDir.y + (spin / onhitSpinDivisor));

            }
//1.0, -0.1
            // Spin
            var boat = coll.collider.GetComponent<Boat>();
            ReduceSpin();
            spin = spin + (boat.velocity().x / 6);

            var finalVelocity = new Vector2(
                standardBounceDir.x + spin, 
                standardBounceDir.y).normalized;

            UpdateGoingLeftDown(finalVelocity);
            direction = finalVelocity;
            
        } else if (coll.gameObject.CompareTag(Tags.BLOCK)) {
            var contacts = coll.contacts[0].normal;

            
            IncreaseSpeed();

            var blk = coll.collider.gameObject.GetComponent<Block>();

            if (Math.Abs(contacts.x - contacts.y) < 0.1f|| Math.Abs(contacts.x + contacts.y) < 0.1f) {
                direction = -direction;
                blk.TakeDamage(this);
            } else {
                if (contacts.y >= 0.7f && goingDown) {
                    direction = new Vector2(direction.x + (spin / onhitSpinDivisor), -direction.y);
                    blk.TakeDamage(this);
                }
                else if (contacts.y <= -0.7f && !goingDown) {
                    direction = new Vector2(direction.x + (spin / onhitSpinDivisor), -direction.y);
                    blk.TakeDamage(this);
                }
                if (contacts.x <= -0.7f && !goingLeft) {
                    direction = new Vector2(-direction.x, direction.y + (spin / onhitSpinDivisor));
                    blk.TakeDamage(this);
                }
                else if (contacts.x >= 0.7f && goingLeft) {
                    direction = new Vector2(-direction.x, direction.y + (spin / onhitSpinDivisor));
                    blk.TakeDamage(this);
                }
            }

            ReduceSpin();
            UpdateGoingLeftDown(direction);

        } else if (coll.gameObject.CompareTag(Tags.CANNONBALL)) {
            var contacts = coll.contacts[0].normal;

            IncreaseSpeed();
            

            if (contacts.y >= 0.8f && goingDown) {
                direction = new Vector2(direction.x + (spin / onhitSpinDivisor), -direction.y);
            }
            else if (contacts.y <= -0.8f && !goingDown) {
                direction = new Vector2(direction.x + (spin / onhitSpinDivisor), -direction.y);
            }

            if (contacts.x <= -0.8f && !goingLeft) {
                direction = new Vector2(-direction.x, direction.y + (spin / onhitSpinDivisor));
            }
            else if (contacts.x >= 0.8f && goingLeft) {
                direction = new Vector2(-direction.x, direction.y + (spin / onhitSpinDivisor));

            }

            ReduceSpin();
            UpdateGoingLeftDown(direction);

        } 
    } 

    private Vector2 NormalizedDifference(Collider2D otherCollider2D) {
        return (gameObject.transform.position - otherCollider2D.transform.position).normalized;
    }

    private void IncreaseSpeed() {
        var newSpeed = speed + 3;
        if (maxSpeed >= newSpeed)
            speed = newSpeed;
    }

    private void ReduceSpin(float divisor = 3f) {
        if (0.01 > spin && spin >= 0)
            spin = 0;
        else if (-0.01 < spin && spin <= 0)
            spin = 0;
        else
            spin = spin / divisor;
    }
   
//    //Called when the ball goes underneath the paddle and "dies"
//    public void ResetBall() {
//        transform.position = Vector3.zero; //put ball in the middle of the screen
//        direction = Vector2.down;
//        StartCoroutine("ResetBallWaiter");
//
//        //manager.LiveLost(); //Calls the 'LiveLost()' function in the GameManager function
//        //sacked this off for now as remaking gamemanager - c
//    }

    
    private void PlaceBallInCenterHeadingDown() {
        //transform.position = Vector3.zero;
        direction = Vector2.down;
    }
    
    /**
     * Called to make the ball wait a second before moving. This is also called when the ball dies
     * and is respawned at the middle of the screen
     */
    private IEnumerator ResetBallWaiter() {
        speed = 0;
        spin = 0;
        yield return new WaitForSeconds(0.2f);
        speed = 200;
    }
    
    private void UpdateGoingLeftDown(Vector2 d) {
        var headingRight = d.x >= 0;
        var headingLeft = d.x < 0;
        var headingUp = d.y >= 0;
        var headingDown = d.y < 0;
        
        if (headingRight)
            goingLeft = false;
        if (headingLeft)
            goingLeft = true;
        if (headingUp)
            goingDown = false;
        if (headingDown)
            goingDown = true;
    }
}