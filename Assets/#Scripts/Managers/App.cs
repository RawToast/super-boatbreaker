using UnityEngine;

namespace Managers {
    public class App : MonoBehaviour {
        private void Awake() {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }
}