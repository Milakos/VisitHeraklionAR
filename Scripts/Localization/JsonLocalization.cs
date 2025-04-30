// using UnityEngine.Localization;
// using UnityEngine.Localization.Tables;
// using UnityEngine;
// using UnityEditor;
// using UnityEditor.Localization;

// [System.Serializable]
// public class LocalizedDataClass
// {
//     // public static string textGlobal;
//     [HideInInspector] public string englishText = "";
//     [HideInInspector] public string greekText = "";
//      public LocalizedString localizedString;
//     public TableEntryReference tableEntryReference;
// }
// [System.Serializable]
// public class GetNumberOfPOI
// {
//     [HideInInspector] public string englishText;
//     [HideInInspector] public string greekText;
//     [HideInInspector] public int numberOfPOIs;
//     [HideInInspector] public LocalizedString localizedString;
//     public TableEntryReference tableEntryReference ;
//     public string NumberOfPois()
//     {
//         return numberOfPOIs.ToString();
//     }
// }
// public class JsonLocalization : MonoBehaviour
// {
//     [SerializeField] public LocalizedDataClass[] data;
//     public GetNumberOfPOI[] pois;
//     public StringTable stringTableGreek;
//     public StringTable stringTableEnglish;
//     public TableReference tableReference;
//     private void OnEnable()
//     {
//         CallJSONMethods();
//     }

//     public void CallJSONMethods()
//     {
//         CacheJsonToCardTitles();
        
//         CachePointOfInterestText();

//         CacheJsonNumberOfPoints();
//     }

//     public void CachePointOfInterestText()
//     {
//         for (int index = 0; index < pois.Length; index++)
//         {
//             pois[index].localizedString.TableReference = tableReference;
//             pois[index].localizedString.SetReference(tableReference, pois[index].tableEntryReference);

//             pois[index].greekText = "Σημεία Ενδιαφέροντος";
//             pois[index].englishText = "Points Of Interest";

//             stringTableEnglish.AddEntryFromReference(pois[index].tableEntryReference, pois[index].englishText);
//             stringTableGreek.AddEntryFromReference(pois[index].tableEntryReference, pois[index].greekText);

//             var collection = LocalizationEditorSettings.GetStringTableCollection(tableReference);
//             var englishTable = collection.GetTable("en") as StringTable;
//             var greekTable = collection.GetTable("el") as StringTable;
//             var entry = englishTable.GetEntry(pois[index].tableEntryReference.Key);
//             var entry1 = greekTable.GetEntry(pois[index].tableEntryReference.Key);
//             entry.IsSmart = true;
//             entry1.IsSmart = true;

//             #if UNITY_EDITOR
//             // If we are in the Editor then we need to mark the table dirty so the changes are saved.
//             EditorUtility.SetDirty(englishTable);
//             EditorUtility.SetDirty(greekTable);
//             #endif
//         }
//     }
//     public void CacheJsonToCardTitles()
//     {

//         for (int index = 0; index < data.Length; index++)
//         {
//             // data[index].stringTableDataG = stringTableGreek;
//             // data[index].stringTableDataE = stringTableEnglish;
//             data[index].localizedString.TableReference = tableReference;
//             data[index].localizedString.SetReference(tableReference, data[index].tableEntryReference);

//             // greekText = GetComponent<ObjectSelectionManager>().selectedObjects[0].GreekRef;
//             // englishText = GetComponent<ObjectSelectionManager>().selectedObjects[0].EnglishRef;

//             data[index].greekText = GetComponent<JSONTest>().globalRoutes.greekTextsRoute[index].Titles[0].ToString();
//             data[index].englishText = GetComponent<JSONTest>().globalRoutes.englishTextsRoute[index].Titles[0].ToString();

//             stringTableEnglish.AddEntryFromReference(data[index].tableEntryReference, data[index].englishText);
//             stringTableGreek.AddEntryFromReference(data[index].tableEntryReference, data[index].greekText);

//             var collection = LocalizationEditorSettings.GetStringTableCollection(tableReference);
//             var englishTable = collection.GetTable("en") as StringTable;
//             var greekTable = collection.GetTable("el") as StringTable;
//             var entry = englishTable.GetEntry(data[index].tableEntryReference.Key);
//             var entry1 = greekTable.GetEntry(data[index].tableEntryReference.Key);
//             entry.IsSmart = true;
//             entry1.IsSmart = true;


//             #if UNITY_EDITOR
//             // If we are in the Editor then we need to mark the table dirty so the changes are saved.
//             EditorUtility.SetDirty(englishTable);
//             EditorUtility.SetDirty(greekTable);
//             #endif
//         }
        
//     }
//     public void CacheJsonNumberOfPoints()
//     {
        
//         for (int index = 0; index < data.Length; index++)
//         {
//             pois[index].numberOfPOIs = GetComponent<JSONTest>().globalPath[index].coordinates.Count;
            
//             print(pois[index].NumberOfPois());
//         }
        
//     }

//     void OnGUI()
//     {
        
//         for (int index = 0; index < data.Length; index++)
//         {
//             EditorGUILayout.LabelField(data[index].greekText);
//             EditorGUILayout.LabelField(data[index].englishText);
//         }
//         for (int index = 0; index < pois.Length; index++)
//         {
//             EditorGUILayout.LabelField(pois[index].greekText);
//             EditorGUILayout.LabelField(pois[index].englishText);
//         }
//     }


// }
