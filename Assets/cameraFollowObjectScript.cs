using UnityEngine;

public class cameraFollowObjectScript : MonoBehaviour
{
    public Transform car; // Assign your car object
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {}

    // Update is called once per frame
    void Update() {}
    
    

    void LateUpdate()
    {
        if (car == null) return;
        
        // Follow the car’s position but ignore rotation
        transform.position = car.position;
    }
}
