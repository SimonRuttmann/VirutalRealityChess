using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofortBeweger : MonoBehaviour, IBeweger
{
    public void MoveTo(Transform transform, Vector3 zielPos)
    {
        transform.position = zielPos;
    }
}