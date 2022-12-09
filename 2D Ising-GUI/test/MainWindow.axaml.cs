using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace test
{
    public partial class MainWindow : Window
    {
        private System.Drawing.Bitmap irBitmap = new System.Drawing.Bitmap(256, 128, PixelFormat.Format24bppRgb);
        public MainWindow()
        {
            InitializeComponent();
        }
        double kb = 1.380649e-23; //Boltzmann-constant
        double Temp = 1; //Fixed Temp
        double kbt_overJ = 2.26;
        int J = 1;
        int B0 = 1;
         //Iterations

        /// <summary>
        /// Multithreaded 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnButtonClick2(object sender, RoutedEventArgs e)
        {
            //int iterations = Convert.ToInt32(tb_iterations.Text);
            //int flips = Convert.ToInt32(tb_flips.Text);
            //int X = Convert.ToInt32(tb_x.Text); //X of Lattice
            //int Y = Convert.ToInt32(tb_y.Text); //Y of Lattice
            //irBitmap = new System.Drawing.Bitmap(X, Y, PixelFormat.Format24bppRgb);
            ////Choose an initial state
            //Lattice lattice1 = new Lattice(X, Y);
            //await DrawLatticeToGui(lattice1); //Draw initial lattice
            //List<(double, int)> hamiltons = new List<(double, int)>(); //List of Hamiltonians with number of iterations
            //List<(double, int)> magnetizations = new List<(double, int)>(); //List of magnetization per iteration

            //int count = 0;
            //var oldHamiltonian = lattice1.Hamiltonian(); //Hamilton of original state
            //Parallel.For(0, iterations, i =>
            //{
            //        //Save old configuration
            //        Lattice lattice2 = lattice1.Copy(); //CARE!!! In csharp = is a copy for structs, but a reference for class objects
            //        //Choose a site i
            //        lattice2.flipRandomBit(flips);
            //        //Calculate the energy change diffE which results if the spin at site i is overturned
            //        var newHamiltonian = lattice2.Hamiltonian(); //Hamilton of flipped state

            //    if (newHamiltonian < oldHamiltonian)
            //    {
            //        lattice1 = lattice2.Copy(); //Continue with the new configuration
            //        oldHamiltonian = newHamiltonian;
            //        hamiltons.Add((newHamiltonian, count)); //Safe the new config for statistical reasons
            //        magnetizations.Add((lattice2.m, count));
            //        count++;
            //    }
            //    else
            //    {
            //        var diffE = oldHamiltonian - newHamiltonian; //Energy difference
            //        //Generate a random number r such that 0<r<1
            //        Random random = new Random();
            //        var r = random.NextDouble();
            //        //if r<exp(-diffE/kbT), flip the spin
            //        if (r < Math.Exp(diffE / (kbt_overJ)))
            //        {
            //            lattice1 = lattice2.Copy(); //Continue with the new configuration
            //            hamiltons.Add((newHamiltonian, count)); //Safe the new config for statistical reasons
            //            magnetizations.Add((lattice2.m, count));
            //            count++;
            //            lattice1 = lattice2.Copy(); //Continue with the new configuration
            //            oldHamiltonian = newHamiltonian;
            //        }
            //    }
                
            //    hamiltons.Add((oldHamiltonian, i)); //Safe the new config for statistical reasons

            //});
            

            //await DrawLatticeToGui(lattice1, iterations); //Draw new configuration
                
            //    l_accepted.Content = "Accepted energy:" + lattice1.Hamiltonian().ToString();
            //    l_found.Content = "Lowest Energy found / Iteration:" + $"{hamiltons.Min().Item1}/{hamiltons.Min().Item2}";
            //await WriteToFileAsync(hamiltons, magnetizations);

            
        }
        public void GenerateLattice(List<(double, int)> hamiltons, Lattice lattice1)
        {

        }
        /// <summary>
        /// Singlethreaded
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            //Get Data from GUI
            int iterations = Convert.ToInt32(tb_iterations.Text); //Iterations 
            int flips = Convert.ToInt32(tb_flips.Text); //flips
            int X = Convert.ToInt32(tb_x.Text); //X of Lattice
            int Y = Convert.ToInt32(tb_y.Text); //Y of Lattice
            kbt_overJ = Convert.ToDouble(tb_temp.Text.Trim()); //Temp
            //Initialise Bitmap for GUI
            irBitmap = new System.Drawing.Bitmap(X, Y, PixelFormat.Format24bppRgb);
            //Choose an initial state
            Lattice lattice1 = new Lattice(X, Y, J, B0);
            //Determine if all spins up, down or random
            switch (cb_spins.SelectedIndex)
            {
                case 0:
                    lattice1 = new Lattice(X, Y, true, J, B0);
                    break;
                case 1:
                    lattice1 = new Lattice(X, Y, false, J, B0);
                    break;
                case 2:
                    lattice1 = new Lattice(X, Y, J, B0);
                    break;
                default:
                     lattice1 = new Lattice(X, Y, true, J, B0);
                    break;
            }
            //Determine the function for the update of the magnet field
            UpdateMode updateMode;
            switch (cb_variation.SelectedIndex)
            {
                case 0:
                    updateMode = UpdateMode.Constant;
                    break;
                case 1:
                    updateMode = UpdateMode.Linear;
                    break;
                case 2:
                    updateMode = UpdateMode.Cos;
                    break;
                default:
                    updateMode = UpdateMode.Constant;
                    break;
            }
            await DrawLatticeToGui(lattice1); //Draw initial lattice
            //Initialise start values and storage containers for the states
            List<(double, int)> hamiltons = new List<(double, int)>(); //List of Hamiltonians with number of iterations
            List<(double, int)> magnetizations = new List<(double, int)>(); //List of magnetization per iteration
            List<int[,]> spins = new List<int[,]>(); //List of spin configurations per iteration
            List<double> work = new List<double>() { 0,0}; //List of works per iteration, 0 and 0 as starting values
            double W = 0; //Work
            var oldHamiltonian = lattice1.Hamiltonian(); //Hamilton of original state
            var oldMagnetization = lattice1.m;
            for (int i = 0; i < iterations; i++)
            {
                //Update B(t) 
                magnetizations.Add((oldMagnetization, i));
                lattice1.UpdateMagnet(i, updateMode, iterations);
                //Save old configuration
                Lattice lattice2 = lattice1.Copy(); //CARE!!! In csharp = is a copy for structs, but a reference for class objects
                //Choose a site i
                lattice2.flipRandomBit(flips);
                //Calculate the energy change diffE which results if the spin at site i is overturned
               
                var newHamiltonian = lattice2.Hamiltonian(); //Hamilton of flipped state

                if (newHamiltonian < oldHamiltonian)
                {
                    lattice1 = lattice2.Copy(); //Continue with the new configuration
                    oldHamiltonian = newHamiltonian;
                    oldMagnetization = lattice2.m;
                }
                else
                {
                    var diffE = oldHamiltonian - newHamiltonian; //Energy difference
                    //Generate a random number r such that 0<r<1
                    Random random = new Random();
                    var r = random.NextDouble();
                    //if r<exp(-diffE/kbT), flip the spin
                    if (r < Math.Exp(diffE/ kbt_overJ))
                    {
                        //Continue with the new configuration
                        lattice1 = lattice2.Copy(); 
                        oldHamiltonian = newHamiltonian;
                        oldMagnetization = lattice2.m;
                    }
                }

                if (hamiltons.Count>=2) //Not happy with the if
                {
                    W += (hamiltons[hamiltons.Count - 1].Item1 - hamiltons[hamiltons.Count - 2].Item1);
                    work.Add(W);
                }

                //Safe the config for statistical reasons
                hamiltons.Add((oldHamiltonian, i)); 
                spins.Add(lattice1.Spins);
                //Update GUI
                await DrawLatticeToGui(lattice1, i); //Draw new configuration
                l_accepted.Content = "Accepted energy:" + Math.Round(oldHamiltonian).ToString();
                l_found.Content = "Lowest Energy found / Iteration:" + $"{hamiltons.Min().Item1}/{hamiltons.Min().Item2}";
                l_Work.Content = $"Work:{W}";
            }
            //Save simulation in file
            await FileWriterClass.WriteToFileAsync(hamiltons, magnetizations, spins, work, temp: kbt_overJ);
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
                        System.Drawing.Color color = System.Drawing.Color.FromArgb(185, 103, 255); //Spin 1
                        if (l.Spins[i, j] == -1)
                        {
                            color = System.Drawing.Color.FromArgb(1, 205, 254); //Spin -1
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
