
using System;

namespace MetroHash
{

    public partial class MetroHash
    {

        //---------------------------------------------------------------------------//
        /* rotate right idiom recognized by compiler*/
        private static ulong RotateRight(ulong lV, uint lK)
        {
            int lSignedK = (int)lK;
            return (lV >> lSignedK) | (lV << (64 - lSignedK));
        }

        //---------------------------------------------------------------------------//
        // unaligned reads, fast and safe on Nehalem and later microarchitectures
        private static ulong Read_u64(byte[] lData, uint lOffset)
        {
            return BitConverter.ToUInt64(lData, (int)lOffset);
        }

        //---------------------------------------------------------------------------//
        private static ulong Read_u32(byte[] lData, uint lOffset)
        {
            return BitConverter.ToUInt32(lData, (int)lOffset);
        }

        //---------------------------------------------------------------------------//
        private static ulong Read_u16(byte[] lData, uint lOffset)
        {
            return BitConverter.ToUInt16(lData, (int)lOffset);
        }

        //---------------------------------------------------------------------------//
        private static ulong Read_u8(byte[] lData, uint lOffset)
        {
            return lData[lOffset];
        }

    }

}
