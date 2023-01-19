using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class CFB
    {
        A5_1 cipher;
        int[] feedback; 
        int blockSize;

        // Takes in our key init vector and our block size
        public CFB(int[] key, int[] initvector, int blockSize)
        {
            cipher = new A5_1(key);
            feedback = initvector;
            this.blockSize = blockSize;
        }    
        public int[] Encrypt(int[] inputArray, int[] key)
        {
            int[] ciphertext = new int[inputArray.Length];

            for (int i = 0; i < inputArray.Length / blockSize; i ++)
            {
               
                this.feedback = this.cipher.Crypt(feedback,key);           

                for (int j = 0; j < blockSize; j++)
                {
                    ciphertext[i * blockSize + j] = feedback[j] ^ inputArray[i * blockSize + j];
                    this.feedback[j] = ciphertext[i * blockSize + j];
                }

            }
            return ciphertext;
        }

        // Decrypt function
        public int[] Decrypt(int[] ciphertext, int[] key , int[] iv)
        {
            int[] plaintext = new int[ciphertext.Length];
            this.feedback = iv;

            // Iterate for each block
            for (int i = 0; i < ciphertext.Length / blockSize; i ++)
            {
                // Crypt the feedback
                
                this.feedback = this.cipher.Decrypt(feedback,key);


                for (int j = 0; j < blockSize; j++)
                {
                    plaintext[i * blockSize + j] = feedback[j] ^ ciphertext[i * blockSize + j];
                    this.feedback[j] = plaintext[i * blockSize + j];
                }
            }
            return plaintext;
        }
    }
}
