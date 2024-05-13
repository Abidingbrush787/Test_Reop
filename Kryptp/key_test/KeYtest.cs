using Microsoft.VisualStudio.TestTools.UnitTesting;
using DESKeyGenerator; // U¿yj odpowiedniej przestrzeni nazw dla Twojej klasy generatora

namespace DESKeyGeneratorTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DESKeyGenerator; // U¿yj odpowiedniej przestrzeni nazw dla Twojej klasy generatora

    namespace DESKeyGeneratorTests
    {
        [TestClass]
        public class KeyGeneratorTests
        {
            // Tablica z oczekiwanymi wartoœciami podkluczy (w formacie ulong)
            private readonly ulong[] expectedSubKeys = new ulong[]
            {
            0b000110110000001011101111111111000111000001110010UL,
            0b011110011010111011011001110110111100100111100101UL,
            0b010101011111110010001010010000101100111110011001UL,
            0b011100101010110111010110110110110011010100011101UL,
            0b011111001110110000000111111010110101001110101000UL,
            0b011000111010010100111110010100000111101100101111UL,
            0b111011001000010010110111111101100001100010111100UL,
            0b111101111000101000111010110000010011101111111011UL,
            0b111000001101101111101011111011011110011110000001UL,
            0b101100011111001101000111101110100100011001001111UL,
            0b001000010101111111010011110111101101001110000110UL,
            0b011101010111000111110101100101000110011111101001UL,
            0b100101111100010111010001111110101011101001000001UL,
            0b010111110100001110110111111100101110011100111010UL,
            0b101111111001000110001101001111010011111100001010UL,
            0b110010110011110110001011000011100001011111110101UL
            };

            [TestMethod]
            public void TestSubKeyGeneration()
            {
                // Przyk³adowy klucz bazowy
                ulong baseKey = 0x133457799BBCDFF1;
                KeyGeneratorData generator = new KeyGeneratorData(baseKey);
                generator.GenerateSubKesys();

                for (int i = 0; i < expectedSubKeys.Length; i++)
                {
                    // Porównanie bezpoœrednio ulong podkluczy
                    Assert.AreEqual(expectedSubKeys[i], KeyGeneratorData.Keys_table[i], $"SubKey K{i + 1} does not match the expected value.");
                }
            }
        }
    }

}
