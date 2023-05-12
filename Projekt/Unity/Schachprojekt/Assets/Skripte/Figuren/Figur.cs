using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Figur : MonoBehaviour
{
	public AudioSource bewegungSound;
	public AudioSource angriffSound;
	public AudioSource sterbeSound;
	public AudioSource idleSound;
	private Quaternion drehgradStart;
	private Quaternion drehgradEnde;
	private float timeCount = 0.0f;
	private bool drehungAktiv;
	public void DreheFigur(float drehung)
	{
		this.drehgradEnde = Quaternion.Euler(0, drehung, 0);
		this.drehgradStart = transform.localRotation;
		this.drehungAktiv = true;
		timeCount = 0;
	}
    public void Update()
    {

		if (transform.rotation != this.drehgradEnde && drehungAktiv)
		{
			transform.rotation = Quaternion.Slerp(this.drehgradStart, this.drehgradEnde, timeCount);
			timeCount = timeCount + Time.deltaTime;
		}
		if (transform.rotation == this.drehgradEnde)
        {
			this.drehungAktiv = false;
        }
	}

    public Animator animator;
	public void IdleAnimation()
    {
		animator.SetTrigger("IdleTrigger");
		idleSound.Play();    
	}

	public void SterbeAnimation()
    {
		animator.SetTrigger("SterbeTrigger");
		sterbeSound.Play();
	}

	public void AngriffAnimation()
    {
		animator.SetTrigger("AngriffTrigger");
		angriffSound.Play();
    }

	public Schachbrett schachbrett;

	public Vector2Int position; // Das aktuell belegte Feld

	public FigurFarbe figurFarbe;

	public bool WurdeBewegt; //F�r Rochade & Bauern
	public List<Vector2Int> Bewegungsm�glichkeiten;

	private IBeweger beweger;

	public abstract List<Vector2Int> WaehleMoeglicheFelder();

	// Pseudo Konstruktor
	private void Awake()
	{
		this.animator = GetComponent<Animator>();
		beweger = GetComponent<IBeweger>();
		Bewegungsm�glichkeiten = new List<Vector2Int>();
		WurdeBewegt = false;
		this.bewegungSound.volume = 0.10f;
	}

	// Aufruf um der Figur alle Daten hinzuzuf�gen nachdem sie erstellt wurde
	public void SetzeFigurdaten(Vector2Int position, FigurFarbe team, Schachbrett schachbrett)
	{
		this.figurFarbe = team;
		this.position = position;
		this.schachbrett = schachbrett;

		//Figur entsprechende Position hinzuf�gen
		transform.position = this.schachbrett.RelativePositionZumSchachbrettfeld(position);
		if (this.figurFarbe == FigurFarbe.weiss)
        {
			transform.Rotate(0, 180, 0); 
		}
	}


	public bool IstGleichesTeam(Figur figur) { return this.figurFarbe == figur.figurFarbe; }
	public bool BewegungMoeglichZu(Vector2Int position) { return Bewegungsm�glichkeiten.Contains(position); }
	protected void AddBewegungsmoeglichkeit(Vector2Int position) { Bewegungsm�glichkeiten.Add(position); }

	public virtual void BewegeFigur(Vector2Int coords)
	{
		Vector3 zielPos = schachbrett.KalkulierePosVonCoords(coords);
		position = coords;
		WurdeBewegt = true;
		bewegungSound.Play();
		beweger.MoveTo(transform, zielPos);
		
	}

	public bool IsAttackingPieceOfType<T>() where T : Figur
	{
		foreach (var feld in Bewegungsm�glichkeiten)
		{
			if (schachbrett.GetFigurOnFeld(feld) is T)
				return true;
		}
		return false;
	}
	public void SterbeOhneSound()
	{
		animator.SetTrigger("SterbeTrigger");
	}

}