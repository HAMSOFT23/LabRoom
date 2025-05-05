using UnityEngine;

public class outlineController : MonoBehaviour
{
    [Header("Outline Active Settings")]
    public float activeOutlineThickness = 0.0101f; // Thickness when highlighted
    public float activeFadeDistance = 70.4f; // Fade distance when highlighted (or keep original)
    public Color activeOutlineColor = Color.red; // Color when highlighted

    [Header("Outline Inactive Settings")]
    public float inactiveOutlineThickness = 0f; // Thickness when not highlighted (usually 0)
    // You might want an inactive color/fade distance too, or just use thickness=0

    // --- Internal variables ---
    private Renderer objectRenderer;
    private Material outlineMaterialInstance;
    private bool isCurrentlyActive = false; // Track the state

    // Property IDs for efficiency
    // --- Make sure these strings exactly match your Shader Graph Reference names! ---
    private static readonly int OutlineThicknessID = Shader.PropertyToID("_Outline_Thickness");
    private static readonly int FadeDistanceID = Shader.PropertyToID("_Fade_Distance");
    private static readonly int OutlineColorID = Shader.PropertyToID("_Outline_Color");

    void Awake()
    {
        // Get the Renderer component
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("OutlineController requires a Renderer component.", this);
            this.enabled = false;
            return;
        }

        // Check if the object has a Collider, required for Raycasting
        if (GetComponent<Collider>() == null)
        {
             Debug.LogWarning("OutlineController GameObject needs a Collider component to be detected by Raycasts.", this);
        }

        // Get the material instance. Accessing .material creates an instance.
        outlineMaterialInstance = objectRenderer.material;
        if (outlineMaterialInstance == null)
        {
             Debug.LogError("Renderer does not have a material assigned.", this);
             this.enabled = false;
             return;
        }

        // Set the initial inactive state
        SetInactiveState();
    }

    // Call this method from your Raycast script when the object is hit
    public void ActivateOutline()
    {
        if (outlineMaterialInstance != null && !isCurrentlyActive)
        {
            outlineMaterialInstance.SetFloat(OutlineThicknessID, activeOutlineThickness);
            outlineMaterialInstance.SetFloat(FadeDistanceID, activeFadeDistance); // Set active fade distance
            outlineMaterialInstance.SetColor(OutlineColorID, activeOutlineColor);
            isCurrentlyActive = true;
             Debug.Log($"Activated outline on {gameObject.name}");
        }
    }

    // Call this method from your Raycast script when the object is no longer hit
    public void DeactivateOutline()
    {
        if (outlineMaterialInstance != null && isCurrentlyActive)
        {
            SetInactiveState();
            // Debug.Log($"Deactivated outline on {gameObject.name}");
        }
    }

    // Helper method to set the inactive state
    private void SetInactiveState()
    {
         if (outlineMaterialInstance != null)
         {
            outlineMaterialInstance.SetFloat(OutlineThicknessID, inactiveOutlineThickness);
            // Optionally reset fade distance and color to defaults if needed
            // outlineMaterialInstance.SetFloat(FadeDistanceID, originalFadeDistance);
            // outlineMaterialInstance.SetColor(OutlineColorID, originalColor);
            isCurrentlyActive = false;
         }
    }


    void OnDestroy()
    {
        // Clean up the created material instance
        if (outlineMaterialInstance != null && objectRenderer != null && outlineMaterialInstance != objectRenderer.sharedMaterial)
        {
            Destroy(outlineMaterialInstance);
        }
    }
}