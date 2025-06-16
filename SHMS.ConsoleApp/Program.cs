using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHMS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] scores = [213, 414, 44, 1223, 41, 44, 14];

            IEnumerable<int> scoreQuery =
                from score in scores
                where score > 80
                select score;   
        }
    }
}
