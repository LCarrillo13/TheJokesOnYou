using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    #if UNITY_EDITOR
        Debug.Log("Unity Editor");
    #endif

    #if UNITY_IOS
      Debug.Log("Iphone");
    #endif

    #if UNITY_STANDALONE_OSX
    Debug.Log("Stand Alone OSX");
    #endif

    #if UNITY_STANDALONE_WIN
        Debug.Log("Stand Alone Windows");
    #endif
        
    #if UNITY_IOS
      Debug.Log("Iphone");
    #endif    
    }

}
