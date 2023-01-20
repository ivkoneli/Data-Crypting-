using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using System.ComponentModel;
using System.Security.Policy;
using System.Drawing.Imaging;

namespace Zastita_Informacija
{
    internal class A5_1
    {
        int[] key = null;

        int SIZEX = 19; int SIZEY = 22; int SIZEZ = 23;
        int regXCLK = 8; int regYCLK = 10; int regZCLK = 10;

        int[] regX = null; int[] regY = null; int[] regZ = null;


        // Konstruktor - inicijalizujemo vrednosti registra i stavljamo nas kljuc u njih
        public A5_1(int[] key)
        {
            regX = new int[SIZEX]; regY = new int[SIZEY]; regZ = new int[SIZEZ];

            this.PlaceKey(key);


            /* Console.Write("A5/1 - Kljuc     :");
             for (int i = 0; i < key.Length; i++)
             {
                 Console.Write(key[i]);
             }
             Console.WriteLine("");

             Console.Write("A5/1 - Registar X:");
             for (int i = 0; i < regX.Length; i++)
             {
                 Console.Write(regX[i]);
             }
             Console.WriteLine("");

             Console.Write("A5/1 - Registar Y:");
             for (int i = 0; i < regY.Length; i++)
             {
                 Console.Write(regY[i]);
             }
             Console.WriteLine("");

             Console.Write("A5/1 - Registar Z:");
             for (int i = 0; i < regZ.Length; i++)
             {
                 Console.Write(regZ[i]);
             }
             Console.WriteLine("");*/
        }

        // Majority funkcija koja proverava clock bitove sva tri registra da bi proverila koji trenutno radi 
        public int majority(int x, int y, int z)
        {
            if (x == y)
            {
                return x;
            }
            else if (y == z)
            {
                return y;
            }
            else if (x == z)
            {
                return z;
            }
            return 0;
        }
        public void PlaceKey(int[] key)
        {
            // Prazan kljuc 
            if (key == null)
            {
                return;
            }


            // Stavljamo 64-bitni kljuc u nase registre redom pocevsi od LFSR  X 
            int j = 0;
            int k = 0;
            for (int i = 0; i < key.Length; i++)
            {

                if (i < 19)
                {
                    regX[i] = key[i];
                }
                else if (i >= 19 && i < 41)
                {
                    regY[j++] = key[i];
                }
                else if (i >= 41 && i < 64)
                {
                    regZ[k++] = key[i];
                }
            }
        }


        // Shiftovanje prosledjenog registra
        public void shiftRegister(ref int[] register)
        {
            int swap = 0;

            if (register.Length == SIZEX)
            {
                swap = register[18] ^ register[17] ^ register[16] ^ register[13];

            }
            else if (register.Length == SIZEY)
            {
                swap = register[20] ^ register[21];
            }
            else if (register.Length == SIZEZ)
            {
                swap = register[22] ^ register[21] ^ register[20] ^ register[7];
            }

            for (int size = (register.Length - 1); size >= 0; size--)
            {
                if (register.Length == SIZEX)
                {
                    if (size == 0)
                    {
                        register[size] = swap;
                    }
                    else
                    {
                        register[size] = register[size - 1];
                    }
                }
                else if (register.Length == SIZEY)
                {
                    if (size == 0)
                    {
                        register[size] = swap;
                    }
                    else
                    {
                        register[size] = register[size - 1];
                    }
                }
                else if (register.Length == SIZEZ)
                {
                    if (size == 0)
                    {
                        register[size] = swap;
                    }
                    else
                    {
                        register[size] = register[size - 1];
                    }
                }

            }

        }

        // Vracamo poslednji element iz registra 
        public int Output(int[] register)
        {
            return register[register.Length - 1];
        }

        public int[] Crypt(int[] input , int[] key)
        {
            int[] result = new int[input.Length];
           

            PlaceKey(key);

            for (int i = 0; i < input.Length; i++)
            {
                int m = majority(this.regX[regXCLK], this.regY[regYCLK], this.regZ[regZCLK]);

                if (this.regX[regXCLK] == m)
                {
                    shiftRegister(ref this.regX);
                }
                else if (this.regY[regYCLK] == m)
                {
                    shiftRegister(ref this.regY);
                }
                else if (this.regZ[regZCLK] == m)
                {
                    shiftRegister(ref this.regZ);
                }
                result[i] = (Output(regX) ^ Output(regY) ^ Output(regZ))  ^ input[i];
            }

            return result;
        }

        public int[] Decrypt(int[] input, int[] key)
        {
            return this.Crypt(input,key);
        }

        public void EncryptFile(string inputFile, string outputFile, int[] key)
        {
            //BinaryReader br = new BinaryReader(new FileStream(inputFile, FileMode.Open));
            //BinaryWriter bw = new BinaryWriter(new FileStream(outputFile, FileMode.Create));

            /*int[] input = new int[8];
            int[] output = new int[8];

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (br.BaseStream.Position == br.BaseStream.Length)
                        break;
                    else
                        input[i] = br.ReadInt32();
                }

                Console.WriteLine("Procitano : ");
                for (int i = 0; i < 8; i++)
                    {
                        Console.Write(input[i]);
                    }
                Console.WriteLine("");
                output = this.Crypt(input);


                for (int i = 0; i < 8; i++)
                {
                    bw.Write(output[i]);
                }
            }*/

            int[] input = new int[8];
            BinaryReader br = new BinaryReader(new FileStream(inputFile, FileMode.Open));
            for (int i = 0; i < 8; i++)
            {
                input[i] = br.ReadInt32();
            }
            br.Close();

            /*Console.WriteLine("Text iz fajla : ");
            for (int i = 0; i < input.Length; i++)
            {
                Console.Write(input[i]);
            }
            Console.WriteLine("");*/

            int[] output = new int[8];
            output = this.Decrypt(input, key);

            /*Console.WriteLine("Cipher iz fajla : ");
            for (int i = 0; i < output.Length; i++)
            {
                Console.Write(output[i]);
            }
            Console.WriteLine("");*/


            BinaryWriter bw = new BinaryWriter(new FileStream(outputFile, FileMode.Create));
            for (int i = 0; i < 8; i++)
            {
                bw.Write(output[i]);
            }
            bw.Close();





        }

        public void DecryptFile(string inputFile, string outputFile, int[] key)
        {
            EncryptFile(inputFile, outputFile, key);
        }
        public void BMP(int[] key)
        {
            // Read all the bytes and store them into a byte array

            byte[] imageBytes = File.ReadAllBytes("BMPinput.bmp");

            // Convert the byte array to an integer array to send it 

            int[] intArray = new int[imageBytes.Length];

            for (int i = 0; i < imageBytes.Length; i++)
            {
                intArray[i] = (int)imageBytes[i];
            };



            // Crypt our array of integers using A5-1 Crypt and store them into an int[] array
            int[] encryptedArray = this.Crypt(intArray,key);


            // Convert the int array back to  byte array to store it in a file 
            byte[] encryptedBytes = new byte[encryptedArray.Length];

            for (int i = 0; i < encryptedArray.Length; i++)
            {
                encryptedBytes[i] = (byte)encryptedArray[i];
            }

            // Write the encrypted byte array to a new file 
            File.WriteAllBytes("BMPencrypted.bmp", encryptedBytes);



            // Decrypt our array of integers and store them into an int array 
            int[] decryptedArray = this.Decrypt(encryptedArray , key);

            // Convert that int array back to byte array in order to store it to a BMP file
            byte[] decryptedBytes = new byte[decryptedArray.Length];

            for (int i = 0; i < decryptedArray.Length; i++)
            {
                decryptedBytes[i] = (byte)decryptedArray[i];
            }

            // Write our newly decrypted bytes to an BMP file
            File.WriteAllBytes("BMPdecrypted.bmp", decryptedBytes);

        }

        public void BMP2(int[] key)
        {
            // Read all the bytes and store them into a byte array
            byte[] imageBytes = File.ReadAllBytes("BMPinput.bmp");

            // Get the first 50 bytes for the image header
            int headerSize = 34; 
            byte[] header = new byte[headerSize];
            Array.Copy(imageBytes, header, headerSize);

            // Get the rest of the bytes for the image data
            int dataSize = imageBytes.Length - headerSize;
            byte[] data = new byte[dataSize];
            Array.Copy(imageBytes, headerSize, data, 0, dataSize);

            // Split the image data into smaller portions
            // We need to split them into smaller chunks for our algorithm to work properly 
            int portionSize = 100; 
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
            List<int[]> encryptedPortions = new List<int[]>();
            foreach (byte[] portion in portions)
            {
                int[] intArray = new int[portion.Length];
                for (int i = 0; i < portion.Length; i++)
                {
                    intArray[i] = (int)portion[i];
                }
                this.PlaceKey(key);
                int[] encryptedPortion = this.Crypt(intArray,key);
                encryptedPortions.Add(encryptedPortion);
            }

            // Concatenate the encrypted portions
            List<int> encryptedInts = new List<int>();
            foreach (int[] encryptedPortion in encryptedPortions)
            {
                encryptedInts.AddRange(encryptedPortion);
            }
            int[] encryptedArray = encryptedInts.ToArray();

            // Convert the int array back to byte array
            byte[] encryptedBytes = new byte[encryptedArray.Length];
            for (int i = 0; i < encryptedArray.Length; i++)
            {
                encryptedBytes[i] = (byte)encryptedArray[i];

            }

            // Concatenate the header and the encrypted data
            byte[] encryptedImage = new byte[headerSize + encryptedBytes.Length];
            Array.Copy(header, 0, encryptedImage, 0, headerSize);
            Array.Copy(encryptedBytes, 0, encryptedImage, headerSize, encryptedBytes.Length);

            // Write the encrypted image to a new file
            File.WriteAllBytes("BMPencrypted.bmp", encryptedImage);

            // Read all the bytes and store them into a byte array
            byte[] EimageBytes = File.ReadAllBytes("BMPencrypted.bmp");

            int EheaderSize = 34;
            byte[] Eheader = new byte[EheaderSize];
            Array.Copy(EimageBytes, Eheader, EheaderSize);

            int DdataSize = EimageBytes.Length - EheaderSize;
            byte[] Ddata = new byte[DdataSize];
            Array.Copy(EimageBytes, EheaderSize, Ddata, 0, DdataSize);

            // Split the image data into smaller portions
            // We need to split them into smaller chunks for our algorithm to work properly 
            
            int DnumberOfPortions = (int)Math.Ceiling((double)Ddata.Length / portionSize);
            List<byte[]> Dportions = new List<byte[]>();
            for (int i = 0; i < DnumberOfPortions; i++)
            {
                int start = i * portionSize;
                int end = (i + 1) * portionSize;
                if (end > Ddata.Length) end = Ddata.Length;
                byte[] Dportion = new byte[end - start];
                Array.Copy(Ddata, start, Dportion, 0, end - start);
                Dportions.Add(Dportion);
            }

            // Decrypt each portion 
            List<int[]> decryptedPortions = new List<int[]>();
            foreach (byte[] Dportion in Dportions)
            {
                int[] intArray = new int[Dportion.Length];
                for (int i = 0; i < Dportion.Length; i++)
                {
                    intArray[i] = (int)Dportion[i];
                }
                this.PlaceKey(key);
                int[] decryptedPortion = this.Crypt(intArray,key);
                decryptedPortions.Add(decryptedPortion);
            }

            // Concatenate the encrypted portions
            List<int> decryptedInts = new List<int>();
            foreach (int[] decryptedPortion in decryptedPortions)
            {
                decryptedInts.AddRange(decryptedPortion);
            }
            int[] decryptedArray = decryptedInts.ToArray();

            // Convert the int array back to byte array
            byte[] decryptedBytes = new byte[decryptedArray.Length];
            for (int i = 0; i < decryptedArray.Length; i++)
            {
                decryptedBytes[i] = (byte)decryptedArray[i];

            }

            // Concatenate the header and the encrypted data
            byte[] decryptedImage = new byte[EheaderSize + decryptedBytes.Length];
            Array.Copy(Eheader, 0, decryptedImage, 0, EheaderSize);
            Array.Copy(decryptedBytes, 0, decryptedImage, EheaderSize, decryptedBytes.Length);

            // Write the encrypted image to a new file
            File.WriteAllBytes("BMPdecrypted.bmp", decryptedImage);
        }

        public int[] getImagePixels(Bitmap bmp)
        {
            // Storing our data 
            int[] data = new int[bmp.Width * bmp.Height * 24];


            // For each pixel at (x,y) get its R,G,B values and add them to our data array
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    
                    Color pixel = bmp.GetPixel(x, y);

                    
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;

                    // Converting bytes to int array 
                    // & 1 extracts the last bit from the byte and stores it into the array 
                    // each iteration stores another bit untill we get all 8
                    // shift right for litle edian
                    for (int i = 0; i < 8; i++)
                    {
                        data[y * bmp.Width * 24 + x * 24 + i] = (r >> (7 - i)) & 1; 
                        data[y * bmp.Width * 24 + x * 24 + 8 + i] = (g >> (7 - i)) & 1;
                        data[y * bmp.Width * 24 + x * 24 + 16 + i] = (b >> (7 - i)) & 1;
                    }
                }
            }
            return data;
        }

        public void setImagePixels(int[] data ,Bitmap bitmap , string file)
        {
            
            // For each pixel at x,y add its r,g,b values from our data array 
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    
                    byte r = 0;
                    byte g = 0;
                    byte b = 0;


                    // opposite of geting the value now we need to set the value so we shift to the right 
                    for (int i = 0; i < 8; i++)
                    {
                        r |= (byte)(data[y * bitmap.Width * 24 + x * 24 + i] << (7 - i));
                        g |= (byte)(data[y * bitmap.Width * 24 + x * 24 + 8 + i] << (7 - i));
                        b |= (byte)(data[y * bitmap.Width * 24 + x * 24 + 16 + i] << (7 - i));
                    }

                    // Set the value 
                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            // Save to file 
            bitmap.Save(file, ImageFormat.Bmp);
        }
        // Working 
        public void BMP3(string inputFile ,string encryptedFile,string decryptedFile, int[] key)
        {

            int[] data = new int[0];

            // Load the BMP image into a Bitmap object
            Bitmap bmp = (Bitmap)Image.FromFile(inputFile);
            data = this.getImagePixels(bmp);

    
            // Enkriptujemo trenutne podatke
            this.PlaceKey(key);
            data = this.Crypt(data ,key);

            Bitmap encrypted = new Bitmap(bmp.Width, bmp.Height);
            this.setImagePixels(data, encrypted , encryptedFile);


            // Enkriptujemo trenutne podatke
            data = Decrypt(data ,key);
            Bitmap decrypted = new Bitmap(bmp.Width, bmp.Height);
            this.setImagePixels(data, decrypted , decryptedFile);

           
        }
    }
}
