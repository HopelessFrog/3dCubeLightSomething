using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WFTools3D;

namespace _3dLight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cube cube;
        Sphere light;
        PointLight pointLight;
        AxisAngleRotation3D axisX;
        AxisAngleRotation3D axisY;
        AxisAngleRotation3D axisZ;
        public float Scale = 1;
        public float Brightness = 1;

        public MainWindow()
        {
            InitializeComponent();

            scene.Models.Add(new AxisModel(10));
            scene.Background = Brushes.LightBlue;

            cube = new Cube(52);

            cube.SpecularMaterial.Brush = Brushes.Silver;
            cube.SpecularMaterial.SpecularPower = 100;
            cube.DiffuseMaterial.Brush = Brushes.Blue;
            scene.Models.Add(cube);
            scene.Camera.Position = new Point3D(25, -15, 8);
            scene.Camera.LookAtOrigin();

            var pointEarth = new Point3D(3, 0, 0);
            light = new Sphere(24) { Radius = 0.2, Position = pointEarth };
            light.DiffuseMaterial.Brush = Brushes.White;
            scene.Models.Add(light);

            var group = scene.Lighting.LightingGroup;
            pointLight = new PointLight(Colors.White, pointEarth);
            pointLight.Range = 60;

            var ambLight = scene.Lighting.AmbientLight;
            ambLight.Color = Color.FromArgb(56, 132, 135, 85);
            //group.Children.Remove(scene.Lighting.AmbientLight);
            
            group.Children.Add(pointLight);
            DirectionalLight direct = scene.Lighting.DirectionalLight1;
            direct.Direction = new Vector3D(0, 0, 0);
            DirectionalLight direct2 = scene.Lighting.DirectionalLight2;
            direct2.Direction = new Vector3D(0, 0, 0);
            //scene.TimerTicked += TimerTicked;
            //scene.StartTimer(ms: 10);

            Square square = new Square(250) { Position = new Point3D(0, 0, -10)};
            square.ScaleX = 100;
            square.ScaleY = 100;
            square.Transform.Freeze();
            square.DiffuseMaterial.Brush = Brushes.Green;
            square.SpecularMaterial.Brush = Brushes.Green;
            square.SpecularMaterial.SpecularPower = 100;
            square.BackMaterial = square.Material;
            scene.Models.Add(square);

            RotateTransform3D myRotX = new();
            axisX = new();
            axisX.Angle = 0;
            axisX.Axis = new Vector3D(1, 0, 0);
            myRotX.Rotation = axisX;

            RotateTransform3D myRotY = new();
            axisY = new();
            axisY.Angle = 0;
            axisY.Axis = new Vector3D(0, 1, 0);
            myRotY.Rotation = axisY;


            RotateTransform3D myRotZ = new();
            axisZ = new();
            axisZ.Angle = 0;
            axisZ.Axis = new Vector3D(0, 0, 1);
            myRotZ.Rotation = axisZ;


            Transform3DGroup transGroup = new();
            transGroup.Children.Add(myRotX);
            transGroup.Children.Add(myRotY);
            transGroup.Children.Add(myRotZ);
            cube.Transform = transGroup;
        }


        void TimerTicked(object? sender, EventArgs e)
        {
            angle += 2;

            
            //light.Rotation3 = Math3D.Rotation(new Vector3D(0, 0, 1), angle * 0.1);
            //pointLight.Transform = light.Transform;
        }
        double angle;

        private void transZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (light is null) return;
            var pos = light.Position;
            pos = new Point3D(pos.X, pos.Y, pos.Z + (e.NewValue - pos.Z));
            light.Position = pos;
            pointLight.Position = pos;
        }

        private void transY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (light is null) return;
            var pos = light.Position;
            pos = new Point3D(pos.X, pos.Y + (e.NewValue - pos.Y), pos.Z);
            light.Position = pos;
            pointLight.Position = pos;
        }

        private void transX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (light is null) return;
            var pos = light.Position;
            pos = new Point3D(pos.X + (e.NewValue - pos.X), pos.Y, pos.Z);
            light.Position = pos;
            pointLight.Position = pos;
        }

        private void angX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cube is null) return;
            axisX.Angle = e.NewValue;
        }

        private void angY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cube is null) return;
            axisY.Angle = e.NewValue;
        }

        private void angZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cube is null) return;
            axisZ.Angle = e.NewValue;
        }

        private void Scale_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cube is null) return;
            cube.Scale = e.NewValue;
        }


        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            pointLight.ConstantAttenuation = 0;
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            pointLight.ConstantAttenuation = 1;

        }
    }
}