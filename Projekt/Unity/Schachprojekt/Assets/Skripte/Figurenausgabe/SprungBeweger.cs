
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprungBeweger: MonoBehaviour, IBeweger
{
    [SerializeField] private float bewegungsGeschwindigkeit;
    [SerializeField] private float sprungHoehe;

    public void MoveTo(Transform transform, Vector3 zielPos)
    {
        float distance = Vector3.Distance(zielPos, transform.position);
        transform.DOJump(zielPos, sprungHoehe, 1, distance / bewegungsGeschwindigkeit);
    }
}
