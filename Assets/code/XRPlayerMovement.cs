using UnityEngine;
using UnityEngine.XR;

public class XRPlayerMovement : MonoBehaviour
{
    public XRNode movementHand = XRNode.LeftHand; // Usually left hand for movement
    public float moveSpeed = 3.0f;
    public Transform cameraTransform; // Reference to the main camera

    private InputDevice movementController;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        
        rb.useGravity = false;
        rb.isKinematic = true;
        
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Get the controller
        if (!movementController.isValid)
        {
            movementController = InputDevices.GetDeviceAtXRNode(movementHand);
            return;
        }
        
        // Get thumbstick input
        if (movementController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement) && movement.magnitude > 0.1f)
        {
            // Create movement direction from the camera's perspective
            Vector3 direction = new Vector3(movement.x, 0, movement.y);
            
            // Convert direction from local space to world space based on camera orientation
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            
            // Calculate final move direction
            Vector3 moveDirection = cameraRight * direction.x + cameraForward * direction.z;
            
            // Move the player
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            
            Debug.Log("Moving: " + moveDirection);
        }
    }
}

// using UnityEngine;
// using UnityEngine.XR;

// public class XRPlayerMovement : MonoBehaviour
// {
//     public XRNode handRole = XRNode.LeftHand;
//     public Transform body;
//     int moveSpeed = 5;
//     Transform camTrans;
//     Rigidbody rb;

//     void Start()
//     {
//         camTrans = Camera.main.transform;
//         rb = GetComponent<Rigidbody>();

//         body.position = new Vector3(camTrans.position.x, body.position.y, camTrans.position.z);
//     }

//     void FixedUpdate()
//     {
//         InputDevices.GetDeviceAtXRNode(handRole).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 direction);
//         Vector3 moveDir = camTrans.forward * direction.y + camTrans.right * direction.x;

//         moveDir = moveDir.normalized * moveSpeed;
//         moveDir.y = rb.linearVelocity.y; // We dont want y so we replace y with that the rb.linearVelocity.y already is.
//         rb.linearVelocity = moveDir; // Set the velocity to our movement vector

//     }
// }