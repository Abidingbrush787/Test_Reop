using Microsoft.VisualStudio.TestTools.UnitTesting;
using DESKeyGenerator;
using System;
using System.Linq;
using DES_File_MANAGER;

namespace DES_File_MANAGER_Tests
{
    [TestClass]
    public class FileProcessorTests
    {
       
        private static byte[] resultAfterPermutation;

        [TestMethod]
        public void ApplyInitialPermutationTest()
        {
            // Oczekiwane wyniki jako ciąg binarny
            byte[] expectedOutput = new byte[8] { 0xCC, 0x00, 0xCC, 0xFF, 0xF0, 0xAA, 0xF0, 0xAA };

            // Wejście jako 64-znakowy ciąg binarny
            string inputBitString = "0000000100100011010001010110011110001001101010111100110111101111";

            // Konwersja wejścia na tablicę bajtów
            byte[] inputBytes = FileProcessor.ConvertStringToByteArray(inputBitString);

            // Stosowanie permutacji początkowej
            byte[] result = FileProcessor.ApplyInitialPermutation(inputBytes);

            resultAfterPermutation = result;

            // Sprawdzenie, czy wynik zgadza się z oczekiwanym
            CollectionAssert.AreEqual(expectedOutput, result);
        }


        [TestMethod]
        public void SplitBlockIntoL0R0Test()
        {
            

            (uint L0, uint R0) = FileProcessor.SplitBlockIntoL0R0(resultAfterPermutation);
            uint expectedL0 = 0b11001100000000001100110011111111;
            uint expectedR0 = 0b11110000101010101111000010101010;

            Assert.AreEqual(expectedL0, L0);
            Assert.AreEqual(expectedR0, R0);
        }

    }
}
