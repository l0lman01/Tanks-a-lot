using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Data/MainCamera", order = 1)]
public class CameraSO : ScriptableObject
{
    [SerializeField]
    public Camera camera;


}
