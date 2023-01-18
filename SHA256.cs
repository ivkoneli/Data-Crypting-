using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Zastita_Informacija
{
    internal class SHA256
    {

        // Initial values of our working  constants a,b,c,d,e,f,g,h
        // they start with these values and are later on filled with our results 
        // Should not be changed 
        private readonly uint[] _h = new uint[8]
        {
        0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
        0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
        };

        // Array of 64 uint's that represent the fractional part of the square roots of the first 64 prime numbers 
        // Should not be modifed as they are standard to SHA256 algorithm structure 
        // They serve to add more randomness and make it harder to decrypt 
        private readonly uint[] _k = new uint[64]
        {
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
        0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
        0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
        0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        // Firstly we pad our data so it can be divided into chunks of 64bytes
        // They we divide it into chunks of 64bytes for and we create our message array 
        // First for loop fills the first 16 elements of our message array with values of the data converted to uint w[0-16] is now done
        // The second loop filles the remaining 48 elements of our message array starting from w[16] 
        // Each element is caluculated like this w[index] = w[index - 16] + sigma0 + w[index - 7] + s1 ; We increment the index and thats how we fill the remainder of our array
        // sigma0 is caluculated like this = ( w[index-15] shiftright 7 (25 if its opposite bcz its a rotational shift) xor w[index-15] shiftright 18 ( 14 opposite )  xor  w[index-15] shiftright 3 
        // sigma1 is caluculated like this = ( w[index-2] shiftright 17 (15) xor w[index-2] shiftright 19 xor w[index-2] shiftright 10 
        // since index starts at 16 both functions look like this on the first iteration 
        //  sigma0 = w1  >> 7  | w1  >> 18 | w1  >> 3
        //  sigma1 = w14 >> 17 | w14 >> 19 | w14 >> 10
        // This is how we get w[16+index] until w[63]  


        public byte[] ComputeHash(byte[] data)
        {
            var paddedData = PadData(data);
            var chunks = SplitToChunks(paddedData);

            for (int i = 0; i < chunks.Length; i++)
            {
                var w = new uint[64];
                var chunk = chunks[i];

                for (int j = 0; j < 16; j++)
                {
                    w[j] = BitConverter.ToUInt32(chunk, j * 4);
                }

                for (int j = 16; j < 64; j++)
                {
                    var sigma0 = (w[j - 15] >> 7 | w[j - 15] << 25) ^ (w[j - 15] >> 18 | w[j - 15] << 14) ^ (w[j - 15] >> 3);
                    var sigma1 = (w[j - 2] >> 17 | w[j - 2] << 15) ^ (w[j - 2] >> 19 | w[j - 2] << 13) ^ (w[j - 2] >> 10);
                    w[j] = w[j - 16] + sigma0 + w[j - 7] + sigma1;
                }

                // After we have our message array filled for all 64 bytes we then initialize our 8 32uint variables with our initialization numbers that we defined in _h
                var a = _h[0];
                var b = _h[1];
                var c = _h[2];
                var d = _h[3];
                var e = _h[4];
                var f = _h[5];
                var g = _h[6];
                var h = _h[7];

                
                // The calculation of these values is predefined in the SHA256 Algorithm like this
                // sum 1 or epsilon1 = e >> 6 xor e >> 11 xor e >> 25 
                // sum 0 or epsilon0 = a >> 2 xor a >> 13 xor a >> 22
                // choice or ch = ( e and f ) xor ( !e and g) 
                // majority or maj = ( a and b ) xor ( a and c ) xor ( b and c)

                // temp1 = h + sum1 + choice + k[value of index] + w[index]
                // temp2 = sum0 + majority 

                for (int j = 0; j < 64; j++)
                {
                    var epsilon1 = (e >> 6 | e << 26) ^ (e >> 11 | e << 21) ^ (e >> 25 | e << 7);
                    var ch = (e & f) ^ (~e & g);
                    var temp1 = h + epsilon1 + ch + _k[j] + w[j];
                    var epsilon0 = (a >> 2 | a << 30) ^ (a >> 13 | a << 19) ^ (a >> 22 | a << 10);
                    var maj = (a & b) ^ (a & c) ^ (b & c);
                    var temp2 = epsilon0 + maj;


                    // After initialization each variable of our 8 variables is updated using this system 


                    h = g;
                    g = f;
                    f = e;
                    e = d + temp1;
                    d = c;
                    c = b;
                    b = a;
                    a = temp1 + temp2;
                }
                // After updating the values we add them to our starting array 

                _h[0] += a;
                _h[1] += b;
                _h[2] += c;
                _h[3] += d;
                _h[4] += e;
                _h[5] += f;
                _h[6] += g;
                _h[7] += h;
            }

            // After completing the for loop for each of our data chunks we take the values of h array add them together into a new array

            var result = new byte[_h.Length * 4];
            for (int i = 0; i < _h.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(_h[i]), 0, result, i * 4, 4);
            }

            return result;
        }

        // Since SHA256 works with only mutiples od 512bits (64bytes) and our data might not be that size we need to add padding
        // Firstly we create an array large enough to store the data 
        // It needs to have room for the whole data + 8bytes that represent the size of our data which we append to our new array
        // Deviding it with /64 gives us the number of chunks need to store the data + padding  + 1 is added as a safety meassure 
        // After that we copy data into our array and at the end of our data we add a single byte with 100000000 the represents the end of our original data
        // Then we append the lenght of our original data to the end
        private byte[] PadData(byte[] data)
        {
            var paddedData = new byte[((data.Length + 8) / 64 + 1) * 64];
            Array.Copy(data, paddedData, data.Length);
            paddedData[data.Length] = 0x80;
            Array.Copy(BitConverter.GetBytes((long)data.Length * 8), 0, paddedData, paddedData.Length - 8, 8);
            return paddedData;
        }

        // Spliting our new padded array into chunks of 512bits so that SHA256 algorithm can work properly 
        private byte[][] SplitToChunks(byte[] paddedData)
        {
            var chunks = new byte[paddedData.Length / 64][];
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i] = new byte[64];
                Array.Copy(paddedData, i * 64, chunks[i], 0, 64);
            }
            return chunks;
        }

        // Gets our input casts into bytes and then calls ComputeHash for the whole data 
        // But the data will be split into chunks latter on 
        // After getting the result we append it all to a string and return it to be displayed
        public string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = ComputeHash(bytes);
            var result = new StringBuilder();
            foreach (var b in hash)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        // Read input from text and call ComputeHash and then display the hash function value so 
        // we can compare two files 
        public string ComputeFileHash(string filepath)
        {
            var filedata = File.ReadAllText(filepath);
            string hash = ComputeHash(filedata);

            return hash;
        }
    
    }
}
