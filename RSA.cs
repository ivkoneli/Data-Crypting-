using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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


        // Initializations 
        public RSA()
        {
            this.p = generateRandomNumber(); this.q = generateRandomNumber(); this.n = p * q; this.phi = (p - 1) * (q - 1);
            this.e = generatePublicKey(this.phi); this.d = generatePrivateKey(this.phi);
        }

        // Generating our p and q values 

        public BigInteger generateRandomNumber()
        {
            BigInteger number;
            do
            {
                // Cryptographic lib that helps give us secure random numbers 

                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomBytes = new byte[7];
                    rng.GetBytes(randomBytes);
                    number = new BigInteger(randomBytes);
                }
            } while (!isPrime(number));

            return number;
        }

        // Helper to determine if its a prime number and return it 
        private bool isPrime(BigInteger number)
        {
            if (number <= 1)
                return false;
            if (number == 2 || number == 3)
                return true;
            if (number % 2 == 0 || number % 3 == 0)
                return false;

            int limit = (int)Math.Sqrt((double)number);
            for (int i = 5; i <= limit; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }

            return true;
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

        // MAP a-z => 1-26 so we can take in string inputs 

        public BigInteger EncodeString(string input)
        {
            input = input.ToLower().Replace(" ",""); // convert the input string to lowercase
            BigInteger encodedNumber = new BigInteger();

            foreach (char c in input)
            {
                int charValue = (int)c - 96; // subtract 96 to convert the ASCII value to 1-26
                encodedNumber = encodedNumber * 27 + charValue; // multiply the previous number by 27 and add the current char value
            }

            return encodedNumber;
        }

        // Decode 1-26 back to original text after decrypting 
        public string DecodeString(BigInteger input)
        {
            string decodedString = "";
            while (input > 0)
            {
                int charValue = (int)(input % 27) + 96; // add 96 to convert the value to an ASCII value
                char c = (char)charValue; // convert the ASCII value to a character
                decodedString = c + decodedString; // prepend the current character to the decoded string
                input /= 27;
            }

            return decodedString;
        }

        // ModPow with private key 
        public BigInteger Crypt(string message)
        {


            BigInteger encrypted; 
            BigInteger input = EncodeString(message);

            encrypted = BigInteger.ModPow(input, this.e, this.n);
           

            return encrypted;
        }

        // ModPow with public key to reverse it 
        public string Decrypt(BigInteger encrypted)
        {
            BigInteger decrypted ;
            decrypted = BigInteger.ModPow(encrypted, this.d, this.n);

            string decryptedString = DecodeString(decrypted);

            return decryptedString;
        }

        // Read , crypt , write to new file and return text so we can print it 

        public string CryptFile(string inputFile , string outputFile)
        {
            string input = File.ReadAllText(inputFile);

            //BigInteger numberInput = BigInteger.Parse(input);
            BigInteger encryptedInput = Crypt(input);

            File.WriteAllText(outputFile, encryptedInput.ToString());

            return encryptedInput.ToString();
        }

        // Read , decrypt , write to new file and retrun the string to display it 

        public string DecryptFile(string sourceFile , string destinationFile)
        {
            string encryptedText = File.ReadAllText(sourceFile);

            BigInteger numberInput = BigInteger.Parse(encryptedText);
            string decryptedNumbers = Decrypt(numberInput);

            File.WriteAllText(destinationFile, decryptedNumbers.ToString());

            return decryptedNumbers.ToString();

        }

        // Slika se duplira u velicini u procesu enkripcije zbog modpow funckije 
        // Fix bi bio da napisem svoju modpow funkciju sa koja ne bi narusavala strukturu , ali BMP ucitavnje radi za a5-1 algoritam


        /*public void BMPEncrypt()
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

          
        }*/

        /*public void BMPDecrypt()
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

        }*/


    }
}
