using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{

    /// <summary>
    /// Represents a listener for a data type T. This can help for when values need to be updated every now and then without checking every frame.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Listener<T>
    {

        /// <summary>
        /// The value encased inside the listener.
        /// </summary>
        internal T Value 
        {
            get
            {
                return localValue;
            }
            set
            {
                //only call any updates once the value changes.
                if (!Equals(value))
                {
                    localValue = value;
                    OnValueChanged?.Invoke();
                }
            }
        }
        
        // local value of lisener data
        private T localValue;

        /// <summary>
        /// Creates a new listener value which can trigger event sequences when a value changes.
        /// </summary>
        /// <param name="value"></param>
        internal Listener(T value)
        {
            localValue = value;
        }

        /// <summary>
        /// This gets executed whenever the value gets changed.
        /// </summary>
        internal event UpdateDelegate OnValueChanged;

        /// <summary>
        /// Gets the listener's value as a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Gets the listener's value as a hash code representation.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Whether the inner value of the listener object is equal to the input.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is T && Value.Equals(obj);
        }

    }
}
