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
using System.Windows.Media.Effects;
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

            // Flacker-Animation
            ColorAnimationUsingKeyFrames colorAnim = new ColorAnimationUsingKeyFrames();
            colorAnim.AutoReverse = true;
            colorAnim.RepeatBehavior = RepeatBehavior.Forever;
            colorAnim.Duration = TimeSpan.FromSeconds(2);

            // Flackernde Farbtöne (wie Feuer)
            colorAnim.KeyFrames.Add(new LinearColorKeyFrame((Color)ColorConverter.ConvertFromString("#FFA500"), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));    // Orange
            colorAnim.KeyFrames.Add(new LinearColorKeyFrame((Color)ColorConverter.ConvertFromString("#FFD700"), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100))));  // Goldgelb
            colorAnim.KeyFrames.Add(new LinearColorKeyFrame((Color)ColorConverter.ConvertFromString("#FF4500"), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200))));  // Feuerrot
            colorAnim.KeyFrames.Add(new LinearColorKeyFrame((Color)ColorConverter.ConvertFromString("#FFFFFF"), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300))));  // Hitzeblitz
            colorAnim.KeyFrames.Add(new LinearColorKeyFrame((Color)ColorConverter.ConvertFromString("#FF8C00"), KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400))));  // Dunkelorange


            // Brush aus Resource holen
            var brush = (SolidColorBrush)this.FindResource("rollTextBrush");
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);

            // Optional: Schatteneffekt flackern lassen
            DoubleAnimation blurAnim = new DoubleAnimation()
            {
                From = 5,
                To = 18,
                Duration = TimeSpan.FromMilliseconds(300),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            glowEffect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, blurAnim);
        }

        // Wird ausgelöst, wenn der Button geklickt wird
        private void rollButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            // UI ausblenden
            uiPanel.Visibility = Visibility.Collapsed;

            // Zeige Platzhalter-Würfel
            diceImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/dice_placeholder.png"));
            Canvas.SetLeft(diceImage, 100);
            Canvas.SetTop(diceImage, 100);
            diceImage.Visibility = Visibility.Visible;

            // Zielkoordinaten
            double newLeft = random.Next(100, 600);
            double newTop = random.Next(50, 300);

            var moveX = new DoubleAnimation()
            {
                From = Canvas.GetLeft(diceImage),
                To = newLeft,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
            };

            var moveY = new DoubleAnimation()
            {
                From = Canvas.GetTop(diceImage),
                To = newTop,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(moveX);
            storyboard.Children.Add(moveY);

            Storyboard.SetTarget(moveX, diceImage);
            Storyboard.SetTargetProperty(moveX, new PropertyPath("(Canvas.Left)"));

            Storyboard.SetTarget(moveY, diceImage);
            Storyboard.SetTargetProperty(moveY, new PropertyPath("(Canvas.Top)"));

            // Animation fertig → Ergebnis bestimmen & anzeigen
            storyboard.Completed += (s, ev) =>
            {
                string selectedDie = ((ComboBoxItem)diceTypeComboBox.SelectedItem).Content.ToString();
                int max;
                switch (selectedDie)
                {
                    case "D4": max = 4; break;
                    case "D6": max = 6; break;
                    case "D8": max = 8; break;
                    case "D10": max = 10; break;
                    case "D12": max = 12; break;
                    case "D20": max = 20; break;
                    case "D100": max = 100; break;
                    default: max = 6; break;
                }

                int result = selectedDie == "D100"
                    ? (random.Next(0, 10) * 10 + random.Next(0, 10)) is var r && r == 0 ? 100 : r
                    : random.Next(1, max + 1);

                // Zeig passendes Würfelbild (für D4–D20; bei D100 ggf. Standardbild)
                string imagePath = "pack://application:,,,/Assets/dice_placeholder.png";
                diceImage.Source = new BitmapImage(new Uri(imagePath));


                // Ed sagt was dazu
                string edComment;
                if (result == 1)
                {
                    edComment = $"Oof! Ed whispers: A sad {result} on a {selectedDie}…";
                }
                else if (result == 20 && selectedDie == "D20")
                {
                    edComment = $"Ed yells: A NATURAL 20!";
                }
                else if (result == 100)
                {
                    edComment = $"Ed gasps: A perfect {result}! The legend is real!";
                }
                else
                {
                    edComment = $"Ed says: You rolled a {selectedDie} and got {result}!";
                }


                var edWindow = new EdWindow(edComment);
                edWindow.ShowDialog();

                // UI wieder einblenden
                uiPanel.Visibility = Visibility.Visible;
            };

            storyboard.Begin();
        }

    }
}
