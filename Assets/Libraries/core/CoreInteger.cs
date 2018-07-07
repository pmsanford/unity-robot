using System;
using System.Collections.Generic;
using System.Text;

namespace core
{
    public class CoreInteger
    {
        private byte highByte;
        private byte lowByte;
        
        public CoreInteger(byte lowByte, byte highByte)
        {
            this.highByte = highByte;
            this.lowByte = lowByte;
        }

        public CoreInteger(byte[] memory, uint idx) : this(memory[idx], memory[idx + 1])
        {
        }

        public CoreInteger(uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            lowByte = bytes[0];
            highByte = bytes[1];
        }
        
        public void Write(byte[] memory, uint address)
        {
            memory[address] = lowByte;
            memory[address+1] = highByte;
        }

        public static implicit operator int(CoreInteger ci)
        {
            return ci.Value;
        }

        public static implicit operator uint(CoreInteger ci)
        {
            return ci.Value;
        }

        public static implicit operator CoreInteger(uint i)
        {
            return new CoreInteger(i);
        }

        public static implicit operator CoreInteger(int i)
        {
            return new CoreInteger((uint)i);
        }

        public UInt16 Value {
            get {
                return BitConverter.ToUInt16(new[] { lowByte, highByte }, 0);
            }
        }

        public static bool operator<(CoreInteger a, CoreInteger b)
        {
            return a.Value < b.Value;
        }

        public static bool operator>(CoreInteger a, CoreInteger b)
        {
            return a.Value > b.Value;
        }

        public static bool operator==(CoreInteger a, CoreInteger b)
        {
            return a.Value == b.Value;
        }

        public static bool operator!=(CoreInteger a, CoreInteger b)
        {
            return a.Value != b.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is CoreInteger) {
                var ci = obj as CoreInteger;
                return Value.Equals(ci.Value);
            }
            return false;
        }

        public byte HighByte { get { return highByte; } }
        public byte LowByte { get { return lowByte; } }
    }
}
