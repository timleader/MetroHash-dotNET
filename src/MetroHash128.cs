
using System;

namespace MetroHash
{
    public partial class MetroHash
    {
        //---------------------------------------------------------------------------//
        private const ulong K0_128_1  = 0xC83A91E1;
        private const ulong K1_128_1  = 0x8648DBDB;
        private const ulong K2_128_1  = 0x7BDEC03B;
        private const ulong K3_128_1  = 0x2F5870A5;

        //---------------------------------------------------------------------------//
        public static void Hash128_1(byte[] lKey, uint lStartOffset, uint lLength, uint lSeed, out byte[] lOutput)
        {
            uint lKeyIndex = lStartOffset;
            uint lKeyEnd = lKeyIndex + lLength;

            if (lKey.Length < lKeyEnd)
            {
                throw new IndexOutOfRangeException("Given Key for hashing is not of expected length");
            }
    
            ulong[] lV = new ulong[4];
    
            lV[0] = ((lSeed - K0_128_1) * K3_128_1) + lLength;
            lV[1] = ((lSeed + K1_128_1) * K2_128_1) + lLength;
    
            if (lLength >= 32)
            {        
                lV[2] = ((lSeed + K0_128_1) * K2_128_1) + lLength;
                lV[3] = ((lSeed - K1_128_1) * K3_128_1) + lLength;

                do
                {
                    lV[0] += Read_u64(lKey, lKeyIndex) * K0_128_1; lKeyIndex += 8; lV[0] = RotateRight(lV[0],29) + lV[2];
                    lV[1] += Read_u64(lKey, lKeyIndex) * K1_128_1; lKeyIndex += 8; lV[1] = RotateRight(lV[1],29) + lV[3];
                    lV[2] += Read_u64(lKey, lKeyIndex) * K2_128_1; lKeyIndex += 8; lV[2] = RotateRight(lV[2],29) + lV[0];
                    lV[3] += Read_u64(lKey, lKeyIndex) * K3_128_1; lKeyIndex += 8; lV[3] = RotateRight(lV[3],29) + lV[1];
                }
                while (lKeyIndex <= (lKeyEnd - 32));

                lV[2] ^= RotateRight(((lV[0] + lV[3]) * K0_128_1) + lV[1], 26) * K1_128_1;
                lV[3] ^= RotateRight(((lV[1] + lV[2]) * K1_128_1) + lV[0], 26) * K0_128_1;
                lV[0] ^= RotateRight(((lV[0] + lV[2]) * K0_128_1) + lV[3], 26) * K1_128_1;
                lV[1] ^= RotateRight(((lV[1] + lV[3]) * K1_128_1) + lV[2], 30) * K0_128_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 16)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_1; lKeyIndex += 8; lV[0] = RotateRight(lV[0],33) * K3_128_1;
                lV[1] += Read_u64(lKey, lKeyIndex) * K2_128_1; lKeyIndex += 8; lV[1] = RotateRight(lV[1],33) * K3_128_1;
                lV[0] ^= RotateRight((lV[0] * K2_128_1) + lV[1], 17) * K1_128_1;
                lV[1] ^= RotateRight((lV[1] * K3_128_1) + lV[0], 17) * K0_128_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 8)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_1; lKeyIndex += 8; lV[0] = RotateRight(lV[0],33) * K3_128_1;
                lV[0] ^= RotateRight((lV[0] * K2_128_1) + lV[1], 20) * K1_128_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 4)
            {
                lV[1] += Read_u32(lKey, lKeyIndex) * K2_128_1; lKeyIndex += 4; lV[1] = RotateRight(lV[1],33) * K3_128_1;
                lV[1] ^= RotateRight((lV[1] * K3_128_1) + lV[0], 18) * K0_128_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 2)
            {
                lV[0] += Read_u16(lKey, lKeyIndex) * K2_128_1; lKeyIndex += 2; lV[0] = RotateRight(lV[0],33) * K3_128_1;
                lV[0] ^= RotateRight((lV[0] * K2_128_1) + lV[1], 24) * K1_128_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 1)
            {
                lV[1] += Read_u8 (lKey, lKeyIndex) * K2_128_1; lV[1] = RotateRight(lV[1],33) * K3_128_1;
                lV[1] ^= RotateRight((lV[1] * K3_128_1) + lV[0], 24) * K0_128_1;
            }
    
            lV[0] += RotateRight((lV[0] * K0_128_1) + lV[1], 13);
            lV[1] += RotateRight((lV[1] * K1_128_1) + lV[0], 37);
            lV[0] += RotateRight((lV[0] * K2_128_1) + lV[1], 13);
            lV[1] += RotateRight((lV[1] * K3_128_1) + lV[0], 37);

            lOutput = new byte[16];
            Buffer.BlockCopy(lV, 0, lOutput, 0, 16);
        }
        
        //---------------------------------------------------------------------------//
        private const ulong K0_128_2  = 0xD6D018F5;
        private const ulong K1_128_2  = 0xA2AA033B;
        private const ulong K2_128_2  = 0x62992FC1;
        private const ulong K3_128_2  = 0x30BC5B29;

        //---------------------------------------------------------------------------//
        public static void Hash128_2(byte[] lKey, uint lStartOffset, uint lLength, uint lSeed, out byte[] lOutput)
        {
            uint lKeyIndex = lStartOffset;
            uint lKeyEnd = lKeyIndex + lLength;

            if (lKey.Length < lKeyEnd)
            {
                throw new IndexOutOfRangeException("Given Key for hashing is not of expected length");
            }
    
            ulong[] lV = new ulong[4];
    
            lV[0] = ((lSeed - K0_128_2) * K3_128_2) + lLength;
            lV[1] = ((lSeed + K1_128_2) * K2_128_2) + lLength;
    
            if (lLength >= 32)
            {        
                lV[2] = ((lSeed + K0_128_2) * K2_128_2) + lLength;
                lV[3] = ((lSeed - K1_128_2) * K3_128_2) + lLength;

                do
                {
                    lV[0] += Read_u64(lKey, lKeyIndex) * K0_128_2; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 29) + lV[2];
                    lV[1] += Read_u64(lKey, lKeyIndex) * K1_128_2; lKeyIndex += 8; lV[1] = RotateRight(lV[1], 29) + lV[3];
                    lV[2] += Read_u64(lKey, lKeyIndex) * K2_128_2; lKeyIndex += 8; lV[2] = RotateRight(lV[2], 29) + lV[0];
                    lV[3] += Read_u64(lKey, lKeyIndex) * K3_128_2; lKeyIndex += 8; lV[3] = RotateRight(lV[3], 29) + lV[1];
                }
                while (lKeyIndex <= (lKeyEnd - 32));

                lV[2] ^= RotateRight(((lV[0] + lV[3]) * K0_128_2) + lV[1], 33) * K1_128_2;
                lV[3] ^= RotateRight(((lV[1] + lV[2]) * K1_128_2) + lV[0], 33) * K0_128_2;
                lV[0] ^= RotateRight(((lV[0] + lV[2]) * K0_128_2) + lV[3], 33) * K1_128_2;
                lV[1] ^= RotateRight(((lV[1] + lV[3]) * K1_128_2) + lV[2], 33) * K0_128_2;
            }

            if ((lKeyEnd - lKeyIndex) >= 16)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_2; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 29) * K3_128_2;
                lV[1] += Read_u64(lKey, lKeyIndex) * K2_128_2; lKeyIndex += 8; lV[1] = RotateRight(lV[1], 29) * K3_128_2;
                lV[0] ^= RotateRight((lV[0] * K2_128_2) + lV[1], 29) * K1_128_2;
                lV[1] ^= RotateRight((lV[1] * K3_128_2) + lV[0], 29) * K0_128_2;
            }

            if ((lKeyEnd - lKeyIndex) >= 8)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_2; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 29) * K3_128_2;
                lV[0] ^= RotateRight((lV[0] * K2_128_2) + lV[1], 29) * K1_128_2;
            }

            if ((lKeyEnd - lKeyIndex) >= 4)
            {
                lV[1] += Read_u32(lKey, lKeyIndex) * K2_128_2; lKeyIndex += 4; lV[1] = RotateRight(lV[1], 29) * K3_128_2;
                lV[1] ^= RotateRight((lV[1] * K3_128_2) + lV[0], 25) * K0_128_2;
            }

            if ((lKeyEnd - lKeyIndex) >= 2)
            {
                lV[0] += Read_u16(lKey, lKeyIndex) * K2_128_2; lKeyIndex += 2; lV[0] = RotateRight(lV[0], 29) * K3_128_2;
                lV[0] ^= RotateRight((lV[0] * K2_128_2) + lV[1], 30) * K1_128_2;
            }

            if ((lKeyEnd - lKeyIndex) >= 1)
            {
                lV[1] += Read_u8(lKey, lKeyIndex) * K2_128_2; lV[1] = RotateRight(lV[1], 29) * K3_128_2;
                lV[1] ^= RotateRight((lV[1] * K3_128_2) + lV[0], 18) * K0_128_2;
            }

            lV[0] += RotateRight((lV[0] * K0_128_2) + lV[1], 33);
            lV[1] += RotateRight((lV[1] * K1_128_2) + lV[0], 33);
            lV[0] += RotateRight((lV[0] * K2_128_2) + lV[1], 33);
            lV[1] += RotateRight((lV[1] * K3_128_2) + lV[0], 33);

            lOutput = new byte[16];
            Buffer.BlockCopy(lV, 0, lOutput, 0, 16);
        }

    }
}
