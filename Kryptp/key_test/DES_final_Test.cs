using Microsoft.VisualStudio.TestTools.UnitTesting;
using DESKeyGenerator;
using System;
using System.Linq;
using DES_Algorithm_N;
using DES_File_MANAGER;
using DES_Round;
using System.Windows.Forms;

namespace DES_Algorithm_FINAL_TEST
{
    [TestClass]
    public class FileProcessorTests
    {
       
        [TestMethod]
        public void TestEncryption()
        {
            ulong key = 0x0E329232EA6D0D73;

            ulong message = 0x8787878787878787; // Przykładowa wiadomość

            ulong encryptedMessage = DES_Algorithm.Encrypt(message, key);
            ulong expectedEncryptedMessage = 0x0000000000000000;

            Assert.AreEqual(encryptedMessage, expectedEncryptedMessage);
        }

        [TestMethod]
        public void TestDecryption()
        {
            ulong key = 0x133457799BBCDFF1;

            ulong message = 0x85E813540F0AB405; // Przykładowa wiadomość
            ulong decryptedMessage = DES_Algorithm.Decrypt(message, key);

            ulong expectedEncryptedMessage = 0x0123456789ABCDEF;

            Assert.AreEqual(decryptedMessage, expectedEncryptedMessage);
        }
    }
}
