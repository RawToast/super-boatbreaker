using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour {
    
    public float speed;

    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody2D rig;
    
    //this sets the width of walls, these wont move the scene walls YET so just dont fuck with it yet - carlo 
    [SerializeField] private float wallBounds = 5f;

    [SerializeField] private int airSpinDivisor;
    
    private Vector2 direction;
    private bool goingLeft;
    private bool goingDown;

    private float spin = 0;
   
    void Start() {
        PlaceBallInCenterHeadingDown();
       
        StartCoroutine("ResetBallWaiter");
    }
    
    void Update() {
        
        direction = new Vector2(direction.x + (spin / airSpinDivisor), direction.y).normalized;
        rig.velocity = direction * speed * Time.deltaTime;
        
        ReduceSpin(1.005f);
        
        if (transform.position.x > wallBounds && !goingLeft) {
            //Is the ball at the right border and is not going left (heading towards the right border)
            direction = new Vector2(-direction.x, direction.y + (spin / airSpinDivisor));
            ReduceSpin();

            goingLeft = true;
        }
        
        if (transform.position.x < -wallBounds && goingLeft) {
            //Is the ball at the left border and is going left (heading towards the left border)
            direction = new Vector2(-direction.x, direction.y - (spin / airSpinDivisor));
            ReduceSpin();
            goingLeft = false;
        }
        
        if (transform.position.y > 16 && !goingDown) {
            //Is the ball at the top border and not going down (heading towards the top border)
            direction = new Vector2(direction.x + (spin / airSpinDivisor), -direction.y);
            ReduceSpin();
            goingDown = true;
        }
        
        if (transform.position.y < -5) {
            //This is wrong, the paddle is not always at y 0
            ResetBall();
        }
    }

    void OnCollisionEnter(Collision collision) {
        
    }
    
    void OnTriggerEnter2D(Collider2D collidingObj) {
        if (collidingObj.gameObject.CompareTag(Tags.BOAT_PADDLE)) {
            Vector2 ndir = NormalizedDifference(collidingObj);

            IncreaseSpeed();

            var standardBounceDir = new Vector2(direction.x + (ndir.x / 2), ndir.y).normalized;

            // Spin
            var boat = collidingObj.GetComponent<Boat>();
            ReduceSpin();
            spin = spin + (boat.velocity().x / 6);

            var finalVelocity = new Vector2(
                standardBounceDir.x + spin, 
                standardBounceDir.y).normalized;

            UpdateGoingLeftDown(finalVelocity);
            direction = finalVelocity;
            
        } else if (collidingObj.gameObject.CompareTag(Tags.BLOCK)) {
            var ndir = NormalizedDifference(collidingObj);
            
            
            IncreaseSpeed();

            if (ndir.y >= 0.3 || -0.3 >= ndir.y) {
                direction = new Vector2(direction.x + spin, ndir.y).normalized;
            }
            else {
                direction = new Vector2(-direction.x + spin, ndir.y).normalized;
            }


            ReduceSpin();
            UpdateGoingLeftDown(direction);
            
            collidingObj.gameObject.GetComponent<Block>().TakeDamage(this);
        }
    }

    private Vector2 NormalizedDifference(Collider2D otherCollider2D) {
        return (gameObject.transform.position - otherCollider2D.transform.position).normalized;
    }

    private void IncreaseSpeed() {
        var newSpeed = speed + 5;
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
   
    //Called when the ball goes underneath the paddle and "dies"
    public void ResetBall() {
        transform.position = Vector3.zero; //put ball in the middle of the screen
        direction = Vector2.down;
        StartCoroutine("ResetBallWaiter");

        //manager.LiveLost(); //Calls the 'LiveLost()' function in the GameManager function
        //sacked this off for now as remaking gamemanager - c
    }

    
    private void PlaceBallInCenterHeadingDown() {
        transform.position = Vector3.zero;
        direction = Vector2.down;
    }
    
    /**
     * Called to make the ball wait a second before moving. This is also called when the ball dies
     * and is respawned at the middle of the screen
     */
    private IEnumerator ResetBallWaiter() {
        speed = 0;
        spin = 0;
        yield return new WaitForSeconds(1.0f);
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