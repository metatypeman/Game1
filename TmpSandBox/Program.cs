using MyNPCLib;
using System;

namespace TmpSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            NLog.LogManager.GetCurrentClassLogger().Info("Hello World!");

            var tmpClass1 = new Class1();
            tmpClass1.Tst();
        }
    }
}
