using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALC
{
    class main
    {
        static void Main(string[] args)
        {
            KEISANNKI calc = new KEISANNKI();
            Console.WriteLine("計算式を入力してください\n");
            string formula = Console.ReadLine();
            calc.Calc(formula);
        }
    }
}
