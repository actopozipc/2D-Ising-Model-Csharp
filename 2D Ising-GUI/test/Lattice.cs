 
using System;
using System.Diagnostics;

namespace test
{
    /// <summary>
    /// X times Y Lattice
    /// </summary>
    public class Lattice
    {
       
        /// <summary>
        /// X times Y matrix with -1 or 1
        /// </summary>
        public int[,] Spins { get; set; }
        /// <summary>
        /// Factor J used in the Hamiltonian
        /// </summary>
        private int J { get; set; }
        /// <summary>
        /// Magnetization factor
        /// </summary>
        public int u { get; set; }
        /// <summary>
        /// Magnetization
        /// </summary>
        public double m { get; set; }
        /// <summary>
        /// Constructor of the class
        /// Creates a random X times Y Array with -1 or 1
        /// </summary>
        /// <param name="X">Rows</param>
        /// <param name="Y">Columns</param>
        /// <param name="j">J Factor of Hamilton</param>
        public Lattice(int X, int Y, int j = 1, int u=1)
        {
            Spins = new int[X,Y];
            Random r = new Random();
            for (int i = 0; i < X; i++)
            {
                for (int k = 0; k < Y; k++)
                {
                    if (r.NextDouble() < 0.5)
                    {
                        Spins[i, k] = -1;
                    }
                    else
                    {
                        Spins[i, k] = 1;
                    }
                }
            }
            J = j;
            this.u = u;

        }

        public Lattice(int X, int Y, bool Up, int j = 1, int u = 1)
        {
            Spins = new int[X, Y];
            if (Up)
            {
                for (int i = 0; i < X; i++)
                {
                    for (int k = 0; k < Y; k++)
                    {
                       Spins[i, k] = 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < X; i++)
                {
                    for (int k = 0; k < Y; k++)
                    {
                        Spins[i, k] = -1;
                    }
                }
            }
            J = j;
            this.u = u;
        }
        
        /// <summary>
        /// Inverts a number of elements in the array
        /// </summary>
        /// <param name="flips">Number of elements that should be flipped</param>
        public void flipRandomBit(int flips=1)
        {
            Random r = new Random();
            for (int i = 0; i < flips; i++)
            {
                int rand1 = r.Next(0, this.Spins.GetLength(0));
                int rand2 = r.Next(0, this.Spins.GetLength(1));
                this.Spins[rand1, rand2] = -(this.Spins[rand1, rand2]);
            }
            
        }
        /// <summary>
        /// Hamilton = -J * Sum of neighbor spins - (magnetfactor * sum of spins)
        /// At the moment its only the sum of neighbor spins
        /// </summary>
        /// <returns></returns>
        public double Hamiltonian()
        {
            //Hamilton = -J * Sum of Energy around a spin - magnetfun
            double energy = 0;
            double magnetization = 0;
            for (int x = 0; x < Spins.GetLength(0); x++)
            {
                for (int y = 0; y < Spins.GetLength(1); y++)
                {
                    var spin = Spins[x, y];
                    //Default values if there are no spins left, right, up and down
                    var left = Spins[Spins.GetLength(0)-1,y]; //if the spin is the first in a row, take the spin from the other side
                    var right = Spins[0,y];  //if the spin is the last in a row, take the spin from the other side
                    var down = Spins[x, 0]; //if the spin is the last in a column, take the first 
                    var up = Spins[x, Spins.GetLength(1) - 1]; //if the spin is the first in a column, take the last
                    if (x > 0) //If the spin is not the first in the row
                    {
                        left = Spins[x - 1, y];
                        if (x < Spins.GetLength(0)  - 1) //if the spin is not the very last in the row
                        {
                            right = Spins[x + 1, y];
                        }
                    }
                    if (y > 0) //if the spin is not the first in the column
                    {
                        up = Spins[x, y - 1];
                        if (y < Spins.GetLength(1) - 1) //if the spin is not the very last in the column
                        {
                            down = Spins[x, y + 1];
                        }
                    }
                    magnetization = magnetization + Spins[x, y];
                    this.m = magnetization;
                    energy = energy + 
                        1 //spin
                    *(left
                    + right
                    + down
                    + up);

                }

            }
            return -J * energy - this.u*magnetization;
        }
        /// <summary>
        /// Print function for console / Later usage 
        /// </summary>
        public void print()
        {
            string a = "";
            for (int x = 0; x < Spins.GetLength(0); x++)
            {
                a = "";
                for (int y = 0; y < Spins.GetLength(1); y++)
                {
                    if (Spins[x, y] == -1)
                    {
                        a = a + "-";
                    }
                    else
                    {
                        a = a + "+";
                    }
                }
            }
        }
        /// <summary>
        /// Attention! In Csharp, = refers to a copy for structs, but to a reference for classes
        /// A obj1 = new A();
        /// A obj2 = new A();
        /// ob1.Method(); -> this method is also called for obj2!!!!
        /// In order to avoid this, I wrote a copy method that returns a second Lattice with the same
        /// properties as the Lattice that calls it
        /// </summary>
        /// <returns></returns>
        public Lattice Copy()
        {
            Lattice copy = new Lattice(this.Spins.GetLength(0), this.Spins.GetLength(1));
            for (int i = 0; i < this.Spins.GetLength(0); i++)
            {
                for (int j = 0; j < this.Spins.GetLength(1); j++)
                {
                    copy.Spins[i, j] = this.Spins[i, j];
                }
            }
            copy.J = this.J;
            copy.m = this.m;
            return copy;
        }
    }

    }
