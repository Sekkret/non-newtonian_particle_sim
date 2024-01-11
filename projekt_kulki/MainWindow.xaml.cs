using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace projekt_kulki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ParticleManager particleManager;
        ChosenTool chosenTool = ChosenTool.None;
        ParticleType chosenParticle = ParticleType.Red;
        enum ChosenTool
        {
            None,
            AddParticle, 
            RemoveParticle,
            MoveParticle
        }

        // these variables are necessary for drag option
        private UIElement? dragParticle = null;
        private Point offset;

        public MainWindow()
        {
            InitializeComponent();
            particleManager = new ParticleManager(playgroundCanvas);

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick; //event!!
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Start();

            UniverseProperties.Load();

        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            particleManager.MoveParticles();

        }

        // next two functions manages the choice of function
        private void toolButtonChecked(object sender, RoutedEventArgs e)
        {
            string tag = (string)((ToggleButton)sender).Tag;
            if (tag == "addParticle")
            {
                this.removeParticleButton.IsChecked = false;
                this.moveParticleButton.IsChecked = false;
                this.chosenTool = ChosenTool.AddParticle;
            }
            if (tag == "removeParticle")
            {
                this.addParticleButton.IsChecked = false;
                this.moveParticleButton.IsChecked = false;
                this.chosenTool = ChosenTool.RemoveParticle;
            }
            if (tag == "moveParticle")
            {
                this.addParticleButton.IsChecked = false;
                this.removeParticleButton.IsChecked = false;
                this.chosenTool = ChosenTool.MoveParticle;
            }
        }
        private void toolButtonUnchecked(object sender, RoutedEventArgs e)
        {
            if (addParticleButton.IsChecked == false && removeParticleButton.IsChecked == false && moveParticleButton.IsChecked == false)
            {
                this.chosenTool = ChosenTool.None;
            }
        }

        // creating of new objects
        private void playgroundCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.chosenTool == ChosenTool.AddParticle)
            {
                if (sender is Canvas playgroundCanvas)
                {
                    Point position = e.GetPosition(playgroundCanvas);
                    position.X -= Particle.radius;
                    position.Y -= Particle.radius;
                    Particle particle = new Particle(position , Vector2.Zero, chosenParticle);
                    playgroundCanvas.Children.Add(particle);
                    particle.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.particle_MouseLeftButtonDown);
                    particle.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.particle_PreviewMouseLeftButtonDown);
                    particleManager.AddParticle(particle);
                }
            }

        }

        //removal of particles
        private void particle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.chosenTool == ChosenTool.RemoveParticle)
            {
                if (sender is Particle clickedParticle)
                {
                    if (playgroundCanvas.Children.Contains(clickedParticle))
                    {
                        playgroundCanvas.Children.Remove(clickedParticle);

                        // Assuming you have a reference to the ParticleManager class
                        particleManager.RemoveParticle(clickedParticle);
                    }
                }
            }
        }

        //drag of particles - next 3 functions.
        private void particle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chosenTool == ChosenTool.MoveParticle)
            {
                this.dragParticle = sender as UIElement;
                this.offset = e.GetPosition(this.playgroundCanvas);
                this.offset.Y -= Canvas.GetTop(this.dragParticle);
                this.offset.X -= Canvas.GetLeft(this.dragParticle);
                this.playgroundCanvas.CaptureMouse();
            }
        }
        private void playgroundCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (chosenTool == ChosenTool.MoveParticle)
            {
                if (this.dragParticle == null)
                    return;
                var position = e.GetPosition(sender as IInputElement);
                Canvas.SetTop(this.dragParticle, position.Y - this.offset.Y);
                Canvas.SetLeft(this.dragParticle, position.X - this.offset.X);
            }
        }

        private void playgroundCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (chosenTool == ChosenTool.MoveParticle)
            {
                this.dragParticle = null;
                this.playgroundCanvas.ReleaseMouseCapture();
            }
        }

        private void chooseParticleButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button) sender;
            string chosenColor = (string)senderButton.Tag;

            Ellipse mainEllipse = (Ellipse)chooseParticleButton.FindName("mainParticlePicture");
            RadialGradientBrush radialBrush = (RadialGradientBrush) mainEllipse.Fill;
            GradientStop redGradientStop = radialBrush.GradientStops[2];

            switch (chosenColor)
            {
                case "red":
                    redGradientStop.Color = Colors.Red;
                    chosenParticle = ParticleType.Red;
                    break;
                case "blue":
                    redGradientStop.Color = Colors.Blue;
                    chosenParticle = ParticleType.Blue;
                    break;
                case "green":
                    redGradientStop.Color = Colors.Green;
                    chosenParticle = ParticleType.Green;
                    break;
                case "yellow":
                    redGradientStop.Color = Colors.Yellow;
                    chosenParticle = ParticleType.Yellow;
                    break;
                case "purple":
                    redGradientStop.Color = Colors.Purple;
                    chosenParticle = ParticleType.Purple;
                    break;
                case "gray":
                    redGradientStop.Color = Colors.Gray;
                    chosenParticle = ParticleType.Gray;
                    break;
                default:
                    redGradientStop.Color = Colors.Red;
                    chosenParticle = ParticleType.Red;
                    break;
            }
            avaiableParticlesPopup.IsOpen = false;
        }
    }
}
