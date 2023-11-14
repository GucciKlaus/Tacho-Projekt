using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tacho_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Line? Needle { get; set; } = null;
        public double Radius { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void sld_speedometer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RotateNeedle(sld_speedometer.Value);
        }

        private void can_speedometer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Alles davor löschen
            can_speedometer.Children.Clear();
            Radius = can_speedometer.ActualWidth / 2 > can_speedometer.ActualHeight * 0.9 ?
                can_speedometer.ActualHeight*0.9 : can_speedometer.ActualWidth / 2;

            RedrawNeedle();
            RedrawScale();
            
            RotateNeedle(sld_speedometer.Value);
        }

        private void RotateNeedle(double angle)
        {
            if (Needle != null)
            {
                RotateTransform rotateNeedle = new RotateTransform(angle, Needle.X1, Needle.Y1);
                Needle.RenderTransform = rotateNeedle;
            }

        }

        private void RedrawNeedle()
        {
            Needle = new Line
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 2,
                X1 = can_speedometer.ActualWidth / 2,
                Y1 = can_speedometer.ActualHeight * 0.9,
                X2 = can_speedometer.ActualWidth / 2 - Radius * 0.75,
                Y2 = can_speedometer.ActualHeight * 0.9
            };
            can_speedometer.Children.Add(Needle);
        }

        private void RedrawScale()
        {
            for (int angle = 0; angle <= 180; angle += 10)
            {
                //Skalenstriche
                Line scaleLine = new Line
                {
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1,
                    X1 = can_speedometer.ActualWidth / 2 - Radius * 0.8,
                    Y1 = can_speedometer.ActualHeight * 0.9,
                    X2 = can_speedometer.ActualWidth / 2 - Radius * 0.85,
                    Y2 = can_speedometer.ActualHeight * 0.9
                };
                RotateTransform rotatescale = new RotateTransform(angle, Needle.X1, Needle.Y1);
                scaleLine.RenderTransform = rotatescale;
                can_speedometer.Children.Add(scaleLine);

                //Skalenbeschriftung
                TextBlock tbSpeedScale = new TextBlock
                { Text = angle.ToString(),
                    Foreground = new SolidColorBrush(Colors.White),
                   // Background = new SolidColorBrush(Colors.Red),
                    TextAlignment= TextAlignment.Center,
                    Width = 30,
                    Height = 20
                    
                };
                //Canvas.SetTop(tbSpeedScale, );
                //Canvas.SetLeft(tbSpeedScale,  );

                //Achtung die Transformationen in umgekehrter Reihenfolge im Code
                TransformGroup transformGroupTB = new TransformGroup();
                transformGroupTB.Children.Add(new TranslateTransform(-tbSpeedScale.Width/2, -tbSpeedScale.Height /2));
                transformGroupTB.Children.Add(new RotateTransform(180 - angle));
                transformGroupTB.Children.Add(new TranslateTransform(Radius * 0.9, 0));
                transformGroupTB.Children.Add(new RotateTransform(-180 + angle));
                transformGroupTB.Children.Add(new TranslateTransform(Needle.X1,Needle.Y1));
                tbSpeedScale.RenderTransform = transformGroupTB;
                can_speedometer.Children.Add(tbSpeedScale);
            }
        }
    }
}
