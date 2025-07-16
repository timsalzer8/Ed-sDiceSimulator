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

namespace DiceSimulator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Wird ausgelöst, wenn der Button geklickt wird
        private void rollButton_Click(object sender, RoutedEventArgs e)
        {
            // Zufallszahl-Generator
            Random random = new Random();

            // Aktuell ausgewählten Würfel auslesen (z. B. "D6", "D20" etc.)
            string selectedDie = ((ComboBoxItem)diceTypeComboBox.SelectedItem).Content.ToString();

            int result = 0;

            // Wurf je nach gewähltem Würfeltyp
            switch (selectedDie)
            {
                case "D4":
                    result = random.Next(1, 5);
                    break;
                case "D6":
                    result = random.Next(1, 7);
                    break;
                case "D8":
                    result = random.Next(1, 9);
                    break;
                case "D10":
                    result = random.Next(1, 11);
                    break;
                case "D12":
                    result = random.Next(1, 13);
                    break;
                case "D20":
                    result = random.Next(1, 21);
                    break;
                case "D100":
                    int tens = random.Next(0, 10) * 10;  // 0, 10, 20 ... 90
                    int ones = random.Next(0, 10);       // 0–9
                    result = (tens + ones == 0) ? 100 : tens + ones;
                    break;
            }

            // Ed's Kommentar – je nach Ergebnis etwas anders
            string edComment = "";

            if (result == 1)
            {
                edComment = $"Oof! Ed whispers: That's a sad {result} on a {selectedDie}… try again!";
            }
            else if (result == 100)
            {
                edComment = $"Ed gasps: A perfect {result}! The legend is real!";
            }
            else if (result == 20 && selectedDie == "D20")
            {
                edComment = $"Ed yells: A NATURAL 20! You magnificent creature!";
            }
            else
            {
                edComment = $"Ed says: You rolled a {selectedDie} and received a glorious {result}!";
            }

            // Zeigt Eds Kommentar in einem Popup an
            var edWindow = new EdWindow(edComment);
            edWindow.ShowDialog();

        }
    }
}
