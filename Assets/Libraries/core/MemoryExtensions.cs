using System;
using System.Collections.Generic;
using System.Text;

namespace core
{
    public static class MemoryExtensions
    {
        public static CoreInteger GetCoreInteger(this byte[] memory, uint idx)
        {
            return new CoreInteger(memory, idx);
        }

        public static Instruction GetInstruction(this byte[] memory, uint idx)
        {
            return new Instruction(memory.GetCoreInteger(idx), memory.GetCoreInteger(idx + 2));
        }
    }
}
