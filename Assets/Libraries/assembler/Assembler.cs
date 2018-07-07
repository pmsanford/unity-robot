using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using core;

namespace assembler
{
    public class Assembler
    {
        public Assembler()
        {
        }

        public byte[] Assemble(string input, uint startAddr = 0)
        {
            var lookup = OpTable.Codes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            var labels = new Dictionary<string, uint>();
            uint addr = startAddr;
            foreach (var line in input.Split('\n')) {
                if (line.Trim().Length == 0 || line.Trim().StartsWith("#")) {
                    continue;
                }
                var tokens = line.Split(' ', '\t').Where(s => s.Length > 0).ToArray();

                var idx = 0;


                if (tokens.Length > 2) {
                    labels.Add(tokens[idx].Trim().ToUpper(), addr);
                    idx++;
                }
                if (tokens[idx].Equals("BYT", StringComparison.InvariantCultureIgnoreCase)) {
                    addr += 1;
                } else {
                    addr += 4;
                }
            }

            addr = startAddr;

            var binary = new List<byte[]>();

            foreach (var line in input.Split('\n')) {
                if (line.Trim().Length == 0 || line.Trim().StartsWith("#")) {
                    continue;
                }
                var idx = 0;
                var tokens = line.Split(' ', '\t').Where(s => s.Length > 0).ToArray();

                if (tokens.Length > 2) {
                    idx++;
                }

                var instruction = new byte[4];

                if (!tokens[idx].Equals("BYT", StringComparison.InvariantCultureIgnoreCase)) {
                    var op = (OpCode)Enum.Parse(typeof(OpCode), tokens[idx], true);
                    instruction[0] = lookup[op];
                }
                idx++;

                uint advance = 4;
                uint target;
                if (tokens[idx - 1].Equals("BYT", StringComparison.InvariantCultureIgnoreCase)) {
                    instruction = new[] { ParseByte(tokens[idx]) };
                    advance = 1;
                } else if (TryParseUint(tokens[idx], out target)) {
                    if (target >= Processor.MaxAddress) {
                        throw new Exception("Target out of range");
                    }
                    var tarBytes = BitConverter.GetBytes(target);
                    instruction[1] = tarBytes[2];
                    instruction[2] = tarBytes[0];
                    instruction[3] = tarBytes[1];
                } else if (labels.ContainsKey(tokens[idx].Trim().ToUpper())) {
                    var tarBytes = BitConverter.GetBytes(labels[tokens[idx].Trim().ToUpper()]);
                    instruction[1] = tarBytes[2];
                    instruction[2] = tarBytes[0];
                    instruction[3] = tarBytes[1];
                } else {
                    throw new Exception($"Unrecognized label {tokens[idx]} on line {line}");
                }

                binary.Add(instruction);

                addr += advance;
            }

            return binary.SelectMany(b => b).ToArray();
        }

        private bool IsHex(string val)
        {
            return val.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase);
        }

        private bool TryParseUint(string val, out uint value)
        {
            if (IsHex(val)) {
                return uint.TryParse(val.Substring(2), NumberStyles.HexNumber, new NumberFormatInfo(), out value);
            }
            return uint.TryParse(val, out value);
        }

        private byte ParseByte(string val)
        {
            if (IsHex(val)) {
                return byte.Parse(val.Substring(2), NumberStyles.HexNumber);
            }
            return byte.Parse(val);
        }
    }
}
