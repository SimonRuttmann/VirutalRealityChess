using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bauer : Figur
{
    
    public override List<Vector2Int> WaehleMoeglicheFelder()
    {
        Bewegungsmöglichkeiten.Clear();
        Vector2Int richtung = figurFarbe == FigurFarbe.weiss ? Vector2Int.up : Vector2Int.down;
        float reichweite = WurdeBewegt ? 1 : 2;
        for (int i = 1; i <= reichweite; i++)
        {
            Vector2Int nextCoords = position + richtung * i;
            Figur figur = schachbrett.GetFigurOnFeld(nextCoords);
            if (!schachbrett.CheckObCoordsAufFeld(nextCoords))
                break;
            if (figur == null)
                AddBewegungsmoeglichkeit(nextCoords);
            else
                break;
        }

        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, richtung.y), new Vector2Int(-1, richtung.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = position + takeDirections[i];
            Figur piece = schachbrett.GetFigurOnFeld(nextCoords);
            if (!schachbrett.CheckObCoordsAufFeld(nextCoords))
                continue;
            if (piece != null && !piece.IstGleichesTeam(this))
            {
                AddBewegungsmoeglichkeit(nextCoords);
            }
        }
        return Bewegungsmöglichkeiten;
    }

    public override void BewegeFigur(Vector2Int coords)
    {
        base.BewegeFigur(coords);
        CheckBeförderung();
    }

    private void CheckBeförderung()
    {
        int endOfBrettYCoord = figurFarbe == FigurFarbe.weiss ? Schachbrett.GesFeldGroesse - 1 : 0;
        if (position.y == endOfBrettYCoord)
        {
            schachbrett.BefoerdereFigur(this);
        }
    }
    
}