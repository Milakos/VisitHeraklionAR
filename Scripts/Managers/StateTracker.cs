using UnityEngine;

public class StateTracker : MonoBehaviour
{
    public enum GameState
    {
        None, Home, Routes, Map, Settings, AR
    } 
    public GameState gameState;
    private void Start() 
    {
        
    }
}
