using UnityEngine;

public class Boat : MonoBehaviour {
    public float speed;

    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private float minXPosition;
    [SerializeField] private float maxXPosition;
    [SerializeField] private bool canMove;

    void Update() {
        if (canMove) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                rig.velocity =
                    new Vector2(-1 * speed * Time.deltaTime, 0); //Set the paddle's rigidbody velocity to move left
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                rig.velocity =
                    new Vector2(1 * speed * Time.deltaTime, 0); //Set the paddle's rigidbody velocity to move right
            }
            else {
                rig.velocity = Vector2.zero; //If those keys arn't being pressed, set the velocity to 0
            }

            transform.position =
                new Vector3(Mathf.Clamp(transform.position.x, minXPosition, maxXPosition), 
                    transform.position.y, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collidingObj) {
        if (collidingObj.gameObject.CompareTag(Tags.CANNONBALL)) {
            collidingObj.gameObject.GetComponent<CannonBall>()
                .SetDirection(transform.position); // bounce the ball of the paddle
        }
    }

    public void ResetPaddle() {
        transform.position = new Vector3(0, transform.position.y, 0); //Sets the paddle's x position to 0
    }
}