using System;
using System.Collections;
using System.Collections.Generic;
using PointsOfInterests;
using UnityEngine;

[System.Serializable]
public class TextData
{
    public string stringG;
    public string stringE;
}
public class ObjectSelectionManager : MonoBehaviour
{
    public List<PointOfInterest> selectedObjects = new List<PointOfInterest>();
    public List<PointOfInterest> pathObjects = new List<PointOfInterest>();
    public List<PointOfInterest> seenpois = new List<PointOfInterest>();

    public static double lat;
    public static double lon;

    public static string textGlobal;
    string englishText = "";
    string greekText = "";
    public static string textGlobalD;
    string englishTextD;
    string greekTextD;

    public static string Contact;
    public static string Address;
    public static string Entry;
    public static string OpenEntrance;
    public TextData address = new TextData();
    public TextData entrance = new TextData();
    public TextData ticket = new TextData();

    public static string filterIDText;
    public static List<PointOfInterest> filters;
    public static Sprite spriteIcon;
    private const int RawZero = 0;
    private const int max = 1;
    public static bool has360;
    public static bool hasWebSite;
    public static bool hasTel;
    public static bool hasTicket;
    private bool isPaused = false;
    private float remainingTime = 0f;
    private IEnumerator audioCoroutine;
    public static int index;
    public static Action ChangePoi32;
    public static Action<bool, bool> AudioFinished;
    public Action animationEvent;
int indeger;
public static int indexOfSelected;
public static int indexOfPOIS;  

[HideInInspector] public int newIndex;

    private void OnEnable() 
    {
        UtilsHomeBar.poiAdd += ChangePOIAdditive;
        UtilsHomeBar.deselectObject += DeselectAllObjects; 
        UtilsHomeBar.playAudio += PlayAudio; 
        UtilsHomeBar.stopAudio += StopAudio;
        UtilsHomeBar.pauseAudio += PauseAudio;
        UtilsHomeBar.resumeAudio += ResumeAudio;
        InfoState.exit += DeselectAllObjects;  
        InfoState.resetButtonColor += ChangeColorFromPopUpInfo;
        MapState.exit += DeselectAllObjects;  
        MapState.resetButtonColor += ChangeColorFromPopUpInfo;
        UtilsHomeBar.RestorePath += () => pathObjects.Clear();
        has360 = false;
        hasWebSite = false;
        hasTel = false;
        hasTicket = false;
    }
    void OnDisable()
    {
        UtilsHomeBar.poiAdd -= ChangePOIAdditive;
        UtilsHomeBar.deselectObject -= DeselectAllObjects; 
        UtilsHomeBar.playAudio -= PlayAudio; 
        UtilsHomeBar.stopAudio -= StopAudio; 
        UtilsHomeBar.pauseAudio -= PauseAudio;
        UtilsHomeBar.resumeAudio -= ResumeAudio;
        InfoState.exit -= DeselectAllObjects;
        // InfoState.resetButtonColor -= ChangeColorFromPopUpInfo;
        MapState.exit -= DeselectAllObjects;  
        // MapState.resetButtonColor -= ChangeColorFromPopUpInfo;
        UtilsHomeBar.RestorePath -= () => pathObjects.Clear();
    }
  
    private IEnumerator StartMethod(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        AudioFinished?.Invoke(true, false);
    }
    private void PlayAudio()
    {
        selectedObjects[RawZero].source.Play();
        float clipLength = selectedObjects[RawZero].source.clip.length;
        audioCoroutine = StartMethod(clipLength);
        StartCoroutine(audioCoroutine);
        print("PlayAudio");
    }
    private void StopAudio()
    {
        if(selectedObjects.Count > 0)
        {
            selectedObjects[RawZero].source.Stop();
            if (audioCoroutine != null)
            {
                StopCoroutine(audioCoroutine);
                audioCoroutine = null;
            }
        }
        else
        {
            print("StopAudio");
        }
            
    }
        private void PauseAudio()
        {
            if(selectedObjects.Count > 0)
            {
                selectedObjects[RawZero].source.Pause();
                if (audioCoroutine != null)
                {
                    StopCoroutine(audioCoroutine);
                    remainingTime = selectedObjects[RawZero].source.time;
                    isPaused = true;
                }
            }
            else
            {
                print("PauseAudio");
            } 
        }
        private void ResumeAudio()
        {
            if(selectedObjects.Count > 0)
            {
                selectedObjects[RawZero].source.UnPause();
                audioCoroutine = StartMethod(selectedObjects[RawZero].source.clip.length - remainingTime);
                StartCoroutine(audioCoroutine);
                isPaused = false;
            }
            else
            {
                print("ResumeAudio");
            } 
        }

    private void ChangePOIAdditive(bool isAdditive)
    {
        if (selectedObjects.Count > 0)
        {
            int indexOfSelectedObject = pathObjects.IndexOf(selectedObjects[RawZero]);

            if (indexOfSelectedObject != -max)
            {
                if (isAdditive)
                {
                    newIndex = Mathf.Min(indexOfSelectedObject + max, pathObjects.Count - max);
                    Debug.Log("Selected object is in pathObjects at index: " + newIndex);
                }
                else
                {
                    newIndex = Mathf.Max(indexOfSelectedObject - max, RawZero);
                }

                // selectedObjects.Clear();
                DeselectObject(selectedObjects[RawZero]);
                selectedObjects.Add(pathObjects[newIndex]);
                
                // if(!seenpois.Contains(pathObjects[newIndex]))
                // {
                //     seenpois.Add(pathObjects[newIndex]);
                // }
                
                // ChangePoi32?.Invoke();
                UtilsHomeBar.SelectedPOIButton(newIndex);
                
            }
            else
            {
                Debug.Log("Selected object is not in pathObjects.");
            }
        }
        else
        {
            Debug.Log("No selected objects to change.");
        }  
    }
    private void Update() 
    {
        if(selectedObjects != null)
        {
            if(selectedObjects.Count > 0)
            {
                has360 = selectedObjects[0].has360Image;
                hasWebSite = selectedObjects[0].hasWebSite;
                hasTel = selectedObjects[0].hasTel;
                hasTicket = selectedObjects[0].hasTicket;
            }
            
        }
        indexOfPOIS = pathObjects.Count;
        Debug.Log($"Index of pois is {indexOfPOIS}");
    }
    public int ReturnSelectedObjectsIndex()
    {
        if(pathObjects.Contains(selectedObjects[0]))
        {
            return pathObjects.IndexOf(selectedObjects[0]);
        }
        else
        {
            return 0;
        }
    }
    // Selects a PointOfInterest, updating UI properties based on the selection.
    public void SelectObject(PointOfInterest selectedObject)
    {   
        DeselectAllObjects();
        
        if (selectedObject != null)
        {
            selectedObject.isSelected = true;
            selectedObjects.Add(selectedObject);
        }

        greekText = selectedObjects[RawZero].GreekRef;
        englishText = selectedObjects[RawZero].EnglishRef;
        englishTextD = selectedObjects[RawZero].EnglishRefD;
        greekTextD = selectedObjects[RawZero].GreekRefD;
        address.stringG = selectedObjects[RawZero].addressG;
        address.stringE = selectedObjects[RawZero].addressE;
        entrance.stringG = selectedObjects[RawZero].entranceG;
        entrance.stringE = selectedObjects[RawZero].entranceE;
        ticket.stringG = selectedObjects[RawZero].ticketG;
        ticket.stringE = selectedObjects[RawZero].ticketE;

        filterIDText = UIExtentions.IsEnglish() ? selectedObject.filterE.ToString() : selectedObject.filterG.ToString();  

        spriteIcon = selectedObjects[RawZero].filterIDIconSprite;
        
        if(!seenpois.Contains(selectedObject))
        {
            selectedObject.frame.color = UIExtentions.BlueDark;
            selectedObject.filterFrame.color = UIExtentions.BlueDark;
            selectedObject.filterRenderer.color = UIExtentions.yellow;
            selectedObject.textCounter.color = UIExtentions.yellow;
        }

        textGlobal = UIExtentions.IsEnglish() ? englishText : greekText;
        textGlobalD = UIExtentions.IsEnglish() ? englishTextD : greekTextD;
        Address = UIExtentions.IsEnglish() ? address.stringE : address.stringG;
        OpenEntrance = UIExtentions.IsEnglish() ? entrance.stringE : entrance.stringG;
        Entry = UIExtentions.IsEnglish() ? ticket.stringE : ticket.stringG;
        
        Contact = selectedObjects[RawZero].Contact;

        index = selectedObject.routeIndex + 1;
        has360 = selectedObject.has360Image;
        hasWebSite = selectedObject.hasWebSite;
        hasTel = selectedObject.hasTel;
        hasTicket = selectedObject.hasTicket;

        indexOfSelected = ReturnSelectedObjectsIndex();
    }

    // Deselects a PointOfInterest, updating its state and removing it from the selection list
    public void DeselectObject(PointOfInterest deselectedObject)
    {
        ChangeColorFromPopUpInfo(deselectedObject);
        selectedObjects.Remove(deselectedObject);
        StopAudio();
    }

    private static void _ChangePOIColor(PointOfInterest deselectedObject, Color color)
    {                
        deselectedObject.frame.color = color;
        deselectedObject.filterFrame.color = color;
        deselectedObject.filterRenderer.color = UIExtentions.BlueDark;
        deselectedObject.textCounter.color = UIExtentions.BlueDark;
        deselectedObject.isSelected = false;       
    }
    

    
    // Deselects all currently selected PointOfInterest objects, updating their states.
    public void DeselectAllObjects()
    {
        // ChangeColorFromPopUpInfo();

        
        if(UtilsHomeBar.isInMap)
        {
            if(IsNoneInMapOrRoutes() || IsEndedInMapOrRoutes())
            {
                if(selectedObjects != null && selectedObjects.Count > 0)
                {
                    var item = selectedObjects[0];

                    item.isSelected = false;
                    item.frame.color = UIExtentions.white;
                    item.filterFrame.color = UIExtentions.white;
                    item.filterRenderer.color = UIExtentions.BlueDark;
                    item.textCounter.color = UIExtentions.BlueDark;
                }
                if(seenpois != null && seenpois.Count > 0) 
                {
                    foreach (var item in seenpois)
                    {
                        item.isSelected = false;
                        item.frame.color = UIExtentions.white;
                        item.filterFrame.color = UIExtentions.white;
                        item.filterRenderer.color = UIExtentions.BlueDark;
                        item.textCounter.color = UIExtentions.BlueDark;
                    }
                }

                seenpois.Clear();
            }
            else if(IsStartedInMapOrRoutes())
            {
                
                if(selectedObjects != null && selectedObjects.Count > 0 && UtilsHomeBar.infoCheck32 == false)
                {
                    var selectedObject = selectedObjects[0];

                    if(!seenpois.Contains(selectedObject))
                    {
                        selectedObject.frame.color = UIExtentions.white;
                        selectedObject.filterFrame.color = UIExtentions.white;
                        selectedObject.filterRenderer.color = UIExtentions.BlueDark;
                        selectedObject.textCounter.color = UIExtentions.BlueDark;
                    }
                }
            }
        }
        else if(UtilsHomeBar.isInRoutes) 
        {
            if(IsNoneInMapOrRoutes() || IsEndedInMapOrRoutes())
            {
                ResetPathObjects();
            }
            else if(IsStartedInMapOrRoutes())
            {
                if(UtilsHomeBar.infoCheck32 == false)
                {
                    if(selectedObjects != null && selectedObjects.Count > 0)
                    {
                        var selectedObject = selectedObjects[0];

                        if(!seenpois.Contains(selectedObject))
                        {
                            selectedObject.frame.color = UIExtentions.white;
                            selectedObject.filterFrame.color = UIExtentions.white;
                            selectedObject.filterRenderer.color = UIExtentions.BlueDark;
                            selectedObject.textCounter.color = UIExtentions.BlueDark;
                        }
                    }
                }
            }
        }
        else if(UtilsHomeBar.ar && ARBoundHandler.Instance.infoPointState.isAtInfoPointState == true
        ||UtilsHomeBar.ar && ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
        {
            foreach (var item in ARBoundHandler.Instance.points)
            {
                
                item.frame.color = UIExtentions.white;
                item.filterFrame.color = UIExtentions.white;
                item.filterRenderer.color = UIExtentions.BlueDark;
                item.textCounter.color = UIExtentions.BlueDark;   
                item.isSelected = false;           
            }    
            seenpois.Clear();
        }     
        
        newIndex = 0;
        selectedObjects.Clear();
    }
    private void ChangeColorFromPopUpInfo()
    {
        if(IsStartedInMapOrRoutes())
        {
            var obj = selectedObjects[0];
            if(!seenpois.Contains(obj))
            {
                seenpois.Add(obj);
                _ChangePOIColor(obj, UIExtentions.grey05);
            }
        }
    }
    private void ChangeColorFromPopUpInfo(PointOfInterest point)
    {
        if(IsStartedInMapOrRoutes())
        {
            if(!seenpois.Contains(point))
            {
                seenpois.Add(point);
                _ChangePOIColor(point, UIExtentions.grey05);
            }
        }
    }
    public void ResetPathObjects()
    {
        foreach (var item in pathObjects)
        {
            if(item != null)
            {
                item.isSelected = false;
                item.frame.color = UIExtentions.white;
                item.filterFrame.color = UIExtentions.white;
                item.filterRenderer.color = UIExtentions.BlueDark;
                item.textCounter.color = UIExtentions.BlueDark;
            }
        }
        seenpois.Clear();
    }
    public bool IsStartedInMapOrRoutes()
    {
        return UtilsHomeBar.isInMap && MapState.staticStartState.stateStart == StartState.state.Start || UtilsHomeBar.isInRoutes && InfoState.staticState.stateStart == StartState.state.Start;
    }
    public bool IsEndedInMapOrRoutes()
    {
        return UtilsHomeBar.isInMap && MapState.staticStartState.stateStart == StartState.state.End || UtilsHomeBar.isInRoutes && InfoState.staticState.stateStart == StartState.state.End;
    }
    public bool IsNoneInMapOrRoutes()
    {
        return UtilsHomeBar.isInMap && MapState.staticStartState.stateStart == StartState.state.None || UtilsHomeBar.isInRoutes && InfoState.staticState.stateStart == StartState.state.None;
    }
    public void OpenGoogleMapsURL()
    {
        // // Split the geocode string into latitude and longitude
        // string locationName =  selectedObjects[RawZero].POIlocation;

        // CultureInfo cultureInfo = new CultureInfo("en-US");
        if(greekText == "Νεώρια")
        {
            string mapsURL = $"https://www.google.com/maps?q=ΕνετικάΝαυπηγεία";
            Application.OpenURL(mapsURL);
        }
        else
        {
            string mapsURL = $"https://www.google.com/maps?q={greekText}";
            Application.OpenURL(mapsURL);
        } 
    }
}

