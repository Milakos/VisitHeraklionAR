using UnityEngine;

public class PlayAudioOnSpawn : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public ObjectSelectionManager objectSelection;

    private void OnEnable() 
    {
        PlayOnSpawn();
    }
    private void Start() 
    {
        source.Play();
    }
    private void OnDisable() 
    {
        source.Stop();
        source.clip = null;
        // objectSelection = null;
        clip = null;
    }
    private void PlayOnSpawn()
    {
        source = GetComponent<AudioSource>(); 
        objectSelection = FindObjectOfType<ObjectSelectionManager>();
        clip = objectSelection.selectedObjects[0].GetComponent<AudioSource>().clip;
        source.clip = clip;
    }
}
