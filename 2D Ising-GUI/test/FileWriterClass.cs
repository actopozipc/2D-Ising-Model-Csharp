using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class FileWriterClass
    {
        /// <summary>
        /// Writes hamiltons and magnetization into a file
        /// iteration, hamilton, magnetization
        /// in this order so numpy can easily convert it
        /// </summary>
        public static async Task WriteToFileAsync(List<(double, int)> hamiltons, List<(double, int)> magnetizations, List<int[,]> spins, List<double> work, double temp=0)
        {
            int n = hamiltons.Count;
            //Order them by time
            hamiltons.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            magnetizations.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            hamiltons.Reverse();
            magnetizations.Reverse();
            //Convert array of spins to a string
            string[] spinString = new string[spins.Count];
            for (int i = 0; i < n; i++)
            {

                spinString[i] = ConvertSpinChainToString(spins[i], ",");
            }
            string[] lines = new string[n + 1]; //string list for file
            lines[0] = "Iterations Energy Magnetization Work Spins";
            for (int i = 0; i < n; i++)
            {
                lines[i + 1] = $"{hamiltons[i].Item2}; {hamiltons[i].Item1};{magnetizations[i].Item1}; " +
                    $"{work[i]}; {spinString[i]}";
            }
            await File.WriteAllLinesAsync($"../../../../Plotting/" +
                $"{n + "-" + temp + "-" + spins[0].Length + "-" +  DateTime.Now.ToString().Replace(':', '-')}.csv", lines);
        }
        public static string ConvertSpinChainToString(Array arr, string separator)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (object o in arr)
            {
                if (o is Array)
                {
                    sb.Append(ConvertSpinChainToString((Array)o, separator));
                }
                else
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(separator);
                    }
                    sb.Append(o.ToString());
                }
            }
            return sb.ToString();
        }

    }
}

