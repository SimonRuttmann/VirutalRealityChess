using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeldAuswahlErsteller : MonoBehaviour
{
    [SerializeField] private GameObject auswahlPrefab;
    [SerializeField] private GameObject angriffPrefab;
    private List<GameObject> instanziiertePrefabs = new List<GameObject>();

    public void ZeigeAuswahl(Dictionary<Vector3, bool> FeldDaten)
    {
        entferneAuswaehler();
        foreach (var data in FeldDaten)
        {
            GameObject auswaehler;
            if (data.Value){
                auswaehler = Instantiate(auswahlPrefab);
                auswaehler.transform.position = data.Key;
            }
            else
            {
                auswaehler = Instantiate(angriffPrefab);
                auswaehler.transform.position = data.Key;
            }
            instanziiertePrefabs.Add(auswaehler);

        }
    }

    public void entferneAuswaehler()
    {
        for (int i = 0; i < instanziiertePrefabs.Count; i++)
        {
            Destroy(instanziiertePrefabs[i]);
        }
    }
}