using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShapePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    [Tooltip("Level/stage number this puzzle belongs to")]
    public int levelId = 1;

    [Tooltip("All puzzle pieces in this puzzle")]
    public List<ShapePuzzlePiece> puzzlePieces = new List<ShapePuzzlePiece>();
    
    [Tooltip("All puzzle sockets in this puzzle")]
    public List<ShapePuzzleSocket> puzzleSockets = new List<ShapePuzzleSocket>();
    
    [Tooltip("Should the puzzle auto-complete when all pieces are correctly placed?")]
    public bool autoComplete = true;
    
    [Header("Events")]
    public UnityEvent onPuzzleCompleted;
    
    private void Start()
    {
        // Auto-collect pieces and sockets if lists are empty
        if (puzzlePieces.Count == 0)
        {
            puzzlePieces.AddRange(FindObjectsOfType<ShapePuzzlePiece>());
        }
        
        if (puzzleSockets.Count == 0)
        {
            puzzleSockets.AddRange(FindObjectsOfType<ShapePuzzleSocket>());
        }

        // Set level ID on all collected sockets
        foreach (ShapePuzzleSocket socket in puzzleSockets)
        {
            socket.levelId = this.levelId;
        }
        
        // Subscribe to socket events to track puzzle completion
        foreach (ShapePuzzleSocket socket in puzzleSockets)
        {
            socket.onPieceSnapped.AddListener(CheckPuzzleCompletion);
            socket.onPieceRemoved.AddListener(() => CheckPuzzleCompletion(null));
        }
        
        // Add level identifier component for the SlidingDoor to reference
        if (GetComponent<PuzzleLevelIdentifier>() == null)
        {
            PuzzleLevelIdentifier levelIdentifier = gameObject.AddComponent<PuzzleLevelIdentifier>();
            levelIdentifier.levelId = this.levelId;
        }
        else
        {
            GetComponent<PuzzleLevelIdentifier>().levelId = this.levelId;
        }
    }
    
    private void CheckPuzzleCompletion(ShapePuzzlePiece piece = null)
    {
        if (!autoComplete)
            return;
        
        // Check if all sockets are filled with the correct pieces
        bool isComplete = true;
        
        foreach (ShapePuzzleSocket socket in puzzleSockets)
        {
            // If any socket that requires a specific piece doesn't have it, puzzle is incomplete
            if (socket.specificPieceId != string.Empty)
            {
                if (!socket.IsOccupied() || socket.GetCurrentPiece().pieceId != socket.specificPieceId)
                {
                    isComplete = false;
                    break;
                }
            }
            else if (!socket.IsOccupied())
            {
                // For general sockets, just check if they're filled with any compatible piece
                isComplete = false;
                break;
            }
        }
        
        if (isComplete)
        {
            Debug.Log("Puzzle completed");
            onPuzzleCompleted?.Invoke();
        }
    }
    
    public void ResetPuzzle()
    {
        // Reset all pieces to their original positions
        foreach (ShapePuzzlePiece piece in puzzlePieces)
        {
            piece.ResetToOriginalPosition();
        }
    }
    
    public bool IsPuzzleComplete()
    {
        foreach (ShapePuzzleSocket socket in puzzleSockets)
        {
            if (socket.specificPieceId != string.Empty)
            {
                if (!socket.IsOccupied() || socket.GetCurrentPiece().pieceId != socket.specificPieceId)
                {
                    return false;
                }
            }
            else if (!socket.IsOccupied())
            {
                return false;
            }
        }
        
        return true;
    }
}