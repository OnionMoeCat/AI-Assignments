using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class Boolean
    {
        private bool value = true;

        public Boolean(bool value)
        {
            this.value = value;
        }

        public static implicit operator Boolean(bool value)
        {
            return new Boolean(value);
        }

        public static implicit operator bool (Boolean boolean)
        {
            return boolean.value;
        }

        public static bool operator ==(Boolean a, bool b)
        {
            if (a == null)
            {
                return false;
            }

            return a.value == b;
        }

        public static bool operator !=(Boolean a, bool b)
        {
            return !(a == b);
        }

        public static bool operator !(Boolean a)
        {
            return !a.value;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Boolean p = obj as Boolean;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.value == p.value;
        }

        public bool Equals(Boolean p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.value == p.value;
        }

        public override int GetHashCode()
        {
            return (value) ? 1 : 0;
        }
    }
}
