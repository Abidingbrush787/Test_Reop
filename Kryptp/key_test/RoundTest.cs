using Microsoft.VisualStudio.TestTools.UnitTesting;
using DES_Round;  
using DESKeyGenerator; 

namespace DESTests
{
    [TestClass]
    public class RoundTests
    {
        private static ulong[] Keys;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Przygotowanie klucza i wygenerowanie subkluczy
            ulong key = 0x133457799BBCDFF1;  // Przyk≥adowy klucz
            var keyGenerator = new KeyGeneratorData(key);
            keyGenerator.GenerateSubKesys();  // Generowanie subkluczy
            Keys = KeyGeneratorData.Keys_table;
        }

        [TestMethod]
        public void ApplyE_Table_ShouldExpandR0Correctly()
        {
            // Ustaw poczπtkowy R0
            uint R0 = 0b11110000101010101111000010101010U;
            // Oczekiwany wynik po rozszerzeniu
            ulong expected = 0b011110100001010101010101011110100001010101010101;

            ulong result = Round_Class.ApplyE_Table(R0);
            Assert.AreEqual(expected, result, "Expansion E is incorrect.");
        }

        [TestMethod]
        public void XORWithKey_ShouldGiveCorrectResult()
        {
            // Wynik rozszerzenia E(R0)
            ulong expandedR0 = 0b011110100001010101010101011110100001010101010101;
            // Klucz rundy K1
            ulong K1 = 0b000110110000001011101111111111000111000001110010UL;
            // Oczekiwany wynik XOR
            ulong expected = 0b11000010001011110111010100001100110010100100111UL;


            ulong result = Round_Class.XOR_R46_KEY(expandedR0, K1);
            Assert.AreEqual(expected, result, "XOR with key is incorrect.");
        }

        [TestMethod]
        public void ApplySBoxes_ShouldTransformCorrectly()
        {
            // Wynik z XOR z kluczem K1
            ulong input = 0b11000010001011110111010100001100110010100100111UL;
            // Oczekiwany wynik po S-Boxach
            uint expected = 0b01011100100000101011010110010111;

            // 0101 1100 1000 0010 1011 0101 1001 0111

            uint result = Round_Class.ApplySBoxes(input);
            Assert.AreEqual(expected, result, "S-Boxes transformation is incorrect.");
        }

        [TestMethod]
        public void Apply_Perpmutation_P()
        {
            // Ustaw poczπtkowy R32
            uint input = 0b01011100100000101011010110010111;
            // Oczekiwany wynik po permutacji
            uint expected = 0b00100011010010101010100110111011U;

            uint result = Round_Class.ApplyPermutationP(input);
            string resultBinary = Convert.ToString(result, 2).PadLeft(32, '0') + "U";
            Assert.AreEqual(expected, result, $"ApplyPermutationP Wynik: {resultBinary}");
        }

        [TestMethod]
        public void Check_Function()
        {
            // Ustaw poczπtkowy R0
            uint R0 = 0b11110000101010101111000010101010U;

            ulong K1 = 0b000110110000001011101111111111000111000001110010UL;

            // Oczekiwany wynik po rozszerzeniu
            ulong expected = 0b00100011010010101010100110111011U;

            ulong result = Round_Class.Function(R0, K1);
            Assert.AreEqual(expected, result, "Expansion E is incorrect.");
        }
       
           
        [TestMethod]
        public void TestPerformFullDesRounds()
        {
            
            uint L0 = 0b11001100000000001100110011111111;
            uint R0 = 0b11110000101010101111000010101010;

            uint expectedL = 0b01000011010000100011001000110100U; 
            uint expectedR = 0b00001010010011001101100110010101U;

            // Wykonaj 16 rund algorytmu DES
            (uint resultL, uint resultR) = Round_Class.PerformFullDesRounds(L0, R0, Keys);

            // Sprawdü, czy wyniki sπ zgodne z oczekiwanymi wartoúciami
            Assert.AreEqual(expectedL, resultL, $"Final L does not match expected. Got: {resultL}, Expected: {expectedL}");
            Assert.AreEqual(expectedR, resultR, $"Final R does not match expected. Got: {resultR}, Expected: {expectedR}");
        }

        [TestMethod]
        public void CHeck_Cobine() 
        {
            uint L16 = 0b01000011010000100011001000110100U;
            uint R16 = 0b00001010010011001101100110010101U;

            ulong result = Round_Class.CombineToUlong(R16, L16);
            ulong expected = 0b0000101001001100110110011001010101000011010000100011001000110100UL;
            Assert.AreEqual(expected, result, $"Combined message does not match expected.");
        }

        [TestMethod]
        public void CHeck_Final_Permutation()
        {
            uint L16 = 0b01000011010000100011001000110100U;
            uint R16 = 0b00001010010011001101100110010101U;

            ulong expectedIP = 0b1000010111101000000100110101010000001111000010101011010000000101UL;

            ulong resultIP = Round_Class.ApplyFinalPermutation(L16, R16);

            // Sprawdü, czy wyniki sπ zgodne z oczekiwanymi wartoúciami
            Assert.AreEqual(expectedIP, resultIP, $"Final message does not match expected. Got: {resultIP}, Expected: {expectedIP}");
        }
    }


 }





