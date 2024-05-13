using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using DES_File_MANAGER;
using DES_Round;
using DESKeyGenerator;

using DATA = DES_Round.RoundData_Class;

namespace DES_Round
{
    public class RoundData_Class
    {
        public uint initial_L;
        public uint initial_R;

        //For initial Perumtaion of L0 R0

        public static readonly int[] E_TABLE = new int[] {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        //Generated Sub_Keys

        public static ulong[] Key = KeyGeneratorData.Keys_table;

        //get initial L0 R0




        //S_Boxes

        public static readonly int[][,] S_BOXES = new int[8][,]
        {
            // S1
            new int[,]
            {
                { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
            },
            // S2
            new int[,]
            {
                { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
            },
            // S3
            new int[,]
            {
                { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
            },
            // S4
            new int[,]
            {
                { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
            },
            // S5
            new int[,]
            {
                { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
            },
            // S6
            new int[,]
            {
                { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
            },
            // S7
            new int[,]
            {
                { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
            },
            // S8
            new int[,]
            {
                { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
            }
        };

        public static readonly int[] P_TABLE = new int[]
        {
            16,  7, 20, 21,
            29, 12, 28, 17,
             1, 15, 23, 26,
             5, 18, 31, 10,
             2,  8, 24, 14,
            32, 27,  3,  9,
            19, 13, 30,  6,
            22, 11,  4, 25
        };

        public static readonly int[] FINAL_PERMUTATION = new int[]
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41,  9, 49, 17, 57, 25
        };

    }

    public class Round_Class
    {
        public uint Current_L;
        public uint Current_R;

        public static uint Function(uint R, ulong Key)
        {
            ulong R48 = ApplyE_Table(R);
            R48 = XOR_R46_KEY(R48, Key);
            uint R32 = ApplySBoxes(R48);
            R32 = ApplyPermutationP(R32);
            return R32;
        }

        public static ulong XOR_R46_KEY(ulong R, ulong Key) 
        {
            return R ^ Key;
        }

        public static (uint Current_L, uint Current_R) E_function(uint previous_L, uint previous_R, int iteration)
        {
            uint L0 = previous_R;
            uint R0 = previous_L ^ Function(previous_R, DATA.Key[iteration]);
            return (L0, R0);
        }

        public static ulong ApplyE_Table(uint Previous_R)
        {
            ulong R48 = 0;
            // Iterujemy przez 48 bitów rozszerzonego bloku R
            for (int i = 0; i < DATA.E_TABLE.Length; i++)
            {
                // Znajdź odpowiedni bit do skopiowania z 32-bitowego bloku R
                // E_TABLE zawiera indeksy 1-32, więc dostosowujemy przez odejmowanie 1
                int bitToCopy = DATA.E_TABLE[i] - 1;

                // Wyodrębnij bitToCopy (od 0 do 31) z Previous_R i przesuń go na odpowiednią pozycję
                // W DES pozycje są numerowane od lewej do prawej, więc bitToCopy=0 odpowiada najbardziej znaczącemu bitowi
                ulong bitValue = (Previous_R >> (31 - bitToCopy)) & 1UL;

                // Ustaw bit na odpowiedniej pozycji w 48-bitowym R (R48)
                // i-tej pozycji w R48 odpowiada 47-i, bo liczymy od 0 i od najbardziej znaczącego bitu
                R48 |= (bitValue << (47 - i));
            }
            return R48;
        }

        public static uint ApplySBoxes(ulong R48)
        {
            uint output = 0;
            for (int i = 0; i < 8; i++)
            {
                // Bity wejściowe dla S-Boxa są brane od końca (47 - 6*i) do (42 - 6*i)
                // gdzie najbardziej znaczący i najmniej znaczący bit określają wiersz,
                // a pozostałe bity określają kolumnę.

                int inputBits = (int)((R48 >> (42 - 6 * i)) & 0x3F); // 6-bitowy fragment dla S-Boxa

                int row = (inputBits & 0x20) >> 4 | (inputBits & 0x01); // bit 6 i bit 1
                int col = (inputBits >> 1) & 0x0F; // bity 2-5

                // Zwróć uwagę na właściwe odwołanie do tablicy S_BOXES
                int sboxValue = DATA.S_BOXES[i][row, col];

                output |= (uint)(sboxValue << (28 - 4 * i));
            }
            return output;
        }


        public static uint ApplyPermutationP(uint R32)
        {
            uint permutedR = 0;
            for (int i = 0; i < DATA.P_TABLE.Length; i++)
            {
                int bitFrom = DATA.P_TABLE[i];
                uint bitValue = (R32 >> (32 - bitFrom)) & 1U; // Correctly shift to get the bit
                permutedR |= bitValue << (31 - i); // Set the bit in the permuted result
            }
            return permutedR;
        }

        public static (uint, uint) PerformFullDesRounds(uint initial_L, uint initial_R, ulong[] subkeys)
        {
            uint L = initial_L;
            uint R = initial_R;

            for (int i = 0; i < 16; i++)
            {
                (L, R) = Round_Class.E_function(L, R, i);
            }

            return (L, R);
        }

        public static ulong ApplyFinalPermutation(uint L, uint R)
            {
            // Łączymy L i R w jeden 64-bitowy blok przed permutacją końcową

            ulong combined = CombineToUlong(R, L);
            ulong finalPermutation = 0;

                for (int i = 0; i < DATA.FINAL_PERMUTATION.Length; i++)
                {
                    int bitFrom = DATA.FINAL_PERMUTATION[i];
                    ulong bitValue = (combined >> (64 - bitFrom)) & 1UL; // Przesuwamy o (64 - bitFrom), aby zliczyć od najbardziej znaczącego bitu
                    finalPermutation |= (bitValue << (64 - i - 1)); // Ustawiamy bit w odpowiedniej pozycji w finalPermutation
                }

                return finalPermutation;
            }

        public static ulong CombineToUlong(uint high, uint low)
        {
            return ((ulong)high << 32) | low;
        }
    }
}
