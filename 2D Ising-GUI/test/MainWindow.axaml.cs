using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
namespace test
{
    public partial class MainWindow : Window
    {
        private System.Drawing.Bitmap irBitmap = new System.Drawing.Bitmap(256, 128, PixelFormat.Format24bppRgb);
        public MainWindow()
        {
            InitializeComponent();

        }
        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var kb = 1.380649e-23; //Boltzmann-constant
            var Temp = 1e-11; //Fixed Temp
            var iterations = 1000; //Iterations
            int X = Convert.ToInt32(tb_x.Text); //X of Lattice
            int Y = Convert.ToInt32(tb_y.Text); //Y of Lattice
            irBitmap = new System.Drawing.Bitmap(X, Y, PixelFormat.Format24bppRgb);
            //Choose an initial state
            Lattice lattice1 = new Lattice(X, Y);
            await DrawLatticeToGui(lattice1); //Draw initial lattice
            
            List<(double, int)> hamiltons = new List<(double, int)>(); //List of Hamiltonians with number of iterations
            for (int i = 0; i < iterations; i++)
            {
                //Save old configuration
                Lattice lattice2 = lattice1.Copy(); //CARE!!! In csharp = is a copy for structs, but a reference for class objects
                //Choose a site i
                lattice2.flipRandomBit(100);
                //Calculate the energy change diffE which results if the spin at site i is overturned
                var oldHamiltonian = lattice1.Hamiltonian(); //Hamilton of original state
                var newHamiltonian = lattice2.Hamiltonian(); //Hamilton of flipped state
                hamiltons.Add((newHamiltonian, i)); //Safe the new config for statistical reasons
                var diffE = newHamiltonian - oldHamiltonian; //Energy difference
                //Generate a random number r such that 0<r<1
                Random random = new Random();
                var r = random.NextDouble();
                //if r<exp(-diffE/kbT), flip the spin
                if (r < Math.Exp(-diffE / (kb * Temp)))
                {
                    lattice1 = lattice2.Copy(); //Continue with the new configuration
                }   

                await DrawLatticeToGui(lattice1, i); //Draw new configuration

                l_accepted.Content = "Accepted energy:" + lattice1.Hamiltonian().ToString();
                l_found.Content = "Lowest Energy found / Iteration:" + $"{hamiltons.Min().Item1}/{hamiltons.Min().Item2}";

            }
        }

            /// <summary>
            /// Draws configuration to the GUI
            /// GUI is done with Avalonia and Avalnoia doesnt directly support Bitmaps
            /// Solution: Create a bitmap and write it with a memorystream to the Avalonia source
            /// </summary>
            /// <param name="l"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            async Task DrawLatticeToGui(Lattice l, int count=0)
            {
                for (int i = 0; i < l.Spins.GetLength(0); i++)
                {
                    for (int j = 0; j < l.Spins.GetLength(1); j++)
                    {
                        Graphics g = Graphics.FromImage(irBitmap);
                        System.Drawing.Color color = System.Drawing.Color.FromName("Blue"); //Spin 1
                        if (l.Spins[i, j] == -1)
                        {
                            color = System.Drawing.Color.FromName("Brown"); //Spin -1
                        }
                        g.DrawRectangle(new System.Drawing.Pen(color), new System.Drawing.Rectangle(i, j, 1, 1));
                    }
                }
                //Convert from bitmap to Avalonia bitmap
                using (MemoryStream memory = new MemoryStream())
                {
                    irBitmap.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    Avalonia.Media.Imaging.Bitmap AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
                    image.Source = AvIrBitmap;


                }
                label.Content = "Iteration:" + count;
                await Task.Delay(1); //Give UI time to update
            }

        }




    }
