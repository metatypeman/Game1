﻿//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Util
{
    /// <summary>
    /// Just a single integer
    /// 
    /// @author Kristina Toutanova (kristina@cs.stanford.edu)
    /// Code ...
    /// </summary>
    public class IntUni : IntTuple
    {
        public IntUni() : base(1)
        {
        }

        public IntUni(int src) : this()
        {
            elements[0] = src;
        }

        public int GetSource()
        {
            return elements[0];
        }

        public void SetSource(int src)
        {
            elements[0] = src;
        }

        public override IntTuple GetCopy()
        {
            var nT = new IntUni(elements[0]);
            return nT;
        }

        public void Add(int val)
        {
            elements[0] += val;
        }
    }
}
