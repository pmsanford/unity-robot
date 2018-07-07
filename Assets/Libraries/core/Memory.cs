using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace core
{
    public class Memory
    {
        private byte[] memory = new byte[Processor.MaxAddress];

        public Dictionary<uint, byte> Outputs = new Dictionary<uint, byte> {
            { 0x20, 0 },
            {0x21, 0 },
            {0x22, 0 },
            {0x23, 0 },
            {0x24, 0 },
            {0x25, 0 },
            {0x26, 0 },
            {0x27, 0 },
            {0x28, 0 },
            {0x29, 0 },
            {0x2A, 0 },
            {0x2B, 0 },
            {0x2C, 0 },
            {0x2D, 0 },
            {0x2E, 0 },
            {0x2F, 0 },
        };

        public void Load(byte[] program, uint startIndex)
        {
            Array.Copy(program, 0, memory, startIndex, program.Length);
        }

        public CoreInteger GetCoreInteger(uint address)
        {
            if (address < 0x10) {
                return address;
            }
            return new CoreInteger(memory, address);
        }

        public Instruction GetInstruction(uint address)
        {
            return new Instruction(new CoreInteger(memory, address), new CoreInteger(memory, address + 2));
        }

        public void ResetOutputs()
        {
            foreach (var key in Outputs.Keys.ToArray()) {
                Outputs[key] = 0;
            }
        }

        private void SetOutput(uint address, byte value)
        {
            if (Outputs.ContainsKey(address)) {
                Outputs[address] = value;
            }
        }

        public void Write(CoreInteger integer, uint address)
        {
            SetOutput(address, integer.LowByte);
            integer.Write(memory, address);
        }

        public byte this[uint address] {
            get {
                if (address < 0x10) {
                    return (byte)address;
                }
                return memory[address];
            }
            set {
                SetOutput(address, value);
                memory[address] = value;
            }
        }
    }
}
