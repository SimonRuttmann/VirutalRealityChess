using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dame : Figur
{
    private Vector2Int[] richtungen = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1,- 1),
    };
    public override List<Vector2Int> WaehleMoeglicheFelder()
    {
        Bewegungsmöglichkeiten.Clear();

        float reichweite = Schachbrett.GesFeldGroesse;
        foreach (var richtung in richtungen)
        {
            for (int i = 1; i <= reichweite; i++)
            {
                Vector2Int nextCoords = position + richtung * i;
                Figur figur = schachbrett.GetFigurOnFeld(nextCoords);
                if (!schachbrett.CheckObCoordsAufFeld(nextCoords))
                    break;
                if (figur == null)
                    AddBewegungsmoeglichkeit(nextCoords);
                else if (!figur.IstGleichesTeam(this))
                {
                    AddBewegungsmoeglichkeit(nextCoords);
                    break;
                }
                else if (figur.IstGleichesTeam(this))
                    break;
            }
        }
        return Bewegungsmöglichkeiten;
    }
}