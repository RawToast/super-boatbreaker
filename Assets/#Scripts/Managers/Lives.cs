using UnityEngine;

namespace Managers {
    public class Lives : MonoBehaviour {
        [SerializeField] private int lives;
        
        void Awake() {
            DontDestroyOnLoad(transform.gameObject);
        }

        public void AddLife() {
            lives += 1 ;
        }

        public void TakeLife() {
            lives -= 1 ;
            print("Remaining lives: " + lives);
        }

        public bool HasLifes() {
            return lives > 0;
        }
    
        public bool IsDead() {
            return lives < 0;
        }
    }
}

