//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Util
{
    /// <summary>
    /// A wrapper for immutable objects to update their value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MutableWrapper<T>
    {
        private T _value;

        public MutableWrapper(T val)
        {
            this._value = val;
        }

        public void SetValue(T t)
        {
            this._value = t;
        }

        public T Value()
        {
            return _value;
        }
    }
}
