/*
 * Projet  : Calculatrice WPF
 * Auteur  : Tedy Barthod - BTS CIEL 1
 * Date    : octobre 2026
 * Fichier : MainWindow.xaml.cs
 * Rôle    : ce qui se passe quand on clique sur les boutons
 *
 * Fonctionnement :
 * - on tape un nombre, il s'écrit sur l'écran
 * - on clique sur un opérateur : le nombre va dans NB1 et l'opérateur dans operation
 * - on tape le 2e nombre puis = : il va dans NB2 et on appelle la bonne fonction
 */

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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Barthod_Tedy_Calculatrice_2026
{
    public partial class MainWindow : Window
    {
        // déclaration des variables
        double NB1 = 0;          // premier nombre
        double NB2 = 0;          // deuxième nombre
        double result = 0;       // résultat du calcul
        double memoire = 0;      // nombre gardé avec M+ et M-
        char operation = ' ';    // opérateur choisi, ' ' = pas encore choisi
        bool erreur = false;     // true quand l'écran affiche une erreur
        bool nouveau = false;    // true quand le prochain chiffre doit effacer l'écran
        bool virgule = false;    // true quand le nombre a déjà une virgule

        public MainWindow()
        {
            InitializeComponent();
        }

        // ----- Fonctions de calcul -----

        double Addition(double a, double b)
        {
            return a + b;
        }

        double Soustraction(double a, double b)
        {
            return a - b;
        }

        double Multiplication(double a, double b)
        {
            return a * b;
        }

        double Division(double a, double b)
        {
            return a / b;
        }

        double Pourcentage(double a)
        {
            return a / 100;
        }

        double Racine(double a)
        {
            return Math.Sqrt(a);
        }

        double Signe(double a)
        {
            return -a;
        }

        // ----- Fonctions utilisées par plusieurs boutons -----

        // Transforme le texte de l'écran en nombre.
        // Si on a tapé autre chose qu'un nombre (ex : abc), on affiche
        // une erreur au lieu de faire planter le programme.
        double Lire(string texte)
        {
            double nombre = 0;

            try
            {
                nombre = double.Parse(texte);
            }
            catch (Exception)
            {
                AfficherErreur("Entrée invalide");
            }

            return nombre;
        }

        // Remet tout à zéro (sauf la mémoire)
        void Effacer()
        {
            TB_Display.Text = "";
            TB_Expression.Text = "";
            NB1 = 0;
            NB2 = 0;
            result = 0;
            operation = ' ';
            erreur = false;
            nouveau = false;
            virgule = false;
        }

        // Affiche un message d'erreur à la place du nombre
        void AfficherErreur(string message)
        {
            TB_Display.Text = message;
            TB_Expression.Text = "";
            operation = ' ';
            erreur = true;
        }

        // Avant d'écrire un chiffre : on enlève l'erreur ou l'ancien résultat
        void PreparerSaisie()
        {
            if (erreur)
            {
                Effacer();
            }

            if (nouveau)
            {
                TB_Display.Text = "";
                nouveau = false;
                virgule = false;

                // si le calcul d'avant est fini, on efface aussi le rappel
                if (operation == ' ')
                {
                    TB_Expression.Text = "";
                }
            }
        }

        // Ajoute un chiffre à la fin du nombre
        void AjouterChiffre(string chiffre)
        {
            PreparerSaisie();

            if (TB_Display.Text == "0")
            {
                TB_Display.Text = chiffre;   // pour ne pas avoir "05"
            }
            else
            {
                TB_Display.Text = TB_Display.Text + chiffre;
            }
        }

        // Fait le calcul NB1 (operation) NB2 et met le résultat dans result
        void Calculer()
        {
            NB2 = Lire(TB_Display.Text);

            if (operation == '+')
            {
                result = Addition(NB1, NB2);
            }
            else if (operation == '-')
            {
                result = Soustraction(NB1, NB2);
            }
            else if (operation == '*')
            {
                result = Multiplication(NB1, NB2);
            }
            else if (operation == '/')
            {
                if (NB2 == 0)
                {
                    AfficherErreur("Division par 0");
                }
                else
                {
                    result = Division(NB1, NB2);
                }
            }

            // plus de 12 chiffres : le nombre ne tient plus sur l'écran
            if (!erreur && (result > 999999999999 || result < -999999999999))
            {
                AfficherErreur("Nombre trop grand");
            }
        }

        // Garde le premier nombre et l'opérateur choisi
        void ChoisirOperateur(char op)
        {
            if (!erreur)
            {
                if (TB_Display.Text != "" && operation != ' ')
                {
                    // un calcul attend déjà (ex : 2 + 3 puis *), on le fait d'abord
                    Calculer();
                    NB1 = result;
                }
                else if (TB_Display.Text != "")
                {
                    NB1 = Lire(TB_Display.Text);
                }

                // il faut un premier nombre pour choisir un opérateur
                // (si l'écran est vide, on change juste d'opérateur)
                if (!erreur && (TB_Display.Text != "" || operation != ' '))
                {
                    operation = op;
                    TB_Expression.Text = NB1.ToString() + " " + op;
                    TB_Display.Text = "";
                    nouveau = false;
                    virgule = false;
                }
            }
        }

        // ----- Chiffres -----

        private void BTN_0_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("0");
        }

        private void BTN_1_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("1");
        }

        private void BTN_2_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("2");
        }

        private void BTN_3_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("3");
        }

        private void BTN_4_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("4");
        }

        private void BTN_5_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("5");
        }

        private void BTN_6_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("6");
        }

        private void BTN_7_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("7");
        }

        private void BTN_8_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("8");
        }

        private void BTN_9_Click(object sender, RoutedEventArgs e)
        {
            AjouterChiffre("9");
        }

        // ----- Virgule -----

        private void BTN_Virgule_Click(object sender, RoutedEventArgs e)
        {
            PreparerSaisie();

            // une seule virgule par nombre
            if (!virgule)
            {
                if (TB_Display.Text == "")
                {
                    TB_Display.Text = "0,";
                }
                else
                {
                    TB_Display.Text = TB_Display.Text + ",";
                }
                virgule = true;
            }
        }

        // ----- Opérateurs -----

        private void BTN_Plus_Click(object sender, RoutedEventArgs e)
        {
            ChoisirOperateur('+');
        }

        private void BTN_Moins_Click(object sender, RoutedEventArgs e)
        {
            ChoisirOperateur('-');
        }

        private void BTN_Fois_Click(object sender, RoutedEventArgs e)
        {
            ChoisirOperateur('*');
        }

        private void BTN_Divise_Click(object sender, RoutedEventArgs e)
        {
            ChoisirOperateur('/');
        }

        // ----- Égal et C -----

        private void BTN_Egal_Click(object sender, RoutedEventArgs e)
        {
            // il faut un opérateur et un deuxième nombre
            if (!erreur && operation != ' ' && TB_Display.Text != "")
            {
                Calculer();

                if (!erreur)
                {
                    TB_Expression.Text = NB1.ToString() + " " + operation + " " + NB2.ToString() + " =";
                    TB_Display.Text = result.ToString();
                    operation = ' ';   // calcul fini
                    nouveau = true;    // le prochain chiffre commence un nouveau calcul
                }
            }
        }

        private void BTN_C_Click(object sender, RoutedEventArgs e)
        {
            Effacer();
        }

        // ----- +/-  %  racine -----
        // Ces boutons changent le nombre affiché.
        // Après, le prochain chiffre commence un nouveau nombre.

        private void BTN_Signe_Click(object sender, RoutedEventArgs e)
        {
            if (!erreur && TB_Display.Text != "")
            {
                double nombre = Lire(TB_Display.Text);

                if (!erreur && nombre != 0)
                {
                    TB_Display.Text = Signe(nombre).ToString();
                    nouveau = true;
                }
            }
        }

        private void BTN_Pourcent_Click(object sender, RoutedEventArgs e)
        {
            if (!erreur && TB_Display.Text != "")
            {
                double nombre = Lire(TB_Display.Text);

                if (!erreur)
                {
                    TB_Display.Text = Pourcentage(nombre).ToString();
                    nouveau = true;
                }
            }
        }

        private void BTN_Racine_Click(object sender, RoutedEventArgs e)
        {
            if (!erreur && TB_Display.Text != "")
            {
                double nombre = Lire(TB_Display.Text);

                if (!erreur)
                {
                    if (nombre < 0)
                    {
                        AfficherErreur("Racine négative");
                    }
                    else
                    {
                        TB_Display.Text = Racine(nombre).ToString();
                        nouveau = true;
                    }
                }
            }
        }

        // ----- Mémoire -----

        // MC : vide la mémoire
        private void BTN_MC_Click(object sender, RoutedEventArgs e)
        {
            memoire = 0;
            TB_Memoire.Text = "";
        }

        // MR : affiche la mémoire
        private void BTN_MR_Click(object sender, RoutedEventArgs e)
        {
            if (erreur)
            {
                Effacer();
            }

            if (operation == ' ')
            {
                TB_Expression.Text = "";
            }

            TB_Display.Text = memoire.ToString();
            nouveau = true;
        }

        // M+ : ajoute le nombre affiché à la mémoire
        private void BTN_MPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!erreur && TB_Display.Text != "")
            {
                double nombre = Lire(TB_Display.Text);

                if (!erreur)
                {
                    memoire = memoire + nombre;
                    TB_Memoire.Text = "M";
                    nouveau = true;
                }
            }
        }

        // M- : enlève le nombre affiché de la mémoire
        private void BTN_MMoins_Click(object sender, RoutedEventArgs e)
        {
            if (!erreur && TB_Display.Text != "")
            {
                double nombre = Lire(TB_Display.Text);

                if (!erreur)
                {
                    memoire = memoire - nombre;
                    TB_Memoire.Text = "M";
                    nouveau = true;
                }
            }
        }
    }
}
