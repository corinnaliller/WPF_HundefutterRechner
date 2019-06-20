using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HundeFutterRechner
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FutterListe tf, nf;

        FutterRechnerController controller;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void cb_wunsch_Checked(object sender, RoutedEventArgs e)
        {
            tb_wunsch.Visibility = Visibility.Visible;
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void cb_wunsch_Unchecked(object sender, RoutedEventArgs e)
        {
            tb_wunsch.Visibility = Visibility.Hidden;
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void rb_gemischt_Unchecked(object sender, RoutedEventArgs e)
        {
            mischer.Visibility = Visibility.Hidden;
            
        }

        private void rb_gemischt_Checked(object sender, RoutedEventArgs e)
        {
            comboboxes.Visibility = Visibility.Visible;
            mischer.Visibility = Visibility.Visible;
            nassFutter.Visibility = Visibility.Visible;
            trockenFutter.Visibility = Visibility.Visible;
            l_dosengroesse.Visibility = Visibility.Visible;
            dosengroesse.Visibility = Visibility.Visible;
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void rb_trocken_Checked(object sender, RoutedEventArgs e)
        {
            comboboxes.Visibility = Visibility.Visible;
            nassFutter.Visibility = Visibility.Hidden;
            trockenFutter.Visibility = Visibility.Visible;
            l_dosengroesse.Visibility = Visibility.Hidden;
            dosengroesse.Visibility = Visibility.Hidden;
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void rb_nass_Checked(object sender, RoutedEventArgs e)
        {
            comboboxes.Visibility = Visibility.Visible;
            nassFutter.Visibility = Visibility.Visible;
            trockenFutter.Visibility = Visibility.Hidden;
            l_dosengroesse.Visibility = Visibility.Visible;
            dosengroesse.Visibility = Visibility.Visible;
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void b_berechnen_Click(object sender, RoutedEventArgs e)
        {
            sb_Fehlermeldung.Content = "";
            napf_halb.Visibility = Visibility.Hidden;
            napf_voll.Visibility = Visibility.Hidden;
            napf_sehr_voll.Visibility = Visibility.Hidden;
            if(tb_gewicht.Text != "")
            {
                if ((rb_gemischt.IsChecked == true && nassFutter.SelectedItem != null && trockenFutter.SelectedItem != null) || (rb_nass.IsChecked==true && nassFutter.SelectedItem != null) || (rb_trocken.IsChecked == true && trockenFutter.SelectedItem != null))
                {
                    Storyboard story = FindResource("button_gone") as Storyboard;
                    story.Begin(sender as Button);

                    try
                    {
                        float gewicht = 0;
                        float[] menge = { 0, 0 };
                        int portionen = cobox_portionen.SelectedIndex+1;
                        if (cb_wunsch.IsChecked == true)
                        {
                            if (tb_wunsch.Text == "")
                            {
                                throw new InvalidOperationException("Kein Wunschgewicht eingegeben!");
                            }
                            if(Convert.ToSingle(tb_gewicht.Text) > (Convert.ToSingle(tb_wunsch.Text)*1.2))
                            {
                                throw new Exception("So viel sollte Ihr Hund nur unter ärztlicher Aufsicht abnehmen!");
                            }
                            gewicht = Convert.ToSingle(tb_wunsch.Text);
                        }
                        else
                        {
                            gewicht = Convert.ToSingle(tb_gewicht.Text);
                        }
                        if (gewicht > 100 || gewicht <= 0)
                        {
                            throw new Exception("Unsinnige Gewichtsangabe");
                        }
                        if (rb_trocken.IsChecked == true)
                        {
                            menge[0] = controller.FutterMengeBerechnen(trockenFutter.SelectedItem as Tierfutter, FutterSorte.trocken, gewicht, portionen);
                            ergebnis.Content = Convert.ToInt32(menge[0]) + " g pro Portion";
                            
                        }
                        else if (rb_nass.IsChecked == true)
                        {
                            int groesse = controller.WelcheDose(dosengroesse.SelectedIndex);
                            menge[0] = controller.FutterMengeBerechnen(nassFutter.SelectedItem as Tierfutter, FutterSorte.nass, gewicht,portionen);
                            float dosen = controller.WieVieleDosen(menge[0], groesse);
                            ergebnis.Content = Convert.ToInt32(menge[0]) + " g ("+Math.Round(dosen,2) +" Dosen) pro Portion";
                        }
                        else if (rb_gemischt.IsChecked == true)
                        {
                            float anteil_trocken = Convert.ToSingle(sl_mix.Value);
                            int groesse = controller.WelcheDose(dosengroesse.SelectedIndex);
                            menge = controller.GemischteFutterMenge(trockenFutter.SelectedItem as Tierfutter, nassFutter.SelectedItem as Tierfutter, anteil_trocken, gewicht, portionen);
                            float dosen = controller.WieVieleDosen(menge[1], groesse);
                            ergebnis.Content = Convert.ToInt32(menge[0]) + " g Trockenfutter und "+ Convert.ToInt32(menge[1]) + " g Nassfutter(" + Math.Round(dosen, 2) + " Dosen) pro Portion";
                        }
                        Storyboard sb = FindResource("tuete_kippen") as Storyboard;
                        sb.Begin();
                        Storyboard sb2 = FindResource("futter_faellt") as Storyboard;
                        sb2.Begin();
                        Storyboard sb3 = FindResource("napf_fuellen") as Storyboard;
                        sb3.Begin();
                        antwort.Visibility = Visibility.Visible;

                    }
                    catch (Exception ex)
                    {
                        sb_Fehlermeldung.Content = ex.Message;
                    }
                }
                
            }
            
            
        }

        private void sl_mix_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            int value = Convert.ToInt32(sl_mix.Value);
            if(value > 99)
            {
                nassFutter.Visibility = Visibility.Hidden;
            }
            else if(value < 1)
            {
                trockenFutter.Visibility = Visibility.Hidden;
            }
            else
            {
                nassFutter.Visibility = Visibility.Visible;
                trockenFutter.Visibility = Visibility.Visible;
                mix_trocken.Content = "Trocken: " + value + " %";
                mix_nass.Content = "Nass: " + (100 - value) + " %";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            List<Tierfutter> trocken = FutterLeser.TrockenfutterLesen();
            List<Tierfutter> nass = FutterLeser.NassfutterLesen();
            controller = new FutterRechnerController();
            tf = new FutterListe();
            tf = FindResource("res_trocken") as FutterListe;
            foreach(Tierfutter futter in trocken)
            {
                tf.Add(futter);
            }
            nf = new FutterListe();
            nf = FindResource("res_nass") as FutterListe;
            foreach(Tierfutter futter in nass)
            {
                nf.Add(futter);
            }
        }
        private void nassFutter_DropDownOpened(object sender, EventArgs e)
        {
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void cobox_portionen_DropDownOpened(object sender, EventArgs e)
        {
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void dosengroesse_DropDownOpened(object sender, EventArgs e)
        {
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void tb_gewicht_TextChanged(object sender, TextChangedEventArgs e)
        {
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void tb_wunsch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

        private void trockenFutter_DropDownOpened(object sender, EventArgs e)
        {
            Storyboard story = FindResource("button_back") as Storyboard;
            story.Begin();
        }

    }
}
