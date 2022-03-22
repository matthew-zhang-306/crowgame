using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
                Debug.Log("Unity Editor");
#endif

        #if UNITY_WSA
            Debug.Log("Unity WSA");
        #endif  

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
