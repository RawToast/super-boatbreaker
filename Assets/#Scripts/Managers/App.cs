#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Managers {
    public class App : MonoBehaviour {
        private void Awake() {
            #if UNITY_EDITOR
                return;
            #endif
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }
}