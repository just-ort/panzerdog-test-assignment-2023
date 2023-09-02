using System;

namespace Panzerdog.Test.Assignment.Attributes
{
    public class AddressableScreenAttribute : Attribute
    {
        public string Address { get; }

        public AddressableScreenAttribute(string address)
        {
            Address = address;
        }
    }
}