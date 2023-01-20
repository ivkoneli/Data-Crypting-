using System;
using System.Windows.Forms;

namespace Zastita_Informacija
{
    public partial class AeroWizard1 : Form
    {
        public AeroWizard1()
        {
            
            InitializeComponent();
        }

        private void wizardControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";

            int[] key = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1,
                          1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0,
                          0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0,
                          0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 };
            richTextBox1.Text = "Encrypting BMP file...";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file, you can get the file path using
                string filePath = openFileDialog.FileName;
                richTextBox1.Text = "Encrypting BMP file...";
                Program.a5_1BMPInput(key, filePath);
                richTextBox1.Text = "File encrypted  succesfully outputs are in BMPencrypted.bmp and BMPdecrypted.bmp ";

                // perform action with the file path
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] inputString = textBox1.Text.Split(',');
            int[] input = Array.ConvertAll(inputString, int.Parse);

            int[] key = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1,
                          1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0,
                          0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0,
                          0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 };
            int[] encryptedText, decryptedText;

            Program.a5_1FileInput(key , input , out encryptedText , out decryptedText);
            richTextBox1.Text = "Encrypted Text :" + String.Join(",", encryptedText) + "\n" +
                                "Decrypted Text :" + String.Join(",", decryptedText);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] inputString = textBox2.Text.Split(',');
            int[] input = Array.ConvertAll(inputString, int.Parse);

            int[] key = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1,
                          1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0,
                          0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0,
                          0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 };
            int[] encryptedText, decryptedText;

            Program.CFB(input, out encryptedText, out decryptedText);

            richTextBox1.Text = "Encrypted Text :" + String.Join(",", encryptedText) + "\n" +
                                "Decrypted Text :" + String.Join(",", decryptedText);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string inputString = textBox3.Text;


            string encryptedText, decryptedText;

            Program.playfairInput(inputString, out encryptedText, out decryptedText);
            richTextBox2.Text = "Encrypted Text :" + encryptedText + "\n" + 
                                "Decrypted Text :" + decryptedText;
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

       

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file, you can get the file path using
                string filePath = openFileDialog.FileName;
                string plainText;
                Program.playfairFileInput(filePath , out plainText);
                richTextBox2.Text = "Plaintext : " + plainText + "\n" + "File encrypted  succesfully outputs are in playfairEncrypted.txt  and playfairOutput.txt ";

                // perform action with the file path
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string inputString = textBox4.Text;
            int input = int.Parse(inputString);

            int encryptedText, decryptedText;

            Program.RSAinput(input, out encryptedText, out decryptedText);
            richTextBox3.Text = "Encrypted Text :" + encryptedText + "\n" +
                                "Decrypted Text :" + decryptedText;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";



            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file, you can get the file path using
                string filePath = openFileDialog.FileName;
                string encryptedText , decryptedText;
                Program.RSAinputFILE(filePath, out encryptedText , out decryptedText);
                richTextBox3.Text = "Encrypted Text :" + encryptedText + "\n" +
                                    "Decrypted Text :" + decryptedText;

                // perform action with the file path
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";



            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file, you can get the file path using
                string filePath = openFileDialog.FileName;

                string hash;

                Program.SHA256Input(filePath, out hash);
                richTextBox4.Text = hash;
                // perform action with the file path
            }


        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

            string hash2 = richTextBox4.Text;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // The user selected a file, you can get the file path using
                string file1 = openFileDialog.FileName;

                string hash1;
                bool same = false;
                Program.SHA256Input(file1 , out hash1);

                if (hash1 == hash2)
                {
                    same = true;
                }

                richTextBox4.Text = "Hash value of the file is  :" + hash1 + "\n" +
                                    "Are the files same ? : " + same;
                                  

                // perform action with the file path
            }
        }
    }
}
