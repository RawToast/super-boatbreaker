using UnityEngine;

//TODO I am not being used. Please fix me.
public class BoatGameManager : MonoBehaviour {
    [SerializeField] private int score;
    [SerializeField] public int lives;

    public void IncrementScore(int amountToIncrementBy) {
        score = score + amountToIncrementBy;
    }
}