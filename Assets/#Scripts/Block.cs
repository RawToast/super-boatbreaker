using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int blockHp;
	
	public void TakeDamage(CannonBall ball) {
		// Maybe use ball speed?
		blockHp = blockHp - 1;
		if (blockHp <= 0) {
			Destroy(gameObject);
		}
	}
}