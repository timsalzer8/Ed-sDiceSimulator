using System;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace DiceSimulator
{
    public partial class EdWindow : Window
    {
        public EdWindow(string comment)
        {
            InitializeComponent();

            // Lade die Ed.gif als animiertes Bild
            var imageUri = new Uri("pack://application:,,,/Ed.gif");
            var image = new BitmapImage(imageUri);
            ImageBehavior.SetAnimatedSource(edGif, image);

            // Kommentar anzeigen
            edCommentText.Text = comment;
        }
    }
}
