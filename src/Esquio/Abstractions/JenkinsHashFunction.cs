using System;
using System.Text;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Non-crypto hash with Jenkins function
    /// https://en.wikipedia.org/wiki/Jenkins_hash_function
    /// </summary>
    static class JenkinsHashFunction
    {
        public static short ResolveToLogicalPartition(string value, short entityPartitionCount)
        {
            if (value == null)
            {
                return 0;
            }

            JenkinsHashFunction.ComputeHash(Encoding.ASCII.GetBytes(value.ToUpper()), 0U, 0U, out uint hash1, out uint hash2);

            return (short)Math.Abs((long)(hash1 ^ hash2) % (long)entityPartitionCount);
        }


        static void ComputeHash(byte[] data, uint seed1, uint seed2, out uint hash1, out uint hash2)
        {
            int num1;
            uint num2 = (uint)(num1 = (int)(uint)(3735928559UL + (ulong)data.Length + (ulong)seed1));
            uint num3 = (uint)num1;
            uint num4 = (uint)num1;
            uint num5 = num2 + seed2;
            int startIndex = 0;
            int length = data.Length;
            while (length > 12)
            {
                uint num6 = num4 + BitConverter.ToUInt32(data, startIndex);
                uint num7 = num3 + BitConverter.ToUInt32(data, startIndex + 4);
                uint num8 = num5 + BitConverter.ToUInt32(data, startIndex + 8);
                uint num9 = num6 - num8 ^ (num8 << 4 | num8 >> 28);
                uint num10 = num8 + num7;
                uint num11 = num7 - num9 ^ (num9 << 6 | num9 >> 26);
                uint num12 = num9 + num10;
                uint num13 = num10 - num11 ^ (num11 << 8 | num11 >> 24);
                uint num14 = num11 + num12;
                uint num15 = num12 - num13 ^ (num13 << 16 | num13 >> 16);
                uint num16 = num13 + num14;
                uint num17 = num14 - num15 ^ (num15 << 19 | num15 >> 13);
                num4 = num15 + num16;
                num5 = num16 - num17 ^ (num17 << 4 | num17 >> 28);
                num3 = num17 + num4;
                startIndex += 12;
                length -= 12;
            }
            switch (length)
            {
                case 0:
                    hash1 = num5;
                    hash2 = num3;
                    return;
                case 1:
                    num4 += (uint)data[startIndex];
                    break;
                case 2:
                    num4 += (uint)data[startIndex + 1] << 8;
                    goto case 1;
                case 3:
                    num4 += (uint)data[startIndex + 2] << 16;
                    goto case 2;
                case 4:
                    num4 += BitConverter.ToUInt32(data, startIndex);
                    break;
                case 5:
                    num3 += (uint)data[startIndex + 4];
                    goto case 4;
                case 6:
                    num3 += (uint)data[startIndex + 5] << 8;
                    goto case 5;
                case 7:
                    num3 += (uint)data[startIndex + 6] << 16;
                    goto case 6;
                case 8:
                    num3 += BitConverter.ToUInt32(data, startIndex + 4);
                    num4 += BitConverter.ToUInt32(data, startIndex);
                    break;
                case 9:
                    num5 += (uint)data[startIndex + 8];
                    goto case 8;
                case 10:
                    num5 += (uint)data[startIndex + 9] << 8;
                    goto case 9;
                case 11:
                    num5 += (uint)data[startIndex + 10] << 16;
                    goto case 10;
                case 12:
                    num4 += BitConverter.ToUInt32(data, startIndex);
                    num3 += BitConverter.ToUInt32(data, startIndex + 4);
                    num5 += BitConverter.ToUInt32(data, startIndex + 8);
                    break;
            }
            uint num18 = (num5 ^ num3) - (num3 << 14 | num3 >> 18);
            uint num19 = (num4 ^ num18) - (num18 << 11 | num18 >> 21);
            uint num20 = (num3 ^ num19) - (num19 << 25 | num19 >> 7);
            uint num21 = (num18 ^ num20) - (num20 << 16 | num20 >> 16);
            uint num22 = (num19 ^ num21) - (num21 << 4 | num21 >> 28);
            uint num23 = (num20 ^ num22) - (num22 << 14 | num22 >> 18);
            uint num24 = (num21 ^ num23) - (num23 << 24 | num23 >> 8);
            hash1 = num24;
            hash2 = num23;
        }
    }
}
