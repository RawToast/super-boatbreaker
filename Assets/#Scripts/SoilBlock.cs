using UnityEngine;

public class SoilBlock : MonoBehaviour {
	
	//Called whenever a trigger has entered this objects BoxCollider2D.
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag(Tags.CANNONBALL)) {

			col.gameObject.GetComponent<CannonBall>().SetDirection(transform.position);
			
			Destroy(gameObject);
		}
	}
}