using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool TurnoJugador1;
    public static int Jugador1esLisa = 0;
    public static int BolasRestantesJugador1 = 8;
    public static int BolasRestantesJugador2 = 8;


    private void Awake()
    {
        TurnoJugador1 = true;
    }

    void bolaLisaEntra()
    {
        if (TurnoJugador1 == true && Jugador1esLisa == 1)
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
        else if (TurnoJugador1 == true && Jugador1esLisa == 0)
        {
            Jugador1esLisa = 1;
        }
        else if (TurnoJugador1 == false && Jugador1esLisa == 0)
        {
            Jugador1esLisa = 2;
        }
    }

    void bolaRayadaEntra()
    {
        if (TurnoJugador1 == true && Jugador1esLisa == 1)
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
        else if (TurnoJugador1 == true && Jugador1esLisa == 0)
        {
            Jugador1esLisa = 2;
        }
        else if (TurnoJugador1 == false && Jugador1esLisa == 0)
        {
            Jugador1esLisa = 1;
        }
    }


    void bolanegraentra()
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

        void PuntoJugador1()
        {
        BolasRestantesJugador1--;
        }

        void PuntoJugador2()
        {
        BolasRestantesJugador2--;
        }

        void FaltaJugador1()
        {

        }

        void FaltaJugador2()
        {

        }

        void VictoriaJugador1()
        {

        }

        void VictoriaJugador2()
        {

        }
    }

