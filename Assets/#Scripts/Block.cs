using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int blockHp;
	
	//Called whenever a trigger has entered this objects BoxCollider2D.
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag(Tags.CANNONBALL)) {
			var ball = col.gameObject.GetComponent<CannonBall>();
			ball.SetDirection(transform.position);
			
			TakeDamage(ball);
		}
	}

	private void TakeDamage(CannonBall ball) {
		// Maybe use ball speed?
		blockHp = blockHp - 1;
		if (blockHp <= 0) {
			Destroy(gameObject);
		}
	}
}