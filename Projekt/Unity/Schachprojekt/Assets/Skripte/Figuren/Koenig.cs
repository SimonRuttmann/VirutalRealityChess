
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koenig : Figur
{

    Vector2Int[] richtungen = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    };

    private Figur linkerTurm;
    private Figur rechterTurm;

    private Vector2Int linksRochade;
    private Vector2Int rechtsRochade;

    public override List<Vector2Int> WaehleMoeglicheFelder()
    {
        Bewegungsmöglichkeiten.Clear();
        BerechneStandartBewegungen();
        BerechneRochadeMoeglichkeiten();
        return Bewegungsmöglichkeiten;

    }

    private void BerechneRochadeMoeglichkeiten()
    {
        linksRochade = new Vector2Int(-1, -1);
        rechtsRochade = new Vector2Int(-1, -1);
        if (!WurdeBewegt)
        {
            linkerTurm = GetFigurInRichtung<Turm>(figurFarbe, Vector2Int.left);
            if (linkerTurm && !linkerTurm.WurdeBewegt)
            {
                linksRochade = position + Vector2Int.left * 2;
                Bewegungsmöglichkeiten.Add(linksRochade);
            }
            rechterTurm = GetFigurInRichtung<Turm>(figurFarbe, Vector2Int.right);
            if (rechterTurm && !rechterTurm.WurdeBewegt)
            {
                rechtsRochade = position + Vector2Int.right * 2;
                Bewegungsmöglichkeiten.Add(rechtsRochade);
            }
        }
    }

    private Figur GetFigurInRichtung<T>(FigurFarbe team, Vector2Int direction)
    {
        for (int i = 1; i <= Schachbrett.GesFeldGroesse; i++)
        {
            Vector2Int nextCoords = position + direction * i;
            Figur figur = schachbrett.GetFigurOnFeld(nextCoords);
            if (!schachbrett.CheckObCoordsAufFeld(nextCoords))
                return null;
            if (figur != null)
            {
                if (figur.figurFarbe != team || !(figur is T))
                    return null;
                else if (figur.figurFarbe == team && figur is T)
                    return figur;
            }
        }
        return null;
    }

    private void BerechneStandartBewegungen()
    {
        float range = 1;
        foreach (var direction in richtungen)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = position + direction * i;
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
    }

    public override void BewegeFigur(Vector2Int coords)
    {
        base.BewegeFigur(coords);
        if (coords == linksRochade)
        {
            schachbrett.UpdateSchachbrettOnFigurBewegt(coords + Vector2Int.right, linkerTurm.position, linkerTurm, null);
            linkerTurm.BewegeFigur(coords + Vector2Int.right);
        }
        else if (coords == rechtsRochade)
        {
            schachbrett.UpdateSchachbrettOnFigurBewegt(coords + Vector2Int.left, rechterTurm.position, rechterTurm, null);
            rechterTurm.BewegeFigur(coords + Vector2Int.left);
        }
    }

}