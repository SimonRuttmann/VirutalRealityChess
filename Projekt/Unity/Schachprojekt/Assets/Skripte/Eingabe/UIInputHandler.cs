using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputHandler : MonoBehaviour, IInputHandler
{
    public void VerarbeiteInput(Vector3 inputPosition, GameObject gewaehltesObjekt, Action onClick)
    {
        onClick?.Invoke();
    }
}