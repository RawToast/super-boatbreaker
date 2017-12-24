using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour {
    
    public float speed;

    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody2D rig;
    
    //this sets the width of walls, these wont move the scene walls YET so just dont fuck with it yet - carlo 
    [SerializeField] private float wallBounds = 5f;
    
    private Vector2 direction;
    private bool goingLeft;
    private bool goingDown;
    
   
    void Start() {
        PlaceBallInCenterHeadingDown();
       
        StartCoroutine("ResetBallWaiter");
    }
    
    void Update() {
        rig.velocity =
            direction * speed *
            Time.deltaTime; //Sets the object's rigidbody velocity to the direction multiplied by the speed

        if (transform.position.x > wallBounds && !goingLeft) {
            //Is the ball at the right border and is not going left (heading towards the right border)
            direction = new Vector2(-direction.x, direction.y);
            goingLeft = true;
        }
        if (transform.position.x < -wallBounds && goingLeft) {
            //Is the ball at the left border and is going left (heading towards the left border)
            direction = new Vector2(-direction.x, direction.y);
            goingLeft = false;
        }
        if (transform.position.y > 5 && !goingDown) {
            //Is the ball at the top border and not going down (heading towards the top border)
            direction = new Vector2(direction.x, -direction.y);
            goingDown = true;
        }
        if (transform.position.y < -5) {
            //Has the ball gone down past the paddle
            ResetBall();
        }
    }

    //Called when the ball needs to change direction (hit paddle, hit brick).
    public void SetDirection(Vector3 target) {
        
        Vector2 dir = transform.position - target; // difference between the ball position and the target
        dir.Normalize(); //Since the difference could be any size, it will be converted to a magnitude of 1.

        direction = dir; //Sets the ball's direction to the 'dir' variable

        //speed += manager.ballSpeedIncrement; //Increases the speed of the ball by the GameManager's 'ballSpeedIncrement' value
        speed += 3; //make this public on ball probably later - c


        if (speed > maxSpeed)
            speed = maxSpeed;
        
        UpdateGoingLeftDown(dir);
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