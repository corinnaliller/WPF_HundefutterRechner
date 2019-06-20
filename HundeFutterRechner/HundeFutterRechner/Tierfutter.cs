using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HundeFutterRechner
{
    /**
     * Corinna Liller, BBM2H17M
     * Die Futtersorten werden aus einer Datei eingelesen. (Siehe FutterLeser).
     * Diese enthält die Angaben zur Futtermenge für unterschiedliche Gewichtsklassen von Hunden,
     * die sich auf den Tüten (Trockenfutter) bzw. Dosen (Nassfutter) befinden. Diese werden in einem
     * Dictionary <Gewicht,Menge> gespeichert.
     * Um die von den angegebenen Werten abweichenden Gewichtsangaben abzudecken, wird eine
     * Regressionsgerade durch die Datenpunkte gelegt.
     */
    public enum FutterSorte
    {
        trocken, nass
    }
    public enum FutterArt
    {
        normal, aktiv, senior, niere
    }
    public class Tierfutter
    {
        private float steigung;
        private float achsenschnitt;
        private string name;

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != "" && value != null)
                {
                    name = value;
                }
                else
                {
                    throw new InvalidOperationException("Name darf nicht leer sein!");
                }
            }
        }
        public string Geschmack { get; set; }
        public FutterSorte Sorte { get; set; }
        public FutterArt Art { get; set; }
        public IDictionary<float, float> Tabelle { get; set; }
        #endregion

        public Tierfutter(string name, string geschmack, FutterSorte sorte, FutterArt art, Dictionary<float, float> tabelle)
        {
            Name = name;
            Geschmack = geschmack;
            Sorte = sorte;
            Art = art;
            Tabelle = tabelle;
            RegressionsgeradeBerechnen();
        }

        #region Private Methods
        private void RegressionsgeradeBerechnen()
        {
            steigung = CovarianzBerechnen() / VarianzXBerechnen();
            float xM = MittelwertX();
            float yM = MittelwertY();
            achsenschnitt = yM - steigung * xM;
        }
        private float CovarianzBerechnen()
        {
            float xM = MittelwertX();
            float yM = MittelwertY();
            float covar = 0;
            foreach (KeyValuePair<float, float> tab in Tabelle)
            {
                covar = (tab.Key - xM) * (tab.Value - yM);
            }
            return covar;
        }
        private float VarianzXBerechnen()
        {
            float xM = MittelwertX();
            float varX = 0;
            foreach (KeyValuePair<float, float> tab in Tabelle)
            {
                varX += (tab.Key - xM) * (tab.Key - xM);
            }
            return varX;
        }
        private float MittelwertX()
        {
            float mw = 0;
            foreach (KeyValuePair<float, float> tab in Tabelle)
            {
                mw += tab.Key;
            }
            mw = mw / Tabelle.Count;
            return mw;
        }
        private float MittelwertY()
        {
            float mw = 0;
            foreach (KeyValuePair<float, float> tab in Tabelle)
            {
                mw += tab.Value;
            }
            mw = mw / Tabelle.Count;
            return mw;
        }
        #endregion

        #region Öffentliche Methoden
        public float FuttermengeBerechnen(float gewicht)
        {
            return achsenschnitt + steigung * gewicht;
        }
        public override string ToString()
        {
            string s = "";
            switch (Art)
            {
                case FutterArt.aktiv: s = "(aktiv)"; break;
                case FutterArt.normal: s = "(normal)"; break;
                case FutterArt.niere: s = "(Niere)"; break;
                case FutterArt.senior: s = "(Senior)"; break;
            }
            return Name + " - " + Geschmack + " " + s;
        }
        #endregion
    }
}
