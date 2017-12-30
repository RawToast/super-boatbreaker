using UnityEngine;

public class Sea : MonoBehaviour {
    
    [SerializeField] private Rigidbody2D rig;

    [SerializeField] private int waveSize;
    [SerializeField] private int waveTimer;
    [SerializeField] private int dipSize;
    
    
    [SerializeField] private float waveVelocity;
    [SerializeField] private float dipVelocity;
    [SerializeField] private float baseVelocity;
    
    
    
    private int waveCounter;

    void Update() {
        
        Wave();

    }


    void Wave() {
        waveCounter++;

        if (waveCounter >= (waveTimer + waveSize)) {
            rig.velocity = Vector2.zero;
            waveCounter = -dipSize;
        } else if (waveCounter >= waveTimer) {
            rig.velocity = new Vector2(0, waveVelocity);
        } else if (0 >= waveCounter) {
            rig.velocity = new Vector2(0, dipVelocity);
        } else {
            rig.velocity = new Vector2(0, baseVelocity);
        }

    }
}