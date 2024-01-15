using projekt_kulki;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace YourNamespace
{
    public class ParticleEllipse : Control
    {
        static ParticleEllipse()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ParticleEllipse), new FrameworkPropertyMetadata(typeof(ParticleEllipse)));

        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(ParticleEllipse), new PropertyMetadata(20.0));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ParticleEllipse), new PropertyMetadata(Colors.Red));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
    }
}
