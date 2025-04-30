using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

[System.Serializable]
public class InitData
{
    public string text;
    public LocalizedString tableKeyName;
    public TableEntryReference entry;
}

[CreateAssetMenu (fileName = "Data", menuName = "ARLocalizedData/Data", order = 0 )]
public class SO_PageData : ScriptableObject
{
    [Space(10)][Header("Collection Table")][Space(10)]
    public TableReference tableCollectionName;  
    [Space(10)]
    public List<InitData> data = new List<InitData>();

    // private void OnEnable() 
    // {
    //     for (int i = 0; i < data.Count; i++)
    //     {
    //         data[i].tableKeyName.SetReference(tableCollectionName, data[i].entry);
    //         data[i].text =  data[i].tableKeyName.GetLocalizedString();  
    //     }
 
    // }
}
