using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    void VerarbeiteInput(Vector3 inputPosition, GameObject gewaehltesObjekt, Action onClick);
}