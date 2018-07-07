using System;
using System.Collections.Generic;
using System.Text;

namespace core
{
    public class Instruction
    {
        private readonly CoreInteger upper;
        private readonly CoreInteger lower;

        public Instruction(CoreInteger upper, CoreInteger lower)
        {
            this.upper = upper;
            this.lower = lower;
        }

        public uint Target {
            get {
                var address = BitConverter.ToUInt32(new byte[] { 0x0, upper.HighByte, 0x0, 0x0 }, 0);
                address += lower;
                return address;
            }
        }

        public bool InRange {
            get {
                if (Target >= Processor.MaxAddress) {
                    return false;
                }
                return true;
            }
        }

        public OpCode OpCode {
            get {
                return OpTable.Codes[upper.LowByte];
            }
        }
    }
}
