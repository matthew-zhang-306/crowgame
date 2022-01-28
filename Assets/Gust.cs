using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gust : MonoBehaviour
{
    [Header("Parameters")]
    public float moveTime;
    public float maxDistance;

    [Header("References")]
    public GameObject tornadoPrefab;

    private void Awake() {
        
    }

}
