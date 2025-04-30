using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{  
   public static T Instance{get; private set;}

   public static bool Initialized{get{return Instance != null;} }

   public virtual void Awake() 
   {
       if (Instance != null)
        Debug.LogError($"Trying to instantiate a second instance of singleton class{GetType().Name}");
        else
        Instance = (T) this;
        DontDestroyOnLoad(Instance);
   }

   protected virtual void OnDestory()
   {
       if (Instance == this)
       Instance = null;
   }
   
   
}
