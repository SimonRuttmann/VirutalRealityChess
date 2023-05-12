using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springer : Figur
{
	Vector2Int[] springfelder = new Vector2Int[]
	{
		new Vector2Int(2, 1),
		new Vector2Int(2, -1),
		new Vector2Int(1, 2),
		new Vector2Int(1, -2),
		new Vector2Int(-2, 1),
		new Vector2Int(-2, -1),
		new Vector2Int(-1, 2),
		new Vector2Int(-1, -2),
	};

	public override List<Vector2Int> WaehleMoeglicheFelder()
	{
		Bewegungsmöglichkeiten.Clear();

		for (int i = 0; i < springfelder.Length; i++)
		{
			Vector2Int nextCoords = position + springfelder[i];
			Figur figur = schachbrett.GetFigurOnFeld(nextCoords);
			if (!schachbrett.CheckObCoordsAufFeld(nextCoords))
				continue;
			if (figur == null || !figur.IstGleichesTeam(this))
				AddBewegungsmoeglichkeit(nextCoords);
		}
		return Bewegungsmöglichkeiten;
	}
}