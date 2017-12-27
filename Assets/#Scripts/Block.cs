using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int blockHp;
	
	//Called whenever a trigger has entered this objects BoxCollider2D.

	public void TakeDamage(CannonBall ball) {
		// Maybe use ball speed?
		blockHp = blockHp - 1;
		if (blockHp <= 0) {
			Destroy(gameObject);
		}
	}
}