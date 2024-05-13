using DES_File_MANAGER;
using DES_Round;
using DESKeyGenerator;
using System;
using System.IO;
using System.Linq;

namespace DES_Algorithm_N
{
    public class DES_Algorithm
    {
        public static ulong Encrypt(ulong plaintext, ulong key)
        {
            try
            {
                KeyGeneratorData keyGenerator = new KeyGeneratorData(key);
                keyGenerator.GenerateSubKesys();

                byte[] initialData = BitConverter.GetBytes(plaintext).Reverse().ToArray();
                byte[] initialPermutedData = FileProcessor.ApplyInitialPermutation(initialData);
                (uint L0, uint R0) = FileProcessor.SplitBlockIntoL0R0(initialPermutedData);

                (uint L16, uint R16) = Round_Class.PerformFullDesRounds(L0, R0, KeyGeneratorData.Keys_table);

                ulong combinedLR = Round_Class.CombineToUlong(R16, L16);
                ulong ciphertext = Round_Class.ApplyFinalPermutation(L16, R16);

                return ciphertext;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encryption failed: " + ex.Message);
                throw;
            }
        }

        public static ulong Decrypt(ulong ciphertext, ulong key)
        {
            try
            {
                KeyGeneratorData keyGenerator = new KeyGeneratorData(key);
                keyGenerator.GenerateSubKesys();
                Array.Reverse(KeyGeneratorData.Keys_table);

                byte[] initialData = BitConverter.GetBytes(ciphertext).Reverse().ToArray();
                byte[] initialPermutedData = FileProcessor.ApplyInitialPermutation(initialData);
                (uint L0, uint R0) = FileProcessor.SplitBlockIntoL0R0(initialPermutedData);

                (uint L16, uint R16) = Round_Class.PerformFullDesRounds(L0, R0, KeyGeneratorData.Keys_table);

                ulong combinedLR = Round_Class.CombineToUlong(R16, L16);
                ulong decryptedText = Round_Class.ApplyFinalPermutation(L16, R16);

                return decryptedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption failed: " + ex.Message);
                throw;
            }
        }

        public static void EncryptFile(string inputFilePath, string outputFilePath, ulong key)
        {
            try
            {
                KeyGeneratorData keyGenerator = new KeyGeneratorData(key);
                keyGenerator.GenerateSubKesys();

                byte[][] blocks = FileProcessor.ProcessInput(inputFilePath, true);

                using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create))
                {
                    foreach (byte[] block in blocks)
                    {
                        ulong blockData = BitConverter.ToUInt64(block.Reverse().ToArray(), 0);
                        ulong encryptedBlock = Encrypt(blockData, keyGenerator.Key);
                        byte[] encryptedBytes = BitConverter.GetBytes(encryptedBlock).Reverse().ToArray();

                        fileStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("File encryption failed: " + ex.Message);
                throw;
            }
        }

        public static void DecryptFile(string inputFilePath, string outputFilePath, ulong key)
        {
            // Utwórz instancję generatora kluczy i wygeneruj subklucze
            KeyGeneratorData keyGenerator = new KeyGeneratorData(key);
            keyGenerator.GenerateSubKesys();
            Array.Reverse(KeyGeneratorData.Keys_table); // Klucze subrund użyte w odwrotnej kolejności dla deszyfrowania

            // Przetwórz zaszyfrowany plik i podziel go na bloki
            byte[][] blocks = FileProcessor.ProcessInput(inputFilePath, true);

            // Deszyfruj każdy blok
            using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create))
            {
                foreach (byte[] block in blocks)
                {
                    // Konwersja bloku bajtów na ulong, odszyfrowanie, konwersja z powrotem na blok bajtów
                    ulong blockData = BitConverter.ToUInt64(block.Reverse().ToArray(), 0);
                    ulong decryptedBlock = Decrypt(blockData, keyGenerator.Key);
                    byte[] decryptedBytes = BitConverter.GetBytes(decryptedBlock).Reverse().ToArray();

                    // Usuń padding z ostatniego bloku danych
                    if (block == blocks.Last())
                    {
                        decryptedBytes = FileProcessor.RemovePadding(decryptedBytes, 8);
                    }

                    // Zapisz odszyfrowany blok do pliku wyjściowego
                    fileStream.Write(decryptedBytes, 0, decryptedBytes.Length);
                }
            }
        }


        public static string EncryptString(string plaintext, ulong key)
        {
            // Konwertuj ciąg znaków na tablicę bajtów
            byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

            // Szyfruj każdy 64-bitowy blok
            List<byte> ciphertextBytes = new List<byte>();
            for (int i = 0; i < plaintextBytes.Length; i += 8)
            {
                // Utwórz 64-bitowy blok
                byte[] block = new byte[8];
                Array.Copy(plaintextBytes, i, block, 0, Math.Min(8, plaintextBytes.Length - i));

                // Szyfruj blok
                ulong blockData = BitConverter.ToUInt64(block, 0);
                ulong encryptedBlock = Encrypt(blockData, key);

                // Dodaj zaszyfrowany blok do listy bajtów wynikowych
                ciphertextBytes.AddRange(BitConverter.GetBytes(encryptedBlock));
            }

            // Konwertuj tablicę bajtów z powrotem na ciąg znaków
            return Convert.ToBase64String(ciphertextBytes.ToArray());
        }

        public static string DecryptString(string ciphertext, ulong key)
        {
            // Konwertuj ciąg znaków na tablicę bajtów
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            // Deszyfruj każdy 64-bitowy blok
            List<byte> plaintextBytes = new List<byte>();
            for (int i = 0; i < ciphertextBytes.Length; i += 8)
            {
                // Utwórz 64-bitowy blok
                byte[] block = new byte[8];
                Array.Copy(ciphertextBytes, i, block, 0, Math.Min(8, ciphertextBytes.Length - i));

                // Deszyfruj blok
                ulong blockData = BitConverter.ToUInt64(block, 0);
                ulong decryptedBlock = Decrypt(blockData, key);

                // Dodaj odszyfrowany blok do listy bajtów wynikowych
                plaintextBytes.AddRange(BitConverter.GetBytes(decryptedBlock));
            }

            // Konwertuj tablicę bajtów z powrotem na ciąg znaków
            return System.Text.Encoding.UTF8.GetString(plaintextBytes.ToArray());
        }




    }
}
