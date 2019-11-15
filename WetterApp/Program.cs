using System;
using System.Collections.Generic;
namespace Wetter
    /*
     * Aufgaben bearbeitung von Jacky Cheung 
     * am 14.11 und 15.11.2019
     */
{
    class WetterMain
    {
        static void Main(string[] args)
        {
            WetterDatenquelle wdq = new WetterDatenquelle();
            WetterApp wa = new WetterApp();
            Wetterdaten wd;
            // Hier m�ssen unsere Eventhandler beim Herausgeber registriert
            // werden. Oder anders ausgedr�ckt: hier abonnieren unsere
            // Eventhandler das Ereignis "Neue Wetterdaten eingetroffen".
            // ============================================================
            wdq.TrageAbonnentenEin(wa.TrageNeueWetterdatenEin);
            wdq.TrageAbonnentenEin(wa.ZeigeTemperaturverlaufAn);
            wdq.TrageAbonnentenEin(wa.WetterVorhersage);
            wdq.EntferneAbonnenten(wa.ZeigeTemperaturverlaufAn);
            Console.WriteLine("Durch Eingabe eines Temperaturwertes wird das Ereignis\n" +
            "\"Neue Wetterdaten eingetroffen\" simuliert.\n");
            bool eingabeOK;
            for (; ; )
            {
                Console.Write("Geben Sie bitte einen Temperaturwert ein (Abbruch mit <enter>): ");
                wd = new Wetterdaten();
                eingabeOK = double.TryParse(Console.ReadLine(), out wd.temperaturwert);
                if (!eingabeOK)
                    break;
                wd.zeit = DateTime.Now;
                // Hier wird das Ereignis "Neue Wetterdaten eingetroffen" ausgel�st:
                // ================================================================
                wdq.BieteNeueWetterdatenAn(wd);
            }
        }
    }
    class WetterApp
    {
        List<Wetterdaten> wetterdatenListe = new List<Wetterdaten>();
        public void TrageNeueWetterdatenEin(Wetterdaten wd)
        {
            wetterdatenListe.Add(wd);
            Console.WriteLine("Neue Wetterdaten wurden eingetragen.");
        }
        public void ZeigeTemperaturverlaufAn(Wetterdaten wd)
        {
            Console.WriteLine("Temperaturverlauf:");
            // Hier werden alle bereits gespeicherten Wetterdaten angezeigt:
            // ============================================================
            foreach (Wetterdaten wd1 in wetterdatenListe)
            {
                Console.WriteLine("{0,2:D2}:{1,2:D2}:{2,2:D2} - {3}",
                wd1.zeit.Hour, wd1.zeit.Minute, wd1.zeit.Second, wd1.temperaturwert);
            }
        }

        public void WetterVorhersage(Wetterdaten wd)
        {
            double summeDerTemps = 0;
            
            for(int i = 0; i < wetterdatenListe.Count; i++)
            {
                summeDerTemps += wetterdatenListe[i].temperaturwert;
            }
            double avgTemperaturen = summeDerTemps / wetterdatenListe.Count  ;
            Console.WriteLine("Vorhersage der nächste Tag wird " + avgTemperaturen);
        }
    }
    class WetterDatenquelle
    {
        public delegate void NeueDatenEventHandler(Wetterdaten temperaturwert);
        private NeueDatenEventHandler BenachrichtigeAlleAbonnenten;
        // Ein Eventhandler, der als Abonnent registriert werden m�chte, muss
        // sich durch Aufruf der Funktion TrageAbonnentenEin registrieren:
        // ============================================================
        public void TrageAbonnentenEin(NeueDatenEventHandler ndeh)
        {
            BenachrichtigeAlleAbonnenten += ndeh;
        }
        public void BieteNeueWetterdatenAn(Wetterdaten wetterdaten)
        {
            if (BenachrichtigeAlleAbonnenten != null)
                BenachrichtigeAlleAbonnenten(wetterdaten);
        }

        public void EntferneAbonnenten(NeueDatenEventHandler ndeh)
        {
            BenachrichtigeAlleAbonnenten -= ndeh;
        }
    }
    class Wetterdaten
    {
        public DateTime zeit;
        public double temperaturwert;
    }
}