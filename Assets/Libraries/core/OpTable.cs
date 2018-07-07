using System;
using System.Collections.Generic;
using System.Text;

namespace core
{
    public static class OpTable
    {
        public static Dictionary<byte, OpCode> Codes = new Dictionary<byte, OpCode> {
            { 0x00, OpCode.ADD },
            { 0x01, OpCode.SUB },
            { 0x02, OpCode.MUL },
            { 0x03, OpCode.DIV },
            { 0x04, OpCode.LDA },
            { 0x05, OpCode.STA },
            { 0x06, OpCode.JMP },
            { 0x07, OpCode.CMA },
            { 0x08, OpCode.JLT },
            { 0x09, OpCode.JEQ },
            { 0x0A, OpCode.JGT },
            { 0xFF, OpCode.HLT },
        };
    }
}
