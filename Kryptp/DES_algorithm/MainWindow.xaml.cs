using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DES_Algorithm_N;


using System;
using System.IO;
using Microsoft.Win32;


namespace DES_algorithm
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void EncryptFile(object sender, RoutedEventArgs e)
        {
            var inputFilePath = filePathTextBox.Text;
            var outputFilePath = System.IO.Path.ChangeExtension(inputFilePath, ".encrypted");

            // Sprawdzenie, czy pole keyTextBox nie jest puste oraz czy wprowadzona wartość klucza jest poprawna
            if (!string.IsNullOrEmpty(keyTextBox.Text) && ulong.TryParse(keyTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out ulong key))
            {
                // Jeśli warunki są spełnione, przekazujemy klucz do funkcji szyfrowania
                DES_Algorithm.EncryptFile(inputFilePath, outputFilePath, key);
                MessageBox.Show("Encryption completed!");
            }
            else
            {
                MessageBox.Show("Please enter a valid hexadecimal key.");
            }
        }

        private void DecryptFile(object sender, RoutedEventArgs e)
        {
            string inputFilePath = filePathTextBox.Text;
            string outputFilePath = System.IO.Path.ChangeExtension(inputFilePath, ".decrypted");
            ulong key = Convert.ToUInt64(keyTextBox.Text, 16); // Konwertuj klucz z szesnastkowego na ulong

            DES_Algorithm.DecryptFile(inputFilePath, outputFilePath, key);

            MessageBox.Show("Decryption completed!");
        }

        private void EncryptMessage_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz wiadomość do zaszyfrowania z pola tekstowego
            string plaintext = inputMessageTextBox.Text;

            // Pobierz klucz z pola tekstowego i przekształć go na ulong
            if (!ulong.TryParse(keyTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out ulong key))
            {
                // Jeśli klucz nie jest prawidłowym ulong, wyświetl komunikat o błędzie
                MessageBox.Show("Klucz musi być liczbą!");
                return;
            }

            // Szyfruj wiadomość
            string ciphertext = DES_Algorithm.EncryptString(plaintext, key);

            // Wyświetl zaszyfrowaną wiadomość w polu tekstowym wyniku
            outputMessageTextBox.Text = ciphertext;
        }

        private void DecryptMessage_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz wiadomość do odszyfrowania z pola tekstowego
            string ciphertext = inputMessageTextBox.Text;

            // Pobierz klucz z pola tekstowego i przekształć go na ulong
            if (!ulong.TryParse(keyTextBox.Text, System.Globalization.NumberStyles.HexNumber, null, out ulong key))
            {
                // Jeśli klucz nie jest prawidłowym ulong, wyświetl komunikat o błędzie
                MessageBox.Show("Klucz musi być liczbą!");
                return;
            }

            // Odszyfruj wiadomość
            string plaintext = DES_Algorithm.DecryptString(ciphertext, key);

            // Wyświetl odszyfrowaną wiadomość w polu tekstowym wyniku
            outputMessageTextBox.Text = plaintext;
        }



    }
}



