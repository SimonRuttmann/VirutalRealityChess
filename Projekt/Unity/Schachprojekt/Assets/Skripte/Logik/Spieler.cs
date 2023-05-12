using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * 
 * The main reason to use System.Serializable for most users is so that your class and variables will show up in the inspector.
    If you added a public List of BaseTest to a component, you would be able to add and configure instances of BaseTest right in the inspector, with all that configured data available in the list at runtime. Otherwise you don't need to make your data classes Serializable.

 */
[System.Serializable] //Vermutung: Entweder zur Speicherung der Klasse oder Inspektor
public class Spieler
{
    public FigurFarbe Farbe;
    public Schachbrett Schachbrett;
    //Alle Figuren eines Spieler, welche sich auf dem Spielfeld befinden
    public List<Figur> AktiveFiguren;
    // Start is called before the first frame update

    //VORSICHT KONSTRUKTOR!
    public Spieler(FigurFarbe farbe, Schachbrett schachbrett)
    {
        AktiveFiguren = new List<Figur>();
        this.Schachbrett = schachbrett;
        this.Farbe = farbe;
    }

    public void AddFigur(Figur figur)
    {
        if (!AktiveFiguren.Contains(figur))
        {
            AktiveFiguren.Add(figur);
        }
    }

    public void RemoveFigur(Figur figur)
    {
        if (AktiveFiguren.Contains(figur))
        {
            AktiveFiguren.Remove(figur);
        }
    }

      public void GeneriereAlleMoeglichenZuege()
      {
          foreach(var figur in AktiveFiguren)
          {
              if (Schachbrett.HatFigur(figur))
              {
                figur.WaehleMoeglicheFelder();
              }
          }
      }

	public Figur[] GetPieceAtackingOppositePiceOfType<T>() where T : Figur
	{
		return AktiveFiguren.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
	}

	public Figur[] GetFigurenVomTyp<T>() where T : Figur
	{
		return AktiveFiguren.Where(p => p is T).ToArray();
	}

	public void EntferneAngriffsMoeglichkeitenAufFigur<T>(Spieler gegner, Figur gewaehlteFigur) where T : Figur
	{
		List<Vector2Int> coordsZumEntfernen = new List<Vector2Int>();

		coordsZumEntfernen.Clear();
		foreach (var coords in gewaehlteFigur.Bewegungsmöglichkeiten)
		{
			Figur pieceOnCoords = this.Schachbrett.GetFigurOnFeld(coords);
			Schachbrett.UpdateSchachbrettOnFigurBewegt(coords, gewaehlteFigur.position, gewaehlteFigur, null);
			gegner.GeneriereAlleMoeglichenZuege();
			if (gegner.CheckObEsFigurAngreift<T>())
				coordsZumEntfernen.Add(coords);
			Schachbrett.UpdateSchachbrettOnFigurBewegt(gewaehlteFigur.position, coords, gewaehlteFigur, pieceOnCoords);
		}
		foreach (var coords in coordsZumEntfernen)
		{
			gewaehlteFigur.Bewegungsmöglichkeiten.Remove(coords);
		}

	}

	internal bool CheckObEsFigurAngreift<T>() where T : Figur
	{
		foreach (var piece in AktiveFiguren)
		{
			if (Schachbrett.HatFigur(piece) && piece.IsAttackingPieceOfType<T>())
				return true;
		}
		return false;
	}

	public bool KannFigurVorAngriffRetten<T>(Spieler opponent) where T : Figur
	{
		foreach (var piece in AktiveFiguren)
		{
			foreach (var coords in piece.Bewegungsmöglichkeiten)
			{
				Figur pieceOnCoords = Schachbrett.GetFigurOnFeld(coords);
				Schachbrett.UpdateSchachbrettOnFigurBewegt(coords, piece.position, piece, null);
				opponent.GeneriereAlleMoeglichenZuege();
				if (!opponent.CheckObEsFigurAngreift<T>())
				{
					Schachbrett.UpdateSchachbrettOnFigurBewegt(piece.position, coords, piece, pieceOnCoords);
					return true;
				}
				Schachbrett.UpdateSchachbrettOnFigurBewegt(piece.position, coords, piece, pieceOnCoords);
			}
		}
		return false;
	}

	internal void OnSpielNeustart()
	{
		AktiveFiguren.Clear();
	}




}

