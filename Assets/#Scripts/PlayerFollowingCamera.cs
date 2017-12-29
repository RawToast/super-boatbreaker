using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour {

    [SerializeField] private Rigidbody2D rig;

    [SerializeField] private float offset;
    
    void LateUpdate() {
        transform.position = new Vector3(0,
            rig.transform.position.y + offset,
            -10f);
    }
}
