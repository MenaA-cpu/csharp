using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace ClienteCHAT
{
    public partial class Form1 : Form
    {
        static private NetworkStream str;
        static private StreamWriter str2;
        static private StreamReader str3;
        static private TcpClient cliente = new TcpClient();
        static private string usuario = "unknown";
        private delegate void DAddItem(string s);

        private void AddItem(String s)
        {
            listBox1.Items.Add(s);
        }

        void Escucha()
        {
            while (cliente.Connected)
            {
                try
                {
                    this.Invoke(new DAddItem(AddItem), str3.ReadLine());
                }
                catch
                {
                    MessageBox.Show("No se estableción conexion");
                    Application.Exit();
                }
            }
        }

        void conexion()
        {
            try
            {
                cliente.Connect("127.0.0.1", 8000);
                if (cliente.Connected)
                {
                    Thread hilo = new Thread(Escucha);
                    str = cliente.GetStream();
                    str2 = new StreamWriter(str);
                    str3 = new StreamReader(str);

                    str2.WriteLine(usuario);
                    str2.Flush();

                    hilo.Start();
                }
                else
                {
                    MessageBox.Show("Servidor no encontrado");
                    Application.Exit();
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Servidor no encontrado");
                Application.Exit();
            }
        }
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            str2.WriteLine(textBox2.Text);
            str2.Flush();
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            usuario = textBox1.Text;
            conexion();
        }
    }
}
