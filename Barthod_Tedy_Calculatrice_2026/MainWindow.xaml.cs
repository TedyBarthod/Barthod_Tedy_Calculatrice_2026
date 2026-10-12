// ====================================================================
//  Projet   : Calculatrice WPF - TP BTS CIEL 1
//  Auteur   : Tedy Barthod
//  Date     : octobre 2026
//  Fichier  : MainWindow.xaml.cs
//  Rôle     : logique de la calculatrice (le XAML s'occupe de l'apparence)
//
//  Principe :
//   1. L'utilisateur tape un nombre : il s'écrit sur l'écran (TB_Display).
//   2. Il clique sur un opérateur : le nombre est rangé dans NB1,
//      l'opérateur dans "operation", et l'écran se vide.
//      La petite ligne du haut (TB_Expression) rappelle le calcul : "12 +".
//   3. Il tape le deuxième nombre puis "=" : le nombre est rangé dans NB2,
//      on compare "operation" avec des if / else if et on appelle
//      la bonne fonction (Addition, Soustraction...).
//
//  Fonctionnalités :
//   - Version 1 : chiffres, virgule, + - * /, =, C, messages d'erreur
//     (division par 0, nombre trop grand, entrée invalide avec try / catch).
//   - Version 2 : +/-, %, racine carrée, mémoire (MC MR M+ M-),
//     calculs à la suite (2 + 3 * 4 = 20, de gauche à droite).
// ====================================================================

using System;
using System.Globalization;
using System.Windows;

namespace Barthod_Tedy_Calculatrice_2026
{
    public partial class MainWindow : Window
    {
        // ================================================================
        //  Variables
        // ================================================================
        double NB1 = 0;          // premier nombre
        double NB2 = 0;          // deuxième nombre
        double result = 0;       // résultat du calcul
        double memoire = 0;      // nombre gardé par les boutons mémoire
        char operation = ' ';    // opérateur choisi : '+', '-', '*' ou '/' (' ' = aucun)
        bool erreur = false;     // true : l'écran affiche un message d'erreur
        bool nouveau = false;    // true : le prochain chiffre remplace le nombre affiché

        // Culture française : la virgule sépare les décimales (2,5)
        CultureInfo culture = new CultureInfo("fr-FR");

        public MainWindow()
        {
            InitializeComponent();
        }

        // ================================================================
        //  Fonctions de calcul : une fonction par opération
        // ================================================================
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

        // ================================================================
        //  Conversions entre le texte de l'écran et les nombres
        // ================================================================

        // Texte -> nombre (ex : "2,5" donne 2.5)
        // try / catch : si le texte n'est pas un nombre, on affiche une erreur
        // au lieu de laisser le programme planter
        double Lire(string texte)
        {
            double nombre = 0;

            try
            {
                nombre = double.Parse(texte, culture);
            }
            catch (Exception)
            {
                AfficherErreur("Entrée invalide");
            }

            return nombre;
        }

        // Nombre -> texte avec une virgule (ex : 2.5 donne "2,5")
        // "G12" : 12 chiffres au maximum, pour que 0,1 + 0,2 affiche 0,3
        string Formater(double nombre)
        {
            if (nombre == 0)
            {
                nombre = 0;   // évite que l'écran affiche "-0"
            }

            return nombre.ToString("G12", culture);
        }

        // Cherche une virgule dans le texte, caractère par caractère
        bool ContientVirgule(string texte)
        {
            bool trouve = false;

            for (int i = 0; i < texte.Length; i++)
            {
                if (texte[i] == ',')
                {
                    trouve = true;
                }
            }

            return trouve;
        }

        // ================================================================
        //  Fonctions utilisées par plusieurs boutons
        // ================================================================

        // Remet la calculatrice à zéro (la mémoire est conservée)
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
        }

        // Affiche un message d'erreur à la place du nombre
        void AfficherErreur(string message)
        {
            TB_Display.Text = message;
            TB_Expression.Text = "";
            operation = ' ';
            erreur = true;
        }

        // Prépare l'écran avant d'écrire un chiffre ou une virgule
        void PreparerSaisie()
        {
            if (erreur)
            {
                Effacer();
            }

            if (nouveau)
            {
                // le nombre affiché est terminé : on en commence un autre
                TB_Display.Text = "";
                nouveau = false;

                if (operation == ' ')
                {
                    TB_Expression.Text = "";   // le calcul précédent est fini
                }
            }
        }

        void AjouterChiffre(string chiffre)
        {
            PreparerSaisie();

            if (TB_Display.Text == "0")
            {
                // on remplace le 0 pour ne pas afficher "05"
                TB_Display.Text = chiffre;
            }
            else if (TB_Display.Text.Length < 12)
            {
                // on ajoute le chiffre à la suite (12 caractères au maximum)
                TB_Display.Text = TB_Display.Text + chiffre;
            }
        }

        // Calcule NB1 (operation) NB2 et range le résultat dans "result"
        // En cas de problème, affiche une erreur à la place
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

            // Le résultat est trop grand pour une variable double
            if (!erreur && double.IsInfinity(result))
            {
                AfficherErreur("Nombre trop grand");
            }
        }

        void ChoisirOperateur(char op)
        {
            if (!erreur)
            {
                if (TB_Display.Text != "" && operation != ' ')
                {
                    // un calcul attend déjà son résultat (ex : 2 + 3 puis *) :
                    // on le termine, et son résultat devient le premier nombre
                    Calculer();
                    NB1 = result;
                }
                else if (TB_Display.Text != "")
                {
                    // premier nombre du calcul
                    NB1 = Lire(TB_Display.Text);
                }

                // On retient l'opérateur seulement s'il y a un premier nombre.
                // Écran vide + opérateur déjà choisi : on change juste d'opérateur.
                if (!erreur && (TB_Display.Text != "" || operation != ' '))
                {
                    operation = op;
                    TB_Expression.Text = Formater(NB1) + " " + op;
                    TB_Display.Text = "";
                    nouveau = false;
                }
            }
        }

        // ================================================================
        //  Chiffres
        // ================================================================
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

        // ================================================================
        //  Virgule
        // ================================================================
        private void BTN_Virgule_Click(object sender, RoutedEventArgs e)
        {
            PreparerSaisie();

            if (TB_Display.Text == "")
            {
                // pas encore de chiffre : on écrit "0,"
                TB_Display.Text = "0,";
            }
            else if (!ContientVirgule(TB_Display.Text))
            {
                // une seule virgule par nombre : la deuxième est ignorée
                TB_Display.Text = TB_Display.Text + ",";
            }
        }

        // ================================================================
        //  Opérateurs
        // ================================================================
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

        // ================================================================
        //  Égal et C
        // ================================================================
        private void BTN_Egal_Click(object sender, RoutedEventArgs e)
        {
            // il faut un opérateur ET un deuxième nombre
            if (!erreur && operation != ' ' && TB_Display.Text != "")
            {
                Calculer();

                if (!erreur)
                {
                    TB_Expression.Text = Formater(NB1) + " " + operation + " " + Formater(NB2) + " =";
                    TB_Display.Text = Formater(result);
                    operation = ' ';   // le calcul est terminé
                    nouveau = true;    // le prochain chiffre commence un nouveau calcul
                }
            }
        }

        private void BTN_C_Click(object sender, RoutedEventArgs e)
        {
            Effacer();
        }

        // ================================================================
        //  +/-   %   racine carrée
        //  Ces boutons transforment le nombre affiché. Le nombre est alors
        //  terminé : le prochain chiffre en commence un autre.
        // ================================================================
        private void BTN_Signe_Click(object sender, RoutedEventArgs e)
        {
            if (!erreur && TB_Display.Text != "")
            {
                double nombre = Lire(TB_Display.Text);

                if (!erreur && nombre != 0)
                {
                    TB_Display.Text = Formater(Signe(nombre));
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
                    TB_Display.Text = Formater(Pourcentage(nombre));
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
                        TB_Display.Text = Formater(Racine(nombre));
                        nouveau = true;
                    }
                }
            }
        }

        // ================================================================
        //  Mémoire
        //  MC : vide la mémoire          MR : affiche la mémoire
        //  M+ : ajoute le nombre affiché  M- : retire le nombre affiché
        //  Le petit "M" (TB_Memoire) indique que la mémoire est utilisée.
        // ================================================================
        private void BTN_MC_Click(object sender, RoutedEventArgs e)
        {
            memoire = 0;
            TB_Memoire.Text = "";
        }

        private void BTN_MR_Click(object sender, RoutedEventArgs e)
        {
            if (erreur)
            {
                Effacer();
            }

            if (operation == ' ')
            {
                TB_Expression.Text = "";   // le calcul précédent est fini
            }

            TB_Display.Text = Formater(memoire);
            nouveau = true;
        }

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
