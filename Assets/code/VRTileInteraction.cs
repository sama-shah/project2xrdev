using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRTileInteraction : MonoBehaviour
{
    public XRNode handRole = XRNode.RightHand;
    public Transform controllerTransform;
    public float hitRadius = 0.05f;
    public LayerMask tileLayer;
    
    private InputDevice _controller;
    private bool _wasTriggerPressed = false;
    
    void Start()
    {
        if (controllerTransform == null)
        {
            controllerTransform = transform;
        }
    }
    
    void Update()
    {
        // Get the controller
        _controller = InputDevices.GetDeviceAtXRNode(handRole);
        
        // Check trigger state
        _controller.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed);
        
        // Check for trigger press/punch action
        if (triggerPressed && !_wasTriggerPressed)
        {
            CheckForTileHit();
        }
        
        // Store trigger state
        _wasTriggerPressed = triggerPressed;
    }
    
    void CheckForTileHit()
    {
        // Cast a sphere to detect tiles
        Collider[] hitColliders = Physics.OverlapSphere(controllerTransform.position, hitRadius, tileLayer);
        
        foreach (var hitCollider in hitColliders)
        {
            GlowingTile glowingTile = hitCollider.GetComponent<GlowingTile>();
            if (glowingTile != null && glowingTile.IsGlowing())
            {
                // Trigger the hit on the glowing tile
                hitCollider.GetComponent<GlowingTile>().StopGlowing();
                
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.OnTileClicked(hitCollider.gameObject);
                }
                
                // Add haptic feedback if desired
                if (_controller.TryGetHapticCapabilities(out HapticCapabilities capabilities) && 
                    capabilities.supportsImpulse)
                {
                    _controller.SendHapticImpulse(0, 0.5f, 0.1f);
                }
                
                // Break after first hit
                break;
            }
        }
    }
    
    // Optional: Draw gizmos for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (controllerTransform != null)
        {
            Gizmos.DrawWireSphere(controllerTransform.position, hitRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, hitRadius);
        }
    }
}