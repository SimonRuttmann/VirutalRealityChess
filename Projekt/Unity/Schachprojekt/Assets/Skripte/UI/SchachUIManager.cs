using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchachUIManager : MonoBehaviour
{
	[SerializeField] private GameObject teamanzeige;
	[SerializeField] private Text teamtext;
	[SerializeField] private GameObject neustartanzeige;
	[SerializeField] private Text neustarttext;
	[SerializeField] private GameObject fadenkreuz;

	[SerializeField] private GameObject hauptmenue;
	[SerializeField] private Button startButton;
	[SerializeField] private Text startText;
	[SerializeField] private Button beendenButton;
	[SerializeField] private Text beendenText;
	 
	//Wird beim Start ausgeführt
	//Beim ersten Start
	public void startUI()
    {
		hauptmenue.SetActive(true);
		
		teamanzeige.SetActive(false);
		neustartanzeige.SetActive(false);
		fadenkreuz.SetActive(false);
    }

	//Spiel starten/Fortsetzen button wird gedrückt
	//Alle weiteren spielstarts wird diese methode aufgerufen
	public void SpielStarten()
    {
		startText.text = "Fortsetzen";
		hauptmenue.SetActive(false);
		
		teamanzeige.SetActive(true);
		fadenkreuz.SetActive(true);
		neustartanzeige.SetActive(false);
    }

	//Wird bei nach einem Schachzug gesetzt
	public void SetTeamanzeige(FigurFarbe farbe)
    {
		if (farbe == FigurFarbe.weiss) teamtext.text = "Team Weiß ist an der Reihe";
		else teamtext.text = "Team Schwarz ist an der Reihe";
    }

	//Wird nach einem Schachmat angezeigt
	public void OnGameFinished(string winner)
	{
		fadenkreuz.SetActive(true);
		teamanzeige.SetActive(false);
		neustartanzeige.SetActive(true);
		neustarttext.text = "Spieler " + winner + "hat gewonnen!\n Drücke X um das Spiel neu zu starten.";
	}

	public void beendeSpiel()
    {
		Application.Quit();
    }
}
