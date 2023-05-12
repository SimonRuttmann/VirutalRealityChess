using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Erstellt eine Figur
public class FigurErsteller : MonoBehaviour
{
    //Dieses Array beinhaltet alle unserer Figuren in schwarzer und wei�er Version
    [SerializeField] private GameObject[] Modellarray;

    //Dieses Dictionary ist einfach nur f�r einfachen Zugriff vorhanden. Zugriff �ber String statt Index
    private Dictionary<string, GameObject> ModellDictionary;

    // Alle Modelle dem Dictionary hinzuf�gen
    private void Awake()
    {
        if (ModellDictionary != null)
        {
            return;
        }
        this.ModellDictionary = new Dictionary<string, GameObject>();
        this.AddModelleZumDictionary();
    }
    private void AddModelleZumDictionary()
    {
        foreach (GameObject modell in Modellarray)
        {
            // Key:  LaueferSchwarz
            // Key:  LaeuferSchwarz=PrefabName (Laeufer=Skript)       Value: LaeuferSchwarz (UnityEngine.GameObject) 
            String modellName = modell.GetComponent<Figur>().ToString();        
                                                           
            modellName = modellName.Split(' ')[0];
            ModellDictionary.Add(modellName, modell);
        }
        
    }
    //ErstelleFigur("schwarzerBauer") -> Sucht sich das Modell/Prefab und instanziiert es -> !Neues Objekt in der Szene!
    public GameObject ErstelleFigur(String figurtyp)
    {

        GameObject modell = ModellDictionary[figurtyp];             //"Bauer1" -> Bauer1Pref
        if (modell)
        {
            GameObject richtigeFigur = Instantiate(modell);          
            return richtigeFigur;
        }
        return null;
    }

}