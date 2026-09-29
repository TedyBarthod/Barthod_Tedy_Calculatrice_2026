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
        // déclaration Variables 
        int NB1 = 0;
        int NB2 = 0;
        int result = 0;
        char operation = ' ';

        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== Chiffres =====
        private void BTN_1_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "1";
        }

        private void BTN_2_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "2";
        }

        private void BTN_3_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "3";
        }

        private void BTN_4_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "4";
        }

        private void BTN_5_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "5";
        }

        private void BTN_6_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "6";
        }

        private void BTN_7_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "7";
        }

        private void BTN_8_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "8";
        }

        private void BTN_9_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "9";
        }

        private void Btn_0_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = TB_Display.Text + "0";
        }

        // ===== Opérateurs =====
        // Quand on clique sur un opérateur : on mémorise NB1 et l'opérateur,
        // puis on vide l'écran pour laisser l'utilisateur taper NB2

        private void BTN_Plus_Click(object sender, RoutedEventArgs e)
        {
            NB1 = int.Parse(TB_Display.Text);
            operation = '+';
            TB_Display.Text = "";
        }

        private void BTN_Moin_Click(object sender, RoutedEventArgs e)
        {
            NB1 = int.Parse(TB_Display.Text);
            operation = '-';
            TB_Display.Text = "";
        }

        private void BTN_Multipliee_Click(object sender, RoutedEventArgs e)
        {
            NB1 = int.Parse(TB_Display.Text);
            operation = '*';
            TB_Display.Text = "";
        }

        private void Btn_divisée_Click(object sender, RoutedEventArgs e)
        {
            NB1 = int.Parse(TB_Display.Text);
            operation = '/';
            TB_Display.Text = "";
        }

        // ===== Égale =====
        private void Btn_egale_Click(object sender, RoutedEventArgs e)
        {
            NB2 = int.Parse(TB_Display.Text);

            if (operation == '+')
            {
                result = NB1 + NB2;
            }
            else if (operation == '-')
            {
                result = NB1 - NB2;
            }
            else if (operation == '*')
            {
                result = NB1 * NB2;
            }
            else if (operation == '/')
            {
                if (NB2 == 0)
                {
                    TB_Display.Text = "Erreur : /0";
                    return;
                }
                result = NB1 / NB2;
            }

            TB_Display.Text = result.ToString();
        }

        // ===== Effacer (C) =====
        private void Btn_Ctrl_Click(object sender, RoutedEventArgs e)
        {
            TB_Display.Text = "";
            NB1 = 0;
            NB2 = 0;
            result = 0;
            operation = ' ';
        }

        private void TB_Display_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}