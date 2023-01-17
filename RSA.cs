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
        public void BMPEncrypt()
        {
            // Read all the bytes and store them into a byte array
            byte[] imageBytes = File.ReadAllBytes("BMPinput.bmp");

            // Get the first 50 bytes for the image header
            int headerSize = 50;
            byte[] header = new byte[headerSize];
            Array.Copy(imageBytes, header, headerSize);

            // Get the rest of the bytes for the image data
            BigInteger dataSize = imageBytes.Length - headerSize;
            byte[] data = new byte[(int)dataSize];
            Array.Copy(imageBytes, headerSize, data, 0, (int)dataSize);

            // Split the image data into smaller portions
            // We need to split them into smaller chunks for our algorithm to work properly 
            int portionSize = 1000;
            int numberOfPortions = (int)Math.Ceiling((double)data.Length / portionSize);
            List<byte[]> portions = new List<byte[]>();
            for (int i = 0; i < numberOfPortions; i++)
            {
                int start = i * portionSize;
                int end = (i + 1) * portionSize;
                if (end > data.Length) end = data.Length;
                byte[] portion = new byte[end - start];
                Array.Copy(data, start, portion, 0, end - start);
                portions.Add(portion);
            }

            // Encrypt each portion 
            List<BigInteger> encryptedPortions = new List<BigInteger>();
            foreach (byte[] portion in portions)
            {
                BigInteger[] bigIntArray = portion.Select(x => (BigInteger)x).ToArray();
                foreach (BigInteger bigInteger in bigIntArray)
                {
                    BigInteger encryptedPortion = this.Crypt(bigInteger);
                    encryptedPortions.Add(encryptedPortion);
                }

            }

            // Concatenate the encrypted portions
            List<byte> encryptedBytes = new List<byte>();
            foreach (BigInteger encryptedPortion in encryptedPortions)
            {
                byte[] bytes = encryptedPortion.ToByteArray();
                Array.Reverse(bytes);
                encryptedBytes.AddRange(bytes);
            }
            byte[] encryptedArray = encryptedBytes.ToArray();

            // Concatenate the header and the encrypted data
            byte[] encryptedImage = new byte[headerSize + encryptedArray.Length];
            Array.Copy(header, 0, encryptedImage, 0, headerSize);
            Array.Copy(encryptedArray, 0, encryptedImage, headerSize, encryptedArray.Length);

            // Write the encrypted image to a new file
            File.WriteAllBytes("BMPencrypted.bmp", encryptedImage);

          
        }

        public void BMPDecrypt()
        {
            // Read the encrypted image from the file
            byte[] encryptedBytes = File.ReadAllBytes("BMPencrypted.bmp");

            // Get the first 50 bytes for the image header
            int headerSize = 50;
            byte[] header = new byte[headerSize];
            Array.Copy(encryptedBytes, header, headerSize);

            // Get the rest of the bytes for the image data
            BigInteger dataSize = encryptedBytes.Length - headerSize;
            byte[] data = new byte[(int)dataSize];
            Array.Copy(encryptedBytes, headerSize, data, 0, (int)dataSize);

            // Split the image data into smaller portions
            // We need to split them into smaller chunks for our algorithm to work properly 
            int portionSize = 1000;
            int numberOfPortions = (int)Math.Ceiling((double)data.Length / portionSize);
            List<byte[]> portions = new List<byte[]>();
            for (int i = 0; i < numberOfPortions; i++)
            {
                int start = i * portionSize;
                int end = (i + 1) * portionSize;
                if (end > data.Length) end = data.Length;
                byte[] portion = new byte[end - start];
                Array.Copy(data, start, portion, 0, end - start);
                portions.Add(portion);
            }

            // Decrypt each portion 
            List<BigInteger> decryptedPortions = new List<BigInteger>();
            foreach (byte[] portion in portions)
            {
                BigInteger[] bigIntArray = portion.Select(x => (BigInteger)x).ToArray();
                foreach (BigInteger bigInteger in bigIntArray)
                {
                    BigInteger decryptedPortion = this.Decrypt(bigInteger);
                    decryptedPortions.Add(decryptedPortion);
                }
            }

            // Concatenate the decrypted portions
            List<byte> decryptedBytes = new List<byte>();
            foreach (BigInteger decryptedPortion in decryptedPortions)
            {
                byte[] bytes = decryptedPortion.ToByteArray();
                Array.Reverse(bytes);
                decryptedBytes.AddRange(bytes);
            }
            byte[] decryptedArray = decryptedBytes.ToArray();

            // Concatenate the header and the decrypted data
            byte[] decryptedImage = new byte[headerSize + decryptedArray.Length];
            Array.Copy(header, 0, decryptedImage, 0, headerSize);
            Array.Copy(decryptedArray, 0, decryptedImage, headerSize, decryptedArray.Length);

            // Write the decrypted image to a new file
            File.WriteAllBytes("BMPdecrypted.bmp", decryptedImage);

        }


    }
}
