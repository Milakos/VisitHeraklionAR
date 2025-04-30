using CrazyMinnow.SALSA;
using UnityEngine;

public class PlayAudioAtInfoPoint : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public ObjectSelectionManager objectSelection;

    public Eyes eyes;
    public GameObject lookAtCamera;

    private void Awake() 
    {
        source = GetComponent<AudioSource>();  
        objectSelection = FindObjectOfType<ObjectSelectionManager>();
    }
    private void OnEnable() 
    {
        lookAtCamera = GameObject.Find("AR Camera");
    }
    private void Update() 
    {
        Eyes();
    }
    public void Eyes()
    {
        eyes.headTarget = lookAtCamera.transform;
    }

    public void PlayOnSpawn()
    {
        clip = objectSelection.selectedObjects[0].GetComponent<AudioSource>().clip;
        source.clip = clip;
        source.Play();
        Debug.Log("ADUIOPLAY");
    }
}
