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
using System.Windows.Shapes;

namespace projekt_kulki
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public TextBox[,] textBoxes = new TextBox[6, 6];
        public TextBox[] textBoxMasses = new TextBox[6];

        public SettingsWindow()
        {
            InitializeComponent();
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    textBoxes[i, j] = (TextBox)FindName($"textBox{j + 1}{i + 1}");
                }
            }
            for(int i=0; i<=5; i++)
            {
                textBoxMasses[i] = (TextBox)FindName($"textBoxMass{i + 1}");
            }
            DownloadParameters();
        }

        private void DownloadParameters()
        {
            
            for(int i=0; i<6; i++)
            {
                for(int j=0; j<6; j++)
                {
                    textBoxes[i, j].Text = UniverseProperties.getCoefficient((ParticleType) i,(ParticleType) j).ToString();
                }
                textBoxMasses[i].Text = UniverseProperties.getMass((ParticleType)i).ToString();
            }
            timeStepTextBox.Text = UniverseProperties.timeStep.ToString();
            frictionTextBox.Text = UniverseProperties.frictionCoefficient.ToString();
            interactionForceComboBox.SelectedIndex = (int)UniverseProperties.interactionForceID;
        }

        private void UploadParameters()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    UniverseProperties.setCoefficient((ParticleType)i, (ParticleType)j, int.Parse(textBoxes[i, j].Text));
                }
                UniverseProperties.setMass((ParticleType)i, int.Parse(textBoxMasses[i].Text));
            }
            UniverseProperties.timeStep = double.Parse(timeStepTextBox.Text);
            UniverseProperties.frictionCoefficient = double.Parse(frictionTextBox.Text);
            changeForce();
        }
        

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            UploadParameters();
            UniverseProperties.Reload(); 
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            UploadParameters();
            UniverseProperties.Reload();
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void changeForce()
        {
            switch (interactionForceComboBox.SelectedIndex)
            {
                case (int)InteractionForce.Electrostatic:
                    UniverseProperties.interactionForce = Force.Electrostatic;
                    UniverseProperties.interactionForceID = InteractionForce.Electrostatic;
                    break;
                case (int)InteractionForce.Linear1:
                    UniverseProperties.interactionForce = Force.Linear1;
                    UniverseProperties.interactionForceID = InteractionForce.Linear1;
                    break;
                case (int)InteractionForce.Linear2:
                    UniverseProperties.interactionForce = Force.Linear2;
                    UniverseProperties.interactionForceID = InteractionForce.Linear2;
                    break;
                default:
                    UniverseProperties.interactionForce = Force.Electrostatic;
                    UniverseProperties.interactionForceID = InteractionForce.Electrostatic;
                    break;
            }
        }
    }
}
