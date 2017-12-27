using UnityEngine;
using UnityEngine.Rendering;

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
                    new Vector2(-1 * speed * Time.deltaTime, rig.velocity.y);
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                rig.velocity =
                    new Vector2(1 * speed * Time.deltaTime, rig.velocity.y);
            }
            else {
                rig.velocity = new Vector2(0, rig.velocity.y);
            }

            transform.position =
                new Vector3(Mathf.Clamp(transform.position.x, minXPosition, maxXPosition), 
                    transform.position.y, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collidingObj) {
        if (collidingObj.gameObject.CompareTag(Tags.SEA)) {
            if (rig.velocity.y < 0) {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.15f);
            }
            else {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.05f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collidingObj) { 
        if (collidingObj.gameObject.CompareTag(Tags.SEA)) {
            if (rig.velocity.y < 0) {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.15f);
            }
            else {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.085f);
            }
            
        }
    }

    public void ResetPaddle() {
        transform.position = new Vector3(0, transform.position.y, 0); //Sets the paddle's x position to 0
    }

    public Vector2 velocity() {
        return rig.velocity;
    }
}