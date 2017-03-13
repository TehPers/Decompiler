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

        public static void ForTest(int n) {
            do {
                for (int i = 0; i < n; i++)
                    Console.WriteLine(n);
            } while (--n > 0);
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
