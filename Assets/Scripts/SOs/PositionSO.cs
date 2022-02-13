using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Position", menuName = "SOs/Position")]
public class PositionSO : ScriptableObject
{
    public Vector3 position;
    public Vector3 direction;
}
