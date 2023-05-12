using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;

public class LaserPointerWrap : MonoBehaviour
{
    [SerializeField] Schachbrett schachbrett;
    protected IInputHandler[] inputhandlers; 
    private SteamVR_LaserPointer steamVrLaserPointer;
    [SerializeField] VrSchachMenu vrSchachMenu;
    [SerializeField] SchachManager schachManager;
    protected IInputHandler[] inputHandlers;

    private void Awake()
    {
    steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
        steamVrLaserPointer.PointerIn += OnPointerIn;
        steamVrLaserPointer.PointerOut += OnPointerOut;
        steamVrLaserPointer.PointerClick += OnPointerClick;
    }

    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        if(e.target.GetComponent<Figur>() != null)
        {
            Vector3 v3 = e.target.position;
            schachbrett.OnFeldAuswahl(v3);
        }
        else if (e.target.tag == "VrButton")
        {
            string id = e.target.name;
            switch (id)
            {
                case "Startbutton": this.vrSchachMenu.hideUI(); break;
                case "SwitchButton": this.vrSchachMenu.wechsleBewegungsmodus(); break;
                case "Neustartbutton": this.schachManager.RestartGame(); this.vrSchachMenu.hideUI(); break;
                case "BeendenButton": this.vrSchachMenu.beendeSpiel(); break;  //Beende ist auskommentiert
            }

        }
        else
        {
            schachbrett.OnFeldAuswahl(e.hit);
        }   
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        IPointerExitHandler pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();
        if (pointerExitHandler == null)
        {
            return;
        }

        pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));
    }

    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        IPointerEnterHandler pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();
        if (pointerEnterHandler == null)
        {
            return;
        }

        pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
    }
}
