using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class RSA
    {
        public BigInteger p;   // should be generated randomly
        public BigInteger q;   // should be generated randomly
        public BigInteger n;   // n = p*q
        public BigInteger e;   // public  key := generate with our values 
        public BigInteger d;   // private key := generate with our values 
        public BigInteger phi; // phi ( n )  = ( p - 1) * ( q - 1)

        public RSA()
        {
            this.p = 53; /* generateRandomNumber();*/ this.q = 59; /*generateRandomNumber();*/ this.n = p * q; this.phi = (p - 1) * (q - 1);
            this.e = generatePublicKey(this.phi); this.d = generatePrivateKey(this.phi);
        }
        public BigInteger generateRandomNumber()
        {
            //DateTime foo = DateTime.Now;
            //int unixTime = (int)((DateTimeOffset)foo).ToUnixTimeSeconds();

            Random rnd = new Random();
            byte[] randomBytes = new byte[4];
            rnd.NextBytes(randomBytes);

            int number = BitConverter.ToInt32(randomBytes, 0);

            return number;
        }
        // Greatest Commond Divisor 
        public static BigInteger gcd(BigInteger a, BigInteger b)
        {
            int temp;
            while (true)
            {
                temp = (int)(a % b);
                if (temp == 0)
                    return b;
                a = b;
                b = temp;
            }
        }
        // Generate our e value in range of [ 1 , phi(n) ]
        public BigInteger generatePublicKey(BigInteger phi)
        {
            int e = 2;
            while ( e < phi)
            {
                if (gcd(e, phi) == 1)
                    break;
                else
                    e++;
            }
            return e;
        }

        // Generate our d value such as it is  = ( phi(n) * k + 1 ) / e
        // In order to validate it we need to check if d*e mod( phi(n) ) == 1 
        // if both conditions are true we  have a valid key 

        public BigInteger generatePrivateKey(BigInteger phi)
        {

            int k = 1;
            BigInteger d = 0;

            while (true)
            {
                d = (1 + (k * phi)) / this.e;
                if ( d % 1 != 0)
                    k++;
                else if (d * this.e % this.phi != 1)
                    k++;
                else
                    break;
            }
            return d; 
        }
        public BigInteger Crypt(BigInteger message)
        {
            BigInteger encrypted ;
            encrypted = BigInteger.ModPow(message, this.e, this.n);
           

            return encrypted;
        }

        public BigInteger Decrypt(BigInteger encrypted)
        {
            BigInteger decrypted ;
            decrypted = BigInteger.ModPow(encrypted, this.d, this.n);
         
      

            return decrypted;
        }

        public string CryptFile(string inputFile , string outputFile)
        {
            string input = File.ReadAllText(inputFile);

            BigInteger numberInput = BigInteger.Parse(input);
            BigInteger encryptedInput = Crypt(numberInput);

            File.WriteAllText(outputFile, encryptedInput.ToString());

            return encryptedInput.ToString();
        }

        public string DecryptFile(string sourceFile , string destinationFile)
        {
            string encryptedText = File.ReadAllText(sourceFile);

            BigInteger numberInput = BigInteger.Parse(encryptedText);
            BigInteger decryptedNumbers = Decrypt(numberInput);

            File.WriteAllText(destinationFile, decryptedNumbers.ToString());

            return decryptedNumbers.ToString();

        }
      
    
// This code is contributed by Pranay Arora.
    }
}
