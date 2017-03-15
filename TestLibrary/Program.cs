using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary {
    public class Program {

        public static void Main(string[] args) {
            Console.WriteLine("Hello, world!");
            Console.WriteLine(Add(10, 2));
            Console.ReadKey();
        }

        public static void ConstructTest(int n) {
            if (n > 10)
                Console.WriteLine(n);
            else
                Console.WriteLine(n + 1);
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

        public static int Max(int a, int b) {
            if (a > b) return a;
            return b;
        }
    }
}
