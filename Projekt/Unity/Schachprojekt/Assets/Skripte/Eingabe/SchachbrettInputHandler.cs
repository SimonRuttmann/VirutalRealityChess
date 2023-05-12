using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Schachbrett))]
public class SchachbrettInputHandler : MonoBehaviour, IInputHandler
{
    private Schachbrett schachbrett;

    private void Awake()
    {
        schachbrett = GetComponent<Schachbrett>();
    }

    public void VerarbeiteInput(Vector3 position, GameObject gewaehltesObjekt, Action onClick)
    {
        schachbrett.OnFeldAuswahl(position);
    }
}