﻿using System;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class Class1
    {
        public void Run()
        {
            Task.Run(() => {

            });
        }

        public void Tst()
        {
            LogInstance.Log("Tst");
        }
    }
}