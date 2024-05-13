using System;
using System.Diagnostics;
using DES_Algorithm_N;

public class Program
{
    public static void Main()
    {
        string inputFilePath = "C:\\Users\\aleks\\Documents\\STUDIA\\Programowanie Wsp�bie�ne\\NG_AK_repository\\Wsp�biegi_Projekt\\DES\\Test.pdf";
        string encryptedFilePath = "C:\\Users\\aleks\\Documents\\STUDIA\\Programowanie Wsp�bie�ne\\NG_AK_repository\\Wsp�biegi_Projekt\\DES\\Test_encrypted.pdf";
        string decryptedFilePath = "C:\\Users\\aleks\\Documents\\STUDIA\\Programowanie Wsp�bie�ne\\NG_AK_repository\\Wsp�biegi_Projekt\\DES\\Test_decrypted.pdf";
        ulong key = 0x133457799BBCDFF1; // Przyk�adowy klucz

        // Szyfrowanie pliku
        DES_Algorithm.EncryptFile(inputFilePath, encryptedFilePath, key);

        // Deszyfrowanie pliku
        DES_Algorithm.DecryptFile(encryptedFilePath, decryptedFilePath, key);

        // Szyfrowanie wiadomo�ci
        ulong message = 0x0123456789ABCDEF; // Przyk�adowa wiadomo��
        ulong encryptedMessage = DES_Algorithm.Encrypt(message, key);
        ulong decryptedMessage = DES_Algorithm.Decrypt(encryptedMessage, key);

        Console.WriteLine($"Original Message: {message:X}");
        Console.WriteLine($"Encrypted Message: {encryptedMessage:X}");
        Console.WriteLine($"Decrypted Message: {decryptedMessage:X}");
    }

}
