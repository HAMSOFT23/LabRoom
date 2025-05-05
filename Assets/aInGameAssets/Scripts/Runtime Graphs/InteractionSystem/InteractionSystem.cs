// --- InteractionSystem modified for OUTLINE ONLY ---
#pragma warning disable
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform _camTransform;
    [SerializeField] private float _interactDistance = 1F;
    [SerializeField] private LayerMask outlineLayer = 3; // Use layers for outlined objects

    private outlineController currentOutlineController = null;

    void Update()
    {
        if (_camTransform == null) { /* ... error check ... */ return; }
        LookForOutlineTarget();
    }

    public void LookForOutlineTarget()
    {
        RaycastHit hitInfo;
        outlineController hitOutlineController = null; // Controller on the object hit THIS frame

        bool hitDetected = Physics.Raycast(_camTransform.position, _camTransform.forward, out hitInfo, _interactDistance, outlineLayer);

        if (hitDetected)
        {
            // Directly try to get OutlineController
            hitInfo.collider.TryGetComponent<outlineController>(out hitOutlineController);
            Debug.Log($"Raycast Hit: {hitInfo.collider.gameObject.name}. Found OutlineController? {hitOutlineController != null}");
        }

        // --- Logic based ONLY on OutlineController ---
        if (hitOutlineController != currentOutlineController)
        {
            // Deactivate previous if it exists
            if (currentOutlineController != null)
            {
                Debug.Log($"Deactivating outline on {currentOutlineController.gameObject.name}");
                currentOutlineController.DeactivateOutline();
            }

            // Activate new one if it exists
            if (hitOutlineController != null)
            {
                Debug.Log($"Activating outline on {hitOutlineController.gameObject.name}");
                hitOutlineController.ActivateOutline();
                currentOutlineController = hitOutlineController;
            }
            else // Hit object doesn't have OutlineController
            {
                 currentOutlineController = null;
            }
        }
        // else: Looking at the same object or one without OutlineController, state is fine.

        // --- Handle case where Raycast hits nothing ---
         if (!hitDetected && currentOutlineController != null)
         {
            Debug.Log($"Deactivating outline on {currentOutlineController.gameObject.name} (Ray missed)");
            currentOutlineController.DeactivateOutline();
            currentOutlineController = null;
         }


        // Debug Draw
        if(hitDetected) Debug.DrawRay(_camTransform.position, _camTransform.forward * hitInfo.distance, Color.green, 0.1f);
        else Debug.DrawRay(_camTransform.position, _camTransform.forward * _interactDistance, Color.red, 0.1f);
    }
}