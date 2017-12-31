using UnityEngine;

namespace Managers {
    public class Score : MonoBehaviour {
        [SerializeField] private int score;
        
        void Awake() {
            print(Grd.Score.score);
            DontDestroyOnLoad(transform.gameObject);
        }

        public void IncrementScore(int amountToIncrementBy) {
            score = score + amountToIncrementBy;
            //print("Score: " + score);
        }

        public int GetScore() {
            return score;
        }
    }
}

