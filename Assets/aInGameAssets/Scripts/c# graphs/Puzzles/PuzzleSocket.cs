using UnityEngine;
using UnityEngine.Events;

// Renamed to avoid conflicts with existing PuzzleSocket class
public class ShapePuzzleSocket : MonoBehaviour
{
    [Header("Socket Settings")]
    [Tooltip("Level ID for door matching")]
    public int levelId = 1;
    
    [Tooltip("Tag of the shape this socket accepts")]
    public string acceptedShapeTag;
    
    [Tooltip("Only accept this specific piece ID (leave empty for any)")]
    public string specificPieceId = "";
    
    [Header("Visual Feedback")]
    [Tooltip("Color when socket is empty")]
    public Color emptyColor = Color.white;
    
    [Tooltip("Color when correct piece is placed")]
    public Color correctColor = Color.green;
    
    [Tooltip("Delay in seconds before destroying the piece")]
    public float destroyDelay = 4.5f;

    [Header("Events")]
    public UnityEvent<ShapePuzzlePiece> onPieceSnapped; // Changed to use ShapePuzzlePiece
    public UnityEvent onPieceRemoved; // Changed to lowercase 'o' to match Unity standard
    
    private ShapePuzzlePiece currentPiece;
    private MeshRenderer socketRenderer;
    private Color originalColor;
    
    private void Awake()
    {
        socketRenderer = GetComponent<MeshRenderer>();
        
        if (socketRenderer != null)
        {
            originalColor = socketRenderer.material.color;
            socketRenderer.material.color = emptyColor;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        // Only process if the tag matches
        if (other.CompareTag(acceptedShapeTag))
        {
            ShapePuzzlePiece piece = other.GetComponent<ShapePuzzlePiece>();
            if (piece != null && !IsOccupied())
            {
                // If a specific ID is required, check it
                if (specificPieceId != "" && piece.pieceId != specificPieceId)
                    return;
                    
                // Auto-snap if close enough
                if (Vector3.Distance(transform.position, piece.transform.position) < piece.snapDistance)
                {
                    SnapPiece(piece);
                }
            }
        }
    }
    
    public void SnapPiece(ShapePuzzlePiece piece)
    {
        currentPiece = piece;
        piece.transform.position = transform.position;
        piece.transform.rotation = transform.rotation;
        
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        // Change color to show correct placement
        if (socketRenderer != null)
        {
            socketRenderer.material.color = correctColor;
        }
        
        // Notify the piece it's been snapped
        piece.OnSnapped(this);
        
        // Fire event
        onPieceSnapped?.Invoke(piece);
        
       // if (destroyAfterSnap)
        {
            StartCoroutine(DestroyPieceAfterDelay(piece, destroyDelay));
        }
    }
    
    private System.Collections.IEnumerator DestroyPieceAfterDelay(ShapePuzzlePiece piece, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
    
        // Make sure the piece is still our current piece
        if (currentPiece == piece)
        {
            // Clear the reference
            currentPiece = null;
        
            // Reset socket color
            if (socketRenderer != null)
            {
                socketRenderer.material.color = emptyColor;
            }
        
            // Fire the removal event
            onPieceRemoved?.Invoke();
        
            // Destroy the piece
            //Destroy(piece.gameObject);
        }
    }
    
    public void RemovePiece()
    {
        if (currentPiece != null)
        {
            // Change color back to empty state
            if (socketRenderer != null)
            {
                socketRenderer.material.color = emptyColor;
            }
            
            currentPiece = null;
            onPieceRemoved?.Invoke();
        }
    }
    
    public bool IsOccupied()
    {
        return currentPiece != null;
    }
    
    public ShapePuzzlePiece GetCurrentPiece()
    {
        return currentPiece;
    }
    
    // Optional: Reset to original color when script is disabled
    private void OnDisable()
    {
        if (socketRenderer != null)
        {
            socketRenderer.material.color = originalColor;
        }
    }
}