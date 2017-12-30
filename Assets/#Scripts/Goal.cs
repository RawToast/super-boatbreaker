using UnityEngine;
using Util;

public class Goal : MonoBehaviour {
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag(Tags.BOAT_PADDLE)) {

			Grd.Level.NextLevel();
		}
	}
	
}
