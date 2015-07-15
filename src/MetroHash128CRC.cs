﻿
using System;

namespace MetroHash
{
    public partial class MetroHash
    {       

        //---------------------------------------------------------------------------//
        private const ulong K0_128_CRC_1  = 0xC83A91E1;
        private const ulong K1_128_CRC_1  = 0x8648DBDB;
        private const ulong K2_128_CRC_1  = 0x7BDEC03B;
        private const ulong K3_128_CRC_1  = 0x2F5870A5;

        //---------------------------------------------------------------------------//
        public static void Hash128CRC_1(byte[] lKey, uint lStartOffset, uint lLength, uint lSeed, out byte[] lOutput)
        {
            uint lKeyIndex = lStartOffset;
            uint lKeyEnd = lKeyIndex + lLength;
            
            if (lKey.Length < lKeyEnd)
            {
                throw new IndexOutOfRangeException("Given Key for hashing is not of expected length");
            }
    
            ulong[] lV = new ulong[4];
    
            lV[0] = ((lSeed - K0_128_CRC_1) * K3_128_CRC_1) + lLength;
            lV[1] = ((lSeed + K1_128_CRC_1) * K1_128_CRC_1) + lLength;
    
            if (lLength >= 32)
            {        
                lV[2] = ((lSeed + K0_128_CRC_1) * K1_128_CRC_1) + lLength;
                lV[3] = ((lSeed - K1_128_CRC_1) * K3_128_CRC_1) + lLength;

                do
                {
                    lV[0] ^= CRC32_u64(lV[0], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                    lV[1] ^= CRC32_u64(lV[1], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                    lV[2] ^= CRC32_u64(lV[2], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                    lV[3] ^= CRC32_u64(lV[3], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                }
                while (lKeyIndex <= (lKeyEnd - 32));

                lV[2] ^= RotateRight(((lV[0] + lV[3]) * K0_128_CRC_1) + lV[1], 34) * K1_128_CRC_1;
                lV[3] ^= RotateRight(((lV[1] + lV[2]) * K1_128_CRC_1) + lV[0], 37) * K0_128_CRC_1;
                lV[0] ^= RotateRight(((lV[0] + lV[2]) * K0_128_CRC_1) + lV[3], 34) * K1_128_CRC_1;
                lV[1] ^= RotateRight(((lV[1] + lV[3]) * K1_128_CRC_1) + lV[2], 37) * K0_128_CRC_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 16)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_CRC_1; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 34) * K3_128_CRC_1;
                lV[1] += Read_u64(lKey, lKeyIndex) * K2_128_CRC_1; lKeyIndex += 8; lV[1] = RotateRight(lV[1], 34) * K3_128_CRC_1;
                lV[0] ^= RotateRight((lV[0] * K2_128_CRC_1) + lV[1], 30) * K1_128_CRC_1;
                lV[1] ^= RotateRight((lV[1] * K3_128_CRC_1) + lV[0], 30) * K0_128_CRC_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 8)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_CRC_1; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 36) * K3_128_CRC_1;
                lV[0] ^= RotateRight((lV[0] * K2_128_CRC_1) + lV[1], 23) * K1_128_CRC_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 4)
            {
                lV[1] ^= CRC32_u64(lV[0], Read_u64(lKey, lKeyIndex)); lKeyIndex += 4;
                lV[1] ^= RotateRight((lV[1] * K3_128_CRC_1) + lV[0], 19) * K0_128_CRC_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 2)
            {
                lV[0] ^= CRC32_u64(lV[1], Read_u16(lKey, lKeyIndex)); lKeyIndex += 2;
                lV[0] ^= RotateRight((lV[0] * K2_128_CRC_1) + lV[1], 13) * K1_128_CRC_1;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 1)
            {
                lV[1] ^= CRC32_u64(lV[0], Read_u8(lKey, lKeyIndex));
                lV[1] ^= RotateRight((lV[1] * K3_128_CRC_1) + lV[0], 17) * K0_128_CRC_1;
            }

            lV[0] += RotateRight((lV[0] * K0_128_CRC_1) + lV[1], 11);
            lV[1] += RotateRight((lV[1] * K1_128_CRC_1) + lV[0], 26);
            lV[0] += RotateRight((lV[0] * K0_128_CRC_1) + lV[1], 11);
            lV[1] += RotateRight((lV[1] * K1_128_CRC_1) + lV[0], 26);
            
            lOutput = new byte[16];
            Buffer.BlockCopy(lV, 0, lOutput, 0, 16);
        }
        
        //---------------------------------------------------------------------------//
        private const ulong K0_128_CRC_2  = 0xEE783E2F;
        private const ulong K1_128_CRC_2  = 0xAD07C493;
        private const ulong K2_128_CRC_2  = 0x797A90BB;
        private const ulong K3_128_CRC_2  = 0x2E4B2E1B;

        //---------------------------------------------------------------------------//
        public static void Hash128CRC_2(byte[] lKey, uint lStartOffset, uint lLength, uint lSeed, out byte[] lOutput)
        {
            uint lKeyIndex = lStartOffset;
            uint lKeyEnd = lKeyIndex + lLength;
            
            if (lKey.Length < lKeyEnd)
            {
                throw new IndexOutOfRangeException("Given Key for hashing is not of expected length");
            }
    
            ulong[] lV = new ulong[4];

            lV[0] = ((lSeed - K0_128_CRC_2) * K3_128_CRC_2) + lLength;
            lV[1] = ((lSeed + K1_128_CRC_2) * K2_128_CRC_2) + lLength;
    
            if (lLength >= 32)
            {
                lV[2] = ((lSeed + K0_128_CRC_2) * K2_128_CRC_2) + lLength;
                lV[3] = ((lSeed - K1_128_CRC_2) * K3_128_CRC_2) + lLength;

                do
                {
                    lV[0] ^= CRC32_u64(lV[0], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                    lV[1] ^= CRC32_u64(lV[1], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                    lV[2] ^= CRC32_u64(lV[2], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                    lV[3] ^= CRC32_u64(lV[3], Read_u64(lKey, lKeyIndex)); lKeyIndex += 8;
                }
                while (lKeyIndex <= (lKeyEnd - 32));

                lV[2] ^= RotateRight(((lV[0] + lV[3]) * K0_128_CRC_2) + lV[1], 12) * K1_128_CRC_2;
                lV[3] ^= RotateRight(((lV[1] + lV[2]) * K1_128_CRC_2) + lV[0], 19) * K0_128_CRC_2;
                lV[0] ^= RotateRight(((lV[0] + lV[2]) * K0_128_CRC_2) + lV[3], 12) * K1_128_CRC_2;
                lV[1] ^= RotateRight(((lV[1] + lV[3]) * K1_128_CRC_2) + lV[2], 19) * K0_128_CRC_2;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 16)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_CRC_2; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 41) * K3_128_CRC_2;
                lV[1] += Read_u64(lKey, lKeyIndex) * K2_128_CRC_2; lKeyIndex += 8; lV[1] = RotateRight(lV[1], 41) * K3_128_CRC_2;
                lV[0] ^= RotateRight((lV[0] * K2_128_CRC_2) + lV[1], 10) * K1_128_CRC_2;
                lV[1] ^= RotateRight((lV[1] * K3_128_CRC_2) + lV[0], 10) * K0_128_CRC_2;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 8)
            {
                lV[0] += Read_u64(lKey, lKeyIndex) * K2_128_CRC_2; lKeyIndex += 8; lV[0] = RotateRight(lV[0], 34) * K3_128_CRC_2;
                lV[0] ^= RotateRight((lV[0] * K2_128_CRC_2) + lV[1], 22) * K1_128_CRC_2;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 4)
            {
                lV[1] ^= CRC32_u64(lV[0], Read_u32(lKey, lKeyIndex)); lKeyIndex += 4;
                lV[1] ^= RotateRight((lV[1] * K3_128_CRC_2) + lV[0], 14) * K0_128_CRC_2;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 2)
            {
                lV[0] ^= CRC32_u64(lV[1], Read_u16(lKey, lKeyIndex)); lKeyIndex += 2;
                lV[0] ^= RotateRight((lV[0] * K2_128_CRC_2) + lV[1], 15) * K1_128_CRC_2;
            }
    
            if ((lKeyEnd - lKeyIndex) >= 1)
            {
                lV[1] ^= CRC32_u64(lV[0], Read_u8(lKey, lKeyIndex));
                lV[1] ^= RotateRight((lV[1] * K3_128_CRC_2) + lV[0], 18) * K0_128_CRC_2;
            }

            lV[0] += RotateRight((lV[0] * K0_128_CRC_2) + lV[1], 15);
            lV[1] += RotateRight((lV[1] * K1_128_CRC_2) + lV[0], 27);
            lV[0] += RotateRight((lV[0] * K0_128_CRC_2) + lV[1], 15);
            lV[1] += RotateRight((lV[1] * K1_128_CRC_2) + lV[0], 27);

            lOutput = new byte[16];
            Buffer.BlockCopy(lV, 0, lOutput, 0, 16);
        }

    }
}
