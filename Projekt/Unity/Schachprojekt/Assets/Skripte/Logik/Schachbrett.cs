using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schachbrett : MonoBehaviour
{
    [SerializeField] private Transform EffektiverStartpunktUntenLinks;
    [SerializeField] private float Feldgroesse;

    public const int GesFeldGroesse = 8;

    private Figur[,] grid;  //Start bei 1, 1
    private Figur gewaehlteFigur;
    private SchachManager schachManager;

    public FeldAuswahlErsteller feldAuswahlErsteller; 
    private AnimationManager animationManager;

    public Vector3 RelativePositionZumSchachbrettfeld(Vector2Int position)
    {
        return EffektiverStartpunktUntenLinks.position + new Vector3(position.x * Feldgroesse, 0f, position.y * Feldgroesse);
    }

    protected virtual void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        feldAuswahlErsteller = GetComponent<FeldAuswahlErsteller>();

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Figur[GesFeldGroesse, GesFeldGroesse];
    }

    public void SetzeAbhaengigkeiten(SchachManager schachManager)
    {
        this.schachManager = schachManager;
    }

    public Vector3 KalkulierePosVonCoords(Vector2Int coords)
    {
        return EffektiverStartpunktUntenLinks.position + new Vector3(coords.x * Feldgroesse, 0f, coords.y * Feldgroesse);
    }

    private Vector2Int KalkuliereCoordsVonPos(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / Feldgroesse) + GesFeldGroesse / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / Feldgroesse) + GesFeldGroesse / 2;
        return new Vector2Int(x, y);
    }

    public void OnFeldAuswahl(Vector3 inputPosition)
    {
        if (this.blocker) return;
        Vector2Int coords = KalkuliereCoordsVonPos(inputPosition);
        Figur figur = GetFigurOnFeld(coords);

        if (gewaehlteFigur)
        {
            // Figur nochnamal anwählen
            if (figur != null && gewaehlteFigur == figur)
            {
                DeselectFigur();
            }
            else if (figur != null && gewaehlteFigur != figur && schachManager.IstTeamzug(figur.figurFarbe))
            {
                figur.IdleAnimation();
                WahleFigur(figur);
            }

            //Figur ist ausgewählt und "Klick" auf bewegbares feld
            else if (gewaehlteFigur.BewegungMoeglichZu(coords))
            {
                OnAusgewaehlteFigurBewegt(coords, gewaehlteFigur);
            }
        }
        else
        {
            if (figur != null && schachManager.IstTeamzug(figur.figurFarbe))
            {
                figur.IdleAnimation();
                WahleFigur(figur);
            }
        }
    }

    private void WahleFigur(Figur figur)
    {
        schachManager.EntferneAngriffsMoeglichkeitenAufFigur<Koenig>(figur);
        gewaehlteFigur = figur;
        List<Vector2Int> auswahl = gewaehlteFigur.Bewegungsmöglichkeiten;
        ZeigeAusgewaehlteFelder(auswahl);
    }

    private void ZeigeAusgewaehlteFelder(List<Vector2Int> auswahl)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < auswahl.Count; i++)
        {
            Vector3 position = KalkulierePosVonCoords(auswahl[i]);
            bool isSquareFree = GetFigurOnFeld(auswahl[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        feldAuswahlErsteller.ZeigeAuswahl(squaresData);
    }

    private void DeselectFigur()
    {
        gewaehlteFigur = null;
        feldAuswahlErsteller.entferneAuswaehler();
    }

    private void OnAusgewaehlteFigurBewegt(Vector2Int kooridanten, Figur figur)
    {
        Vector2Int pos = figur.position;
        VersucheGegnerischeFigurZuSchlagen(kooridanten);
        UpdateSchachbrettOnFigurBewegt(kooridanten, pos, figur, null);
        DeselectFigur();
        BeendeZug();
    }
    private void BeendeZug()
    {
        schachManager.BeendeZug();
    }

    public void UpdateSchachbrettOnFigurBewegt(Vector2Int newCoords, Vector2Int oldCoords, Figur neuFig, Figur altFig)
    {
        grid[oldCoords.x, oldCoords.y] = altFig;
        grid[newCoords.x, newCoords.y] = neuFig;
    }

    public Figur GetFigurOnFeld(Vector2Int coords)
    {
        if (CheckObCoordsAufFeld(coords)) return grid[coords.x, coords.y];
        return null;
    }

    public bool CheckObCoordsAufFeld(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= GesFeldGroesse || coords.y >= GesFeldGroesse) return false;
        return true;
    }

    public bool HatFigur(Figur figur)
    {
        for (int i = 0; i < GesFeldGroesse; i++)
        {
            for (int j = 0; j < GesFeldGroesse; j++)
            {
                if (grid[i, j] == figur) return true;
            }
        }
        return false;
    }

    public void SetzeFigurAufsFeld(Vector2Int coords, Figur figur)
    {
        if (CheckObCoordsAufFeld(coords))
            grid[coords.x, coords.y] = figur;
    }
    private void VersucheGegnerischeFigurZuSchlagen(Vector2Int coords)
    {
        //Gegnerische Figur
        Figur figur = GetFigurOnFeld(coords);
        if (figur && !gewaehlteFigur.IstGleichesTeam(figur))
        {
            StartKonflikt(gewaehlteFigur, figur, coords);
            SchlageFigur(figur);
        }
        else
        {
            gewaehlteFigur.BewegeFigur(coords);
        }

    }


    private void StartKonflikt(Figur angreifendeFigur, Figur geschlageneFigur, Vector2Int kooridanten)
    {
        Figur figWeiß, figSchwarz;

        if (angreifendeFigur.figurFarbe == FigurFarbe.weiss)
        {
            figWeiß = angreifendeFigur;
            figSchwarz = geschlageneFigur;
        }
        else
        {
            figWeiß = geschlageneFigur;
            figSchwarz = angreifendeFigur;
        }

        double RotationspunktAngreifer;
        double RotationspunktVerteidiger;

        if (geschlageneFigur.position.y - angreifendeFigur.position.y == 0)
        {

            if (angreifendeFigur.figurFarbe == FigurFarbe.schwarz)
            {
                //Dame schwarz greif an und es passt sgoar
                if (geschlageneFigur.position.x > angreifendeFigur.position.x) RotationspunktAngreifer = 270;
                else RotationspunktAngreifer = 90;
            }
            //Dame weiss greift an -> Beide figuren in die verkehrte richtung
            else
            {
                if (geschlageneFigur.position.x > angreifendeFigur.position.x) RotationspunktAngreifer = 90;
                else RotationspunktAngreifer = 270;
            }
        }
        else
        {
            float gegenkathete = geschlageneFigur.position.x - angreifendeFigur.position.x;
            float ankathete = geschlageneFigur.position.y - angreifendeFigur.position.y;
            if (figWeiß.position.y > figSchwarz.position.y)
            {
                RotationspunktAngreifer = 180 + (180 / Math.PI) * Math.Atan((gegenkathete) / (ankathete));
            }
            else RotationspunktAngreifer = (180 / Math.PI) * Math.Atan((gegenkathete) / (ankathete));
        }

        RotationspunktVerteidiger = RotationspunktAngreifer;

        if (angreifendeFigur.figurFarbe == FigurFarbe.weiss) { RotationspunktAngreifer = RotationspunktAngreifer - 180; }
        if (geschlageneFigur.figurFarbe == FigurFarbe.weiss) { RotationspunktVerteidiger = RotationspunktVerteidiger - 180; }


        //Bewegung Angreifer
        animationManager.DreheFigur1(0f, angreifendeFigur, (float)RotationspunktAngreifer);

        //Bewegung Verteidiger
        animationManager.DreheFigur2(0f, geschlageneFigur, (float)RotationspunktVerteidiger);

        //Angriff
        animationManager.StartAnimation(1f, angreifendeFigur, null, AnimationManager.Animationtrigger.Angriff);

        //Sterben
        animationManager.StartAnimation(1.5f, null, geschlageneFigur, AnimationManager.Animationtrigger.Sterben);

        //Loeschen
        animationManager.StartAnimation(4f, null, geschlageneFigur, AnimationManager.Animationtrigger.Loeschen);

        //Bewege Figur 1 auf Figur 2
        animationManager.BewegeFigur(5f, angreifendeFigur, kooridanten);

        //Drehe Figur zurück
        int back = 0;
        if (angreifendeFigur.figurFarbe == FigurFarbe.weiss) back = 180;

        animationManager.DreheFigur1(6f, angreifendeFigur, (float)back);
        this.BlockEingabe(7f);
    }

    //Schlage Figur -> Übergebene Figur wird sterben
    private void SchlageFigur(Figur figur)
    {
        if (figur)
        {
            grid[figur.position.x, figur.position.y] = null;
            schachManager.OnFigurRemoved(figur);
        }
    }

    public void BlockEingabe(float time)
    {
        this.blocker = true;
        StartCoroutine(EingabeblockerManager(time));
    }

    IEnumerator EingabeblockerManager(float time)
    {
        yield return new WaitForSeconds(time);
        this.blocker = false;

    }
    private bool blocker = false;

    public void BefoerdereFigur(Figur figur)
    {
        string modell;
        if (figur.figurFarbe == FigurFarbe.weiss) modell = "DameWeiss";
        else
        {
            modell = "DameSchwarz";
        }
        Vector2Int pos = figur.position;
        FigurFarbe figurFarbe = figur.figurFarbe;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        SchlageFigur(figur);
        animationManager.SauberesLoeschen(1, figur);
        schachManager.BefoerdernErstellung(pos, figurFarbe, modell);
    }

    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal void OnSpielNeustart()
    {
        gewaehlteFigur = null;
        CreateGrid();
    }
    public void SterbenUndLoeschen(Figur geschlageneFigur)
    {
        animationManager.StartEndAnimation(1.5f, geschlageneFigur);
        //animationManager.StartAnimation(1f, null, geschlageneFigur, AnimationManager.Animationtrigger.Loeschen);
    }
}