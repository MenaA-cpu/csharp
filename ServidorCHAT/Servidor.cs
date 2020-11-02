using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace ServidorCHAT
{
    class Servidor
    {
        private TcpClient cliente = new TcpClient();
        private TcpListener servidor;
        private IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, 8000);
        private List<Connection> list = new List<Connection>();

        Connection con;

        private struct Connection 
        {
            public NetworkStream str;
            public StreamWriter str2;
            public StreamReader str3;
            public string usuario;
        }

        public Servidor()
        {
            arranque();
        }

        public void arranque()
        {
            Console.WriteLine("Arranque de servidor listo");
            servidor = new TcpListener(ipendpoint);
            servidor.Start();

            while (true)
            {
                cliente = servidor.AcceptTcpClient();

                con = new Connection();
                con.str = cliente.GetStream();
                con.str3 = new StreamReader(con.str);
                con.str2 = new StreamWriter(con.str);

                con.usuario = con.str3.ReadLine();

                list.Add(con);
                Console.WriteLine(con.usuario + " Se ha conectado con exito");

                Thread hilo = new Thread(Espera_conexion);

                hilo.Start();
            }
        }

        void Espera_conexion() 
        {
            Connection xcon = con;

            do
            {
                try
                {
                    string tmp = xcon.str3.ReadLine();
                    Console.WriteLine(xcon.usuario + ": " + tmp);
                    foreach (Connection c in list)
                    {
                        try
                        {
                            c.str2.WriteLine(xcon.usuario + ": " + tmp);
                            c.str2.Flush();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {
                    list.Remove(xcon);
                    Console.WriteLine(con.usuario + " Salio");
                    break;
                }
            } while (true);
        }
    }
}
