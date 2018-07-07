using System;

namespace core
{
    public class Processor
    {
        public Memory memory = new Memory();

        public CoreInteger PC = 0;

        public CoreInteger A = 0;

        public bool CR = false;

        public Comparison CC = Comparison.Equal;

        public bool Halted = false;

        public const int MaxAddress = 0x3FFFFF;

        public Action<OpCode, uint, uint, uint> tracer { get; set; }

        public void Load(byte[] program, uint startIndex = 1000)
        {
            memory.Load(program, startIndex);
        }

        public void Step()
        {
            var ci = memory.GetInstruction(PC);

            int val = 0;

            if (ci.InRange) {
                val = memory.GetCoreInteger(ci.Target);
            }

            int aval = A;

            memory.ResetOutputs();

            tracer?.Invoke(ci.OpCode, ci.Target, A, (uint)val);

            switch (ci.OpCode) {
                case OpCode.ADD:
                    int result = A + val;
                    if (result >= 0xFFFF) {
                        CR = true;
                    } else {
                        CR = false;
                    }
                    A = result;
                    break;
                case OpCode.SUB:
                    A = A - val;
                    break;
                case OpCode.MUL:
                    A = A * val;
                    break;
                case OpCode.DIV:
                    A = A / val;
                    break;
                case OpCode.LDA:
                    A = val;
                    break;
                case OpCode.STA:
                    if (ci.InRange) {
                        memory.Write(A, ci.Target);
                    }
                    break;
                case OpCode.JMP:
                    if (ci.InRange) {
                        PC = ci.Target - 4;
                    }
                    break;
                case OpCode.CMA:
                    if (ci.InRange) {
                        var memval = memory.GetCoreInteger(ci.Target);
                        if (A > memval) {
                            CC = Comparison.Greater;
                        } else if (memval == A) {
                            CC = Comparison.Equal;
                        } else {
                            CC = Comparison.Less;
                        }
                    } else {
                        CC = Comparison.Equal;
                    }
                    break;
                case OpCode.JLT:
                    if (ci.InRange && CC == Comparison.Less) {
                        PC = ci.Target - 4;
                    }
                    break;
                case OpCode.JEQ:
                    if (ci.InRange && CC == Comparison.Equal) {
                        PC = ci.Target - 4;
                    }
                    break;
                case OpCode.JGT:
                    if (ci.InRange && CC == Comparison.Greater) {
                        PC = ci.Target - 4;
                    }
                    break;
                case OpCode.HLT:
                    Halted = true;
                    break;
            }

            PC += 4;
        }
    }
}
