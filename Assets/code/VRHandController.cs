using UnityEngine;

public class VRHandController : MonoBehaviour
{
    // Add this script to your VR controller objects
    
    void Start()
    {
        // Make sure the controller object has a collider with "Is Trigger" checked
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            // Add a sphere collider if none exists
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.05f;
            sphereCollider.isTrigger = true;
            Debug.Log("Added sphere collider to VR controller");
        }
        else if (!collider.isTrigger)
        {
            collider.isTrigger = true;
            Debug.Log("Set existing collider to trigger mode");
        }
        
        // Make sure the controller has the right tag
        if (tag != "Hand" && tag != "Controller")
        {
            tag = "Controller";
            Debug.Log("Set tag to Controller");
        }
    }
}