using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HundeFutterRechner
{
    /**
     * Corinna Liller, BBM1H17M
     * Der Versuch der Auslagerung der Controller-Funktion
     */
    class FutterRechnerController
    {
        
        public FutterRechnerController()
        {
            
        }
        public float FutterMengeBerechnen(Tierfutter futter, FutterSorte sorte, float gewicht, int portionen)
        {
            float antwort = 0;
            if (futter != null)
            {
                antwort = futter.FuttermengeBerechnen(gewicht) / portionen;
            }
            else
            {
                throw new InvalidOperationException("Kein Futter übergeben");
            }

            return antwort;
        }
        public float[] GemischteFutterMenge(Tierfutter futter_trocken, Tierfutter futter_nass, float anteil_trocken, float gewicht, int portionen)
        {
            float[] antwort = new float[2];
            if(futter_trocken == null || futter_nass == null)
            {
                throw new InvalidOperationException("Kein Futter übergeben");
            }
            if(anteil_trocken > 99)
            {
                return new float[] { futter_trocken.FuttermengeBerechnen(gewicht), 0 };
            }
            else if(anteil_trocken < 1)
            {
                return new float[] { 0, futter_nass.FuttermengeBerechnen(gewicht) };
            }
            else
            {
                antwort[0] = ((anteil_trocken / 100) * futter_trocken.FuttermengeBerechnen(gewicht)) / portionen;
                antwort[1] = (((100-anteil_trocken) / 100) * futter_nass.FuttermengeBerechnen(gewicht)) / portionen;
            }
            return antwort;
        }
        public float WieVieleDosen(float menge, int dosengroesse)
        {
            return menge / dosengroesse;
        }
        public int WelcheDose(int index)
        {
            switch(index)
            {
                case 0: return 1240;
                case 1: return 800;
                case 2: return 400;
                case 3: return 200;
                case 4: return 185;
                case 5: return 125;
                default: return 400;
            }
        }
    }
}
