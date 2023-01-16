using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class CFB
    {
        playfair playfair;
        A5_1 cipher;
        int[] feedback;
        string iv;
        int blockSize;

        // Prima ključ, niz za inicijalizaciju i veličinu bloka
        public CFB(int[] key, int[] initvector, int blockSize)
        {
            cipher = new A5_1(key);
            feedback = initvector;
            this.blockSize = blockSize;
        }
        public CFB(string key , string initvector , int blockSize)
        {
            this.playfair = new playfair(key);
            this.iv = initvector;
            this.blockSize = blockSize;
        }


        // Funkcija za šifrovanje koja prima niz 0 i 1 i vraća niz šifrovanih 0 i 1
        public int[] Encrypt(int[] plaintext, int[] key)
        {
            int[] ciphertext = new int[plaintext.Length];
            cipher.PlaceKey(key);
            // Obradjujemo svaki blok
            for (int i = 0; i < plaintext.Length; i += blockSize)
            {
                // Šifrujemo feedback niz koristeći šifru A5/1
                this.feedback = this.cipher.Crypt(feedback);
                //Console.WriteLine(pom);
                Console.Write("Feedback : ");
                {
                    for (int l = 0; l < feedback.Length; l ++)
                    {
                        Console.Write(feedback[i]);
                    }
                }
                Console.WriteLine();

                

                // XOR-ujemo prvi feedback niz sa svakim elementom u trenutnom bloku
                // i smeštamo rezultat u odgovarajući element niza šifrovanih 0 i 1
                for (int j = 0; j < blockSize; j++)
                {
                    ciphertext[i + j] = feedback[j] ^ plaintext[i + j];
                    this.feedback[j] = ciphertext[i + j];
                }
                Console.Write("Feedback : ");
                {
                    for (int l = 0; l < feedback.Length; l++)
                    {
                        Console.Write(feedback[i]);
                    }
                }
                Console.WriteLine();
            }

            cipher.PlaceKey(key);
            return ciphertext;
        }

        // Funkcija za dešifrovanje koja prima niz šifrovanih 0 i 1 i vraća niz početnih 0 i 1
        public int[] Decrypt(int[] ciphertext, int[] key)
        {
            int[] plaintext = new int[ciphertext.Length];
            cipher.PlaceKey(key);

            // Obradjujemo svaki blok
            for (int i = 0; i < ciphertext.Length; i += blockSize)
            {
                // Šifrujemo feedback niz koristeći šifru A5/1
                feedback = cipher.Crypt(feedback);

                // XOR-ujemo prvi feedback niz sa svakim elementom u trenutnom bloku
                // i smeštamo rezultat u odgovarajući element niza dešifrovanih 0 i 1
                for (int j = 0; j < blockSize; j++)
                {
                    plaintext[i + j] = feedback[j] ^ ciphertext[i + j];
                    feedback[j] = ciphertext[i + j];
                }
            }

            cipher.PlaceKey(key);
            return plaintext;
        }
        private string XOR(string s1, string s2)
        {
            var result = new StringBuilder();

            for (int i = 0; i < s1.Length; i++)
            {
                var c = (char)((s1[i] - 'a') ^ (s2[i] - 'a') + 'a');
                if (c < 'a' || c > 'z')
                {
                    if (c == 'x')
                    {
                        // If the resulting character is an 'x', XOR it with 'a' instead of 'z'
                        c = (char)(c ^ 'a');
                    }
                    else
                    {
                        c = (char)(c - 'a' + 'z');
                    }
                }
                result.Append(c);
            }

            return result.ToString();
        }

        public string EncryptPlayFair(string plaintext)
        {
            var ciphertext = new StringBuilder();
            var previousCipherBlock = this.iv;

            for (int i = 0; i < plaintext.Length; i += this.blockSize)
            {
                var plainBlock = plaintext.Substring(i, this.blockSize);
                var cipherBlock = this.playfair.Crypt(previousCipherBlock);
                var xorResult = XOR(plainBlock, cipherBlock);

                ciphertext.Append(xorResult);
                previousCipherBlock = xorResult;
            }

            return ciphertext.ToString();
        }
        public string DecryptPlayFair(string ciphertext)
        {
            var plaintext = new StringBuilder();
            var previousCipherBlock = this.iv;

            for (int i = 0; i < ciphertext.Length; i += this.blockSize)
            {
                var cipherBlock = ciphertext.Substring(i, this.blockSize);
                var plainBlock = this.playfair.Crypt(previousCipherBlock);
                var xorResult = XOR(cipherBlock, plainBlock);

                plaintext.Append(xorResult);
                previousCipherBlock = cipherBlock;
            }

            return plaintext.ToString();
        }
    }
}
