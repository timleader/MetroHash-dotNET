
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


        /*
        void metrohash128_2(const uint8_t * key, uint64_t len, uint32_t seed, uint8_t * out)
        {
            static const uint64_t k0 = 0xD6D018F5;
            static const uint64_t k1 = 0xA2AA033B;
            static const uint64_t k2 = 0x62992FC1;
            static const uint64_t k3 = 0x30BC5B29; 

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
                    v[0] += read_u64(ptr) * k0; ptr += 8; v[0] = rotate_right(v[0],29) + v[2];
                    v[1] += read_u64(ptr) * k1; ptr += 8; v[1] = rotate_right(v[1],29) + v[3];
                    v[2] += read_u64(ptr) * k2; ptr += 8; v[2] = rotate_right(v[2],29) + v[0];
                    v[3] += read_u64(ptr) * k3; ptr += 8; v[3] = rotate_right(v[3],29) + v[1];
                }
                while (ptr <= (end - 32));

                v[2] ^= rotate_right(((v[0] + v[3]) * k0) + v[1], 33) * k1;
                v[3] ^= rotate_right(((v[1] + v[2]) * k1) + v[0], 33) * k0;
                v[0] ^= rotate_right(((v[0] + v[2]) * k0) + v[3], 33) * k1;
                v[1] ^= rotate_right(((v[1] + v[3]) * k1) + v[2], 33) * k0;
            }
    
            if ((end - ptr) >= 16)
            {
                v[0] += read_u64(ptr) * k2; ptr += 8; v[0] = rotate_right(v[0],29) * k3;
                v[1] += read_u64(ptr) * k2; ptr += 8; v[1] = rotate_right(v[1],29) * k3;
                v[0] ^= rotate_right((v[0] * k2) + v[1], 29) * k1;
                v[1] ^= rotate_right((v[1] * k3) + v[0], 29) * k0;
            }
    
            if ((end - ptr) >= 8)
            {
                v[0] += read_u64(ptr) * k2; ptr += 8; v[0] = rotate_right(v[0],29) * k3;
                v[0] ^= rotate_right((v[0] * k2) + v[1], 29) * k1;
            }
    
            if ((end - ptr) >= 4)
            {
                v[1] += read_u32(ptr) * k2; ptr += 4; v[1] = rotate_right(v[1],29) * k3;
                v[1] ^= rotate_right((v[1] * k3) + v[0], 25) * k0;
            }
    
            if ((end - ptr) >= 2)
            {
                v[0] += read_u16(ptr) * k2; ptr += 2; v[0] = rotate_right(v[0],29) * k3;
                v[0] ^= rotate_right((v[0] * k2) + v[1], 30) * k1;
            }
    
            if ((end - ptr) >= 1)
            {
                  v[1] += read_u8 (ptr) * k2; v[1] = rotate_right(v[1],29) * k3;
                  v[1] ^= rotate_right((v[1] * k3) + v[0], 18) * k0;
            }
    
            v[0] += rotate_right((v[0] * k0) + v[1], 33);
            v[1] += rotate_right((v[1] * k1) + v[0], 33);
            v[0] += rotate_right((v[0] * k2) + v[1], 33);
            v[1] += rotate_right((v[1] * k3) + v[0], 33);

            memcpy(out, v, 16);
        }
         */
    }
}
