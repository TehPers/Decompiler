using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary {
    public class Program {

        public static void Main(string[] args) {
            Console.WriteLine("Hello, world!");
            Console.WriteLine(Add(1, 2));
            Console.ReadKey();
        }

        public static int Add(int a, int b) {
            return a + b;
        }

        public static int Sub(int a, int b) {
            return a - b;
        }

        public static int Mul(int a, int b) {
            return a * b;
        }

        public static int Div(int a, int b) {
            return a / b;
        }
    }
}
