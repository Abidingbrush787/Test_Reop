using System;


namespace DESKeyGenerator
{
    public class KeyGeneratorData
    {

        //basic 64 bit Key
        public ulong Key { get; }


        //Constructor

        public KeyGeneratorData(ulong key)
        {
            this.Key = key;
        }

        // PC-1 Table for generating 56bit sub-key
        private static readonly int[] PC1 = new int[] {
            57, 49, 41, 33, 25, 17, 9, 1,
            58, 50, 42, 34, 26, 18, 10, 2,
            59, 51, 43, 35, 27, 19, 11, 3,
            60, 52, 44, 36, 63, 55, 47, 39,
            31, 23, 15, 7, 62, 54, 46, 38,
            30, 22, 14, 6, 61, 53, 45, 37,
            29, 21, 13, 5, 28, 20, 12, 4
        };

        // PC-2 Table for generating 48bit sub-key from 56bit
        private static readonly int[] PC2 = new int[] {
            14, 17, 11, 24, 1, 5, 3, 28,
            15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        // Shifts for each round of key generation
        private static readonly int[] Shifts = new int[] {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };

        //sub keyes be stroed here

        public static ulong[] Keys_table = new ulong[16];


        //Helping function to binary conversion
        private static string ToBinaryString(ulong value, int length)
        {
            return Convert.ToString((long)value, 2).PadLeft(length, '0');
        }

        //--------------------------------------------------------------PC1
        public static ulong applyPC1(ulong key)
        {
            ulong result = 0;

            ulong keybuffor = key;


            //go through PC1 Table
            for (int i = 0; i < PC1.Length; i++)
            {
                int bitPosition = PC1[i] - 1;
                ulong bitValue = (key >> (64 - bitPosition - 1)) & 1;
                result |= (bitValue << (56 - i - 1));
            }

            return result;
        }

        //--------------------------------------------------------------PC2
        public static ulong applyPC2(ulong combinedKey)
        {

            ulong result = 0;

            for (int i = 0; i < PC2.Length; i++)
            {
                int bitPosition = PC2[i];
                ulong bitValue = (combinedKey >> (56 - bitPosition)) & 0x01;
                result |= (bitValue << (47 - i));
            }
            return result;
        }


        private ulong RotateLeft(ulong half, int shifts, ulong mask)
        {
            return ((half << shifts) | (half >> (28 - shifts))) & mask;
        }

        public void GenerateSubKesys() 
        {
            ulong mask_up = 0x0FFFFFFF;

            //Console.WriteLine($"Initial baseKey: {ToBinaryString(Key, 64)}");

            //Divide on 2 parts (C0 and D0)

            ulong Key_56 = applyPC1(Key);

            //Console.WriteLine($"Initial Key (56-bit): {ToBinaryString(Key_56, 56)}");


            ulong C = (Key_56 >> 28) & mask_up; //Przesunięcie bitów od 28 i uzycie maski
            ulong D = Key_56 & mask_up;

            //Console.WriteLine($"Initial C: {ToBinaryString(C, 28)}");
            //Console.WriteLine($"Initial D: {ToBinaryString(D, 28)}");

            for (int i = 0; i < Shifts.Length; i++) //Leftshifts
            {
                C = RotateLeft(C, Shifts[i], mask_up);
                D = RotateLeft(D, Shifts[i], mask_up);

                //Console.WriteLine($"Round {i + 1}: C = {ToBinaryString(C, 28)}, D = {ToBinaryString(D, 28)}");

                // Concatenate C and D
                ulong CD = (C << 28) | D;

                //Console.WriteLine($"Round {i + 1} CD (before PC-2): {Convert.ToString((long)CD, 2).PadLeft(56, '0')}");

                // Apply PC2
                ulong subKey = applyPC2(CD);

                //Console.WriteLine($"SubKey {i + 1}: {Convert.ToString((long)subKey, 2).PadLeft(48, '0')}");

                // Save to Keys array
                Keys_table[i] = subKey;

            }

        }

    }
}
