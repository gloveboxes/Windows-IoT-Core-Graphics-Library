using Glovebox.Graphics;
using Glovebox.Graphics.Components;
using Glovebox.Graphics.Drivers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Pixel[] palette = new Pixel[] { TriColour.Red, TriColour.Green, TriColour.Yellow };


            Ht16K33BiColor driver = new Ht16K33BiColor(113, Ht16K33BiColor.Rotate.D90);

            LED8x8BiColorMatrixHT16K33 matrix = new LED8x8BiColorMatrixHT16K33(driver);

            matrix.SetBrightness(1);

            while (true) {
                matrix.ScrollStringInFromRight("hello world ", 100, palette);
            }
        }
    }
}
