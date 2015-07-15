
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

        /*
        void metrohash128crc_2(const uint8_t * key, uint64_t len, uint32_t seed, uint8_t * out)
        {
            static const uint64_t k0 = 0xEE783E2F;
            static const uint64_t k1 = 0xAD07C493;
            static const uint64_t k2 = 0x797A90BB;
            static const uint64_t k3 = 0x2E4B2E1B;

            const uint8_t * ptr = reinterpret_cast<const uint8_t*>(key);
            const uint8_t * const end = ptr + len;
    
            uint64_t v[4];
    
            v[0] = ((static_cast<uint64_t>(seed) - k0) * k3) + len;
            v[1] = ((static_cast<uint64_t>(seed) + k1) * k2) + len;
    
            if (len >= 32)
            {        
                v[2] = ((static_cast<uint64_t>(seed) + k0) * k2) + len;
                v[3] = ((static_cast<uint64_t>(seed) - k1) * k3) + len;

                do
                {
                    v[0] ^= _mm_crc32_u64(v[0], read_u64(ptr)); ptr += 8;
                    v[1] ^= _mm_crc32_u64(v[1], read_u64(ptr)); ptr += 8;
                    v[2] ^= _mm_crc32_u64(v[2], read_u64(ptr)); ptr += 8;
                    v[3] ^= _mm_crc32_u64(v[3], read_u64(ptr)); ptr += 8;
                }
                while (ptr <= (end - 32));

                v[2] ^= rotate_right(((v[0] + v[3]) * k0) + v[1], 12) * k1;
                v[3] ^= rotate_right(((v[1] + v[2]) * k1) + v[0], 19) * k0;
                v[0] ^= rotate_right(((v[0] + v[2]) * k0) + v[3], 12) * k1;
                v[1] ^= rotate_right(((v[1] + v[3]) * k1) + v[2], 19) * k0;
            }
    
            if ((end - ptr) >= 16)
            {
                v[0] += read_u64(ptr) * k2; ptr += 8; v[0] = rotate_right(v[0],41) * k3;
                v[1] += read_u64(ptr) * k2; ptr += 8; v[1] = rotate_right(v[1],41) * k3;
                v[0] ^= rotate_right((v[0] * k2) + v[1], 10) * k1;
                v[1] ^= rotate_right((v[1] * k3) + v[0], 10) * k0;
            }
    
            if ((end - ptr) >= 8)
            {
                v[0] += read_u64(ptr) * k2; ptr += 8; v[0] = rotate_right(v[0],34) * k3;
                v[0] ^= rotate_right((v[0] * k2) + v[1], 22) * k1;
            }
    
            if ((end - ptr) >= 4)
            {
                v[1] ^= _mm_crc32_u64(v[0], read_u32(ptr)); ptr += 4;
                v[1] ^= rotate_right((v[1] * k3) + v[0], 14) * k0;
            }
    
            if ((end - ptr) >= 2)
            {
                v[0] ^= _mm_crc32_u64(v[1], read_u16(ptr)); ptr += 2;
                v[0] ^= rotate_right((v[0] * k2) + v[1], 15) * k1;
            }
    
            if ((end - ptr) >= 1)
            {
                v[1] ^= _mm_crc32_u64(v[0], read_u8 (ptr));
                v[1] ^= rotate_right((v[1] * k3) + v[0],  18) * k0;
            }
    
            v[0] += rotate_right((v[0] * k0) + v[1], 15);
            v[1] += rotate_right((v[1] * k1) + v[0], 27);
            v[0] += rotate_right((v[0] * k0) + v[1], 15);
            v[1] += rotate_right((v[1] * k1) + v[0], 27);

            memcpy(out, v, 16);
        }*/
    }
}
