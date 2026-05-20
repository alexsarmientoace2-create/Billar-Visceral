using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    public static GameManager gM;
    public static bool TurnoJugador1;
    public static int Jugador1esLisa = 0;
    public static int BolasRestantesJugador1 = 8;
    public static int BolasRestantesJugador2 = 8;
    public GameObject CanvasVictoria1;
    public GameObject CanvasVictoria2;


    private void Awake()
    {
        TurnoJugador1 = true;
        gM = this;
    }

    public void CambioDeTurno()
    {
        TurnoJugador1 = !TurnoJugador1;
    }
    public void BolaLisaEntra()
    {
        if (Jugador1esLisa == 0 && TurnoJugador1 == true)
        {
            Jugador1esLisa = 1;
        }
        else if (Jugador1esLisa == 0 && TurnoJugador1 == false)
        {
            Jugador1esLisa = 2;
        }
        else if (TurnoJugador1 == true && Jugador1esLisa == 1)
        {
            PuntoJugador1();
        }
        else if (TurnoJugador1 == false && Jugador1esLisa == 2)
        {
            PuntoJugador2();
        }
        else if (TurnoJugador1 == true && Jugador1esLisa == 2)
        {
            FaltaJugador1();
            BolasRestantesJugador2--;
        }
        else if (TurnoJugador1 == false && Jugador1esLisa == 1)
        {
            FaltaJugador2();
            BolasRestantesJugador1--;
        }
    }

    public void BolaRayadaEntra()
    {
        if (Jugador1esLisa == 0 && TurnoJugador1 == true)
        {
            Jugador1esLisa = 2;
        }
        else if (Jugador1esLisa == 0 && TurnoJugador1 == false)
        {
            Jugador1esLisa = 1;
        }
        else if (TurnoJugador1 == true && Jugador1esLisa == 1)
        {
            FaltaJugador1();
            BolasRestantesJugador2--;
        }
        else if (TurnoJugador1 == false && Jugador1esLisa == 2)
        {
            FaltaJugador2();
            BolasRestantesJugador1--;
        }
        else if (TurnoJugador1 == true && Jugador1esLisa == 2)
        {
            PuntoJugador1();
        }
        else if (TurnoJugador1 == false && Jugador1esLisa == 1)
        {
            PuntoJugador2();
        }
    }


    public void Bolanegraentra()
    {
        if (TurnoJugador1 == true && BolasRestantesJugador1 == 1)
        {
            VictoriaJugador1();
        }
        else if (TurnoJugador1 == false && BolasRestantesJugador2 == 1)
        {
            VictoriaJugador2();
        }
        else if (TurnoJugador1 == true && BolasRestantesJugador1 > 1)
        {
            VictoriaJugador2();
        }
        else if (TurnoJugador1 == false && BolasRestantesJugador2 > 1)
        {
            VictoriaJugador1();
        }
    }

    public void PuntoJugador1()
    {
        BolasRestantesJugador1--;
    }

    public void PuntoJugador2()
    {
        BolasRestantesJugador2--;
    }

    void FaltaJugador1()
    {

    }

    public void FaltaJugador2()
    {

    }

    public void VictoriaJugador1()
    {
        Time.timeScale = 0;
        CanvasVictoria1.SetActive(true);
    }

    public void VictoriaJugador2()
    {
        Time.timeScale = 0;
        CanvasVictoria2.SetActive(true);
    }
}

