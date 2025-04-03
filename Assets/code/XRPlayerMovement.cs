using UnityEngine;
using UnityEngine.XR;

public class XRPlayerMovement : MonoBehaviour
{
    public XRNode handRole = XRNode.LeftHand;
    public Transform body;
    int moveSpeed = 5;
    Transform camTrans;
    Rigidbody rb;

    void Start()
    {
        camTrans = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

        body.position = new Vector3(camTrans.position.x, body.position.y, camTrans.position.z);
    }

    void FixedUpdate()
    {
        InputDevices.GetDeviceAtXRNode(handRole).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 direction);
        Vector3 moveDir = camTrans.forward * direction.y + camTrans.right * direction.x;

        moveDir = moveDir.normalized * moveSpeed;
        moveDir.y = rb.linearVelocity.y; // We dont want y so we replace y with that the rb.linearVelocity.y already is.
        rb.linearVelocity = moveDir; // Set the velocity to our movement vector

    }
}