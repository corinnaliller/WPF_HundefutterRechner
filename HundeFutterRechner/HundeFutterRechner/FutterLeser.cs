using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HundeFutterRechner
{
    /**
     * Corinna Liller, BBM2H17M
     * Diese Klasse liest die hinterlegten Daten aus den beiden CSV-Dateien aus, die sich
     * im Debug-Ordner befinden und legt die Instanzen der Tierfutter-Objekte an.
     */
    class FutterLeser
    {
        public FutterLeser()
        {

        }

        public static List<Tierfutter> NassfutterLesen()
        {
            StreamReader nass = new StreamReader(@"FutterHundNass.csv");
            List<Tierfutter> nassfutter = new List<Tierfutter>();
            int zeile = 0;
            string[] ersteZeile = new string[24];
            while (!nass.EndOfStream)
            {
                var line = nass.ReadLine();
                var values = line.Split(';');
                
                if (zeile == 0)
                {
                    ersteZeile = values;
                    zeile++;
                }
                else
                {
                    string name = values[0];
                    string geschmack = values[1];
                    string aktivitaet = values[2];
                    FutterArt art;
                    switch(aktivitaet)
                    {
                        case "normal": art = FutterArt.normal;break;
                        case "aktiv": art = FutterArt.aktiv;break;
                        case "senior": art = FutterArt.senior;break;
                        case "Niere": art = FutterArt.niere;break;
                        default: art = FutterArt.normal;break;
                    }
                    Dictionary<float,float> tabelle = new Dictionary<float, float>();
                    for(int i = 3; i <values.Length;i++)
                    {
                        if(values[i] != "NA")
                        {
                            try
                            {
                                tabelle.Add(Convert.ToSingle(ersteZeile[i]), Convert.ToSingle(values[i]));
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    Tierfutter nf = new Tierfutter(name, geschmack, FutterSorte.nass, art, tabelle);
                    nassfutter.Add(nf);
                    //Console.WriteLine(nf);
                }
            }
            return nassfutter;
        }
        public static List<Tierfutter> TrockenfutterLesen()
        {
            StreamReader trocken = new StreamReader(@"FutterHundTrocken.csv");
            List<Tierfutter> trockenfutter = new List<Tierfutter>();
            int zeile = 0;
            string[] ersteZeile = new string[30];
            while (!trocken.EndOfStream)
            {
                var line = trocken.ReadLine();
                var values = line.Split(';');
                if (zeile == 0)
                {
                    ersteZeile = values;
                    zeile++;
                }
                else
                {
                    string name = values[0];
                    string geschmack = values[1];
                    string aktivitaet = values[2];
                    FutterArt art;
                    switch (aktivitaet)
                    {
                        case "normal": art = FutterArt.normal; break;
                        case "aktiv": art = FutterArt.aktiv; break;
                        case "senior": art = FutterArt.senior; break;
                        case "Niere": art = FutterArt.niere; break;
                        default: art = FutterArt.normal; break;
                    }
                    Dictionary<float, float> tabelle = new Dictionary<float, float>();
                    for (int i = 3; i < values.Length; i++)
                    {
                        if (values[i] != "NA")
                        {
                            try
                            {
                                tabelle.Add(Convert.ToSingle(ersteZeile[i]), Convert.ToSingle(values[i]));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    Tierfutter tf = new Tierfutter(name, geschmack, FutterSorte.trocken, art, tabelle);
                    trockenfutter.Add(tf);
                }
            }
            return trockenfutter;
        }
    }
}
