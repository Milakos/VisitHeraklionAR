using TMPro;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(WorldSpaceInfoCanvas))]
    public class EditorEnum : Editor
    {
        private SerializedProperty isAtInfo;
        private SerializedProperty model;
        private void OnEnable() 
        {
            isAtInfo = serializedObject.FindProperty("hasModel");
            model = serializedObject.FindProperty("model");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            if (isAtInfo.boolValue)
            {
                EditorGUILayout.PropertyField(model);
            }

            // Apply any changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }         
    }
    #endif
public class WorldSpaceInfoCanvas : MonoBehaviour
{
    public GameObject quadObject;

    public TMP_Text textTitle;
    public SpriteRenderer sprite;
    public TMP_Text description;
    public TMP_Text moreText;
    

    [HideInInspector] public GameObject model;
    public bool hasModel;
    // Start is called before the first frame update
    private void Start() 
    {
        
    }
    public void InitializeTextsAndContent(string titleText, Sprite sprite1, string description1)
    {
        moreText.text = TextLibrary.moreTextTranslate.GetTranslatedText();
        textTitle.text = titleText;
        sprite.sprite = sprite1;
        description.text = description1;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Check if the touch phase is began or moved
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // Check if the object is the one we want to track && info.style.display == DisplayStyle.None
                    if (hitInfo.collider.tag == "ExitButtonAR")
                    {
                        if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
                        {
                            quadObject.SetActive(false);

                            var aRElement = FindObjectOfType<ARElement>();
                            
                            var objectAR = aRElement.selectedObjectInAR[0];
                            
                            objectAR.GetComponent<assignNameTMP>().children.SetActive(true);              
                            Debug.Log("ExitButtonAR");
                        }
                        else
                        {
                            UtilsHomeBar.StopAudio();
                            UtilsHomeBar.DeselectPOI();
                            UtilsHomeBar.MakeChevronBlue();
                            FindObjectOfType<GroupInfoPoint>().MakeTransparentNew(255f, true);
                            model.SetActive(false);
                            UtilsHomeBar.toggleFiltersOpacity?.Invoke(false);
                            Debug.Log("ExitButtonAR At InfoPoint");

                        }
                    }
                    if (hitInfo.collider.tag == "MoreButtonAR")
                    {
                        if(ARBoundHandler.Instance.infoPointState.isAtInfoPointState == false)
                        {
                            FindObjectOfType<ARElement>().InfoPageEnabled();
                        }
                        else
                        {
                            FindObjectOfType<ARElement>().InfoPageEnabled();
                            if (model.GetComponent<AudioSource>().isPlaying)
                            {
                                model.GetComponent<AudioSource>().Stop();
                            }
                            else
                            {
                                Debug.Log("No audio playing");
                            }
                        }
                        
                        
                        Debug.Log("MoreButton");
                    }

                }
            }
        }
    }
}
