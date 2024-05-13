using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DES_File_MANAGER
{
    public class FileProcessor
    {
        

        private static readonly int[] IP = new int[]
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        // Metoda przetwarza dane wejściowe: plik lub ciąg znaków (tekst)
        public static byte[][] ProcessInput(string input, bool isFilePath)
        {
            byte[] data = isFilePath ? File.ReadAllBytes(input) : ConvertStringToByteArray(input);
            return SeparateToBlocks(data, 8);
        }

        // Konwersja ciągu znaków na tablicę bajtów
        public static byte[] ConvertStringToByteArray(string bitString)
        {
            if (bitString.Length != 64)
                throw new ArgumentException("Bit string must be exactly 64 bits long.");

            byte[] byteArray = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                string byteString = bitString.Substring(i * 8, 8);
                byteArray[i] = Convert.ToByte(byteString, 2);
            }
            return byteArray;
        }

        // Dzielenie danych na 64-bitowe bloki z dopełnieniem
        public static byte[][] SeparateToBlocks(byte[] data, int blockSize)
        {
            // Oblicz ilość bloków wliczając dopełnienie
            int blocksCount = (data.Length + blockSize - 1) / blockSize;
            byte[][] blocks = new byte[blocksCount][];

            for (int i = 0; i < blocksCount; i++)
            {
                int blockStart = i * blockSize;
                int blockLength = Math.Min(blockSize, data.Length - blockStart);

                byte[] block = new byte[blockSize];
                Array.Copy(data, blockStart, block, 0, blockLength);

                // Jeśli to ostatni blok i potrzebuje dopełnienia
                if (i == blocksCount - 1 && blockLength != blockSize)
                {
                    block = ApplyPadding(block, blockSize);
                }

                blocks[i] = block;
            }

            return blocks;
        }



        // Stosowanie permutacji początkowej IP
        public static byte[] ApplyInitialPermutation(byte[] input)
        {
            if (input.Length != 8)
            {
                throw new ArgumentException("Input must be exactly 8 bytes long.");
            }

            byte[] output = new byte[8];
            for (int i = 0; i < 64; i++)
            {
                int bitValue = GetBit(input, IP[i] - 1);
                SetBit(ref output, i, bitValue);
            }

            return output;
        }


        // Pobieranie wartości bitu
        private static int GetBit(byte[] data, int position)
        {
            int byteIndex = position / 8;
            int bitIndex = position % 8;
            return (data[byteIndex] >> (7 - bitIndex)) & 0x01;
        }

        // Ustawianie wartości bitu
        private static void SetBit(ref byte[] data, int position, int value)
        {
            int byteIndex = position / 8;
            int bitIndex = position % 8;
            if (value == 1)
                data[byteIndex] |= (byte)(1 << (7 - bitIndex));
            else
                data[byteIndex] &= (byte)~(1 << (7 - bitIndex));
        }

        // Dzielenie bloku na L0 i R0
        // Dzielenie bloku na L0 i R0 przyjmując ciąg bitowy jako wejście
        public static (uint L0, uint R0) SplitBlockIntoL0R0(byte[] block)
        {
            if (block.Length != 8)
            {
                throw new ArgumentException("Block must be exactly 8 bytes long.");
            }

            // Konwersja 8 bajtów na jeden 64-bitowy ulong
            ulong block64 = BitConverter.ToUInt64(block.Reverse().ToArray(), 0); // Uwzględniamy big-endian format

            // Maska do wyodrębnienia lewej i prawej części (L0 i R0)
            ulong maskLeft = 0xFFFFFFFF00000000;
            ulong maskRight = 0x00000000FFFFFFFF;

            // Przesunięcie o 32 bity w prawo, aby uzyskać L0
            uint L0 = (uint)((block64 & maskLeft) >> 32);
            // Brak przesunięcia, bezpośrednie uzyskanie R0
            uint R0 = (uint)(block64 & maskRight);

            return (L0, R0);
        }

        public static byte[] ApplyPadding(byte[] dataBlock, int blockSize)
        {
            // Oblicz, ile bajtów paddingu potrzeba.
            int paddingLength = blockSize - (dataBlock.Length % blockSize);
            paddingLength = paddingLength == blockSize ? 0 : paddingLength; // Jeśli paddingLength równa się blockSize, ustaw 0, w przeciwnym razie zachowaj obliczoną wartość

            // Stwórz nowy blok danych, który zawiera oryginalne dane plus padding.
            byte[] paddedBlock = new byte[dataBlock.Length + paddingLength];
            Array.Copy(dataBlock, paddedBlock, dataBlock.Length);

            // Dodaj bajty paddingu.
            for (int i = dataBlock.Length; i < paddedBlock.Length; i++)
            {
                paddedBlock[i] = (byte)paddingLength;
            }


            return paddedBlock;
        }


        public static byte[] RemovePadding(byte[] paddedData, int blockSize)
        {
            int paddingLength = paddedData[paddedData.Length - 1];

            if (paddingLength < 1 || paddingLength > blockSize || paddingLength == 0)
            {
                return paddedData;
            }

            for (int i = 0; i < paddingLength; i++)
            {
                if (paddedData[paddedData.Length - 1 - i] != paddingLength)
                {
                    return paddedData;
                }
            }

            byte[] unpaddedData = paddedData.Take(paddedData.Length - paddingLength).ToArray();
            return unpaddedData;
        }








    }
}
