using UnityEngine;

// Renamed to avoid conflicts with existing PuzzlePiece class
public class ShapePuzzlePiece : MonoBehaviour
{
    [Header("Piece Settings")]
    public string pieceId;
    public float snapDistance = 0.5f;
    
    private ShapePuzzleSocket currentSocket;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    
    // Called by the socket when this piece is snapped
    public void OnSnapped(ShapePuzzleSocket socket)
    {
        // Detach from previous socket if any
        if (currentSocket != null && currentSocket != socket)
        {
            currentSocket.RemovePiece();
        }
        
        currentSocket = socket;
    }
    
    public void DetachFromSocket()
    {
        if (currentSocket != null)
        {
            currentSocket.RemovePiece();
            currentSocket = null;
        }
    }
    
    public void ResetToOriginalPosition()
    {
        DetachFromSocket();
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
    
    public bool IsPlaced()
    {
        return currentSocket != null;
    }
    
    public ShapePuzzleSocket GetCurrentSocket()
    {
        return currentSocket;
    }
}