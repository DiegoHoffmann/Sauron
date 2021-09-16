using Accord.Video.FFMPEG;
using Sauron.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sauron
{
    public partial class Configuracao : Form
    {
        private string strConexao = ConfigurationManager.AppSettings["strConexao"].ToString();
        bool flagSucesso = false;
        public Configuracao()
        {
            InitializeComponent();
        }

        private void Configuracao_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PropriedadeCamera1.IP))
            {
                txtIP1.Text = PropriedadeCamera1.IP;
                txtSenha1.Text = PropriedadeCamera1.Senha;
                txtUsuario1.Text = PropriedadeCamera1.Usuario;
            }
            if (!string.IsNullOrEmpty(PropriedadeCamera2.IP))
            {
                txtIP2.Text = PropriedadeCamera2.IP;
                txtSenha2.Text = PropriedadeCamera2.Senha;
                txtUsuario2.Text = PropriedadeCamera2.Usuario;
            }
            if (!string.IsNullOrEmpty(PropriedadeCamera3.IP))
            {
                txtIP3.Text = PropriedadeCamera3.IP;
                txtSenha3.Text = PropriedadeCamera3.Senha;
                txtUsuario3.Text = PropriedadeCamera3.Usuario;
            }
            if (!string.IsNullOrEmpty(PropriedadeCamera4.IP))
            {
                txtIP4.Text = PropriedadeCamera4.IP;
                txtSenha4.Text = PropriedadeCamera4.Senha;
                txtUsuario4.Text = PropriedadeCamera4.Usuario;
            }
            carregarCamerasIP();
        }

        public void carregarCamerasIP()
        {
            var arpStream = ExecuteCommandLine("arp", "-a");
            List<Arp> lstArp = new List<Arp>();
            string ip = string.Empty;
            string mac = string.Empty;
            bool cabecalhoArp = true;
            while (!arpStream.EndOfStream)
            {
                Arp arp = new Arp();                     
                var linha = arpStream.ReadLine();
                if (cabecalhoArp)
                {
                    if (string.IsNullOrEmpty(linha))
                        linha = arpStream.ReadLine();
                    if (linha.Contains("Interface"))
                        linha = arpStream.ReadLine();
                    if (linha.Contains("Endere"))
                        linha = arpStream.ReadLine();
                    cabecalhoArp = false;
                }
                if (string.IsNullOrEmpty(linha))
                    continue;
                if(linha.Length >= 17)
                    ip = linha.Substring(2, 15);
                if(linha.Length >= 41)
                    mac = linha.Substring(24, 17);
                arp.IP = ip.Trim();
                arp.MacAddress = mac.Trim();
                lstArp.Add(arp);
            }
            lstArp.RemoveAll(x => (!x.MacAddress.Contains("24-fd-0d-66-57") && !x.MacAddress.Contains("24-fd-0d-b9-4c")));
            gridCameras.DataSource = lstArp;
            gridCameras.Refresh();
            gridCameras.RowsDefaultCellStyle.BackColor = Color.LightGray;
            gridCameras.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
        }
        public static StreamReader ExecuteCommandLine(String file, String arguments = "")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = file;
            startInfo.Arguments = arguments;
            startInfo.StandardOutputEncoding = Encoding.GetEncoding("iso-8859-1");

            Process process = Process.Start(startInfo);
            return process.StandardOutput;
        }

        private void btnVerificar1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (verificarConectividadeCamera(txtIP1.Text, txtUsuario1.Text, txtSenha1.Text, picStatus1))
            {
                PropriedadeCamera1.IP = txtIP1.Text;
                PropriedadeCamera1.Usuario = txtUsuario1.Text;
                PropriedadeCamera1.Senha = txtSenha1.Text;
            }
            Cursor.Current = Cursors.Default;


        }
        private void btnVerificar2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (verificarConectividadeCamera(txtIP2.Text, txtUsuario2.Text, txtSenha2.Text, picStatus2))
            {
                PropriedadeCamera2.IP = txtIP2.Text;
                PropriedadeCamera2.Usuario = txtUsuario2.Text;
                PropriedadeCamera2.Senha = txtSenha2.Text;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnVerificar3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (verificarConectividadeCamera(txtIP3.Text, txtUsuario3.Text, txtSenha3.Text, picStatus3))
            {
                PropriedadeCamera3.IP = txtIP3.Text;
                PropriedadeCamera3.Usuario = txtUsuario3.Text;
                PropriedadeCamera3.Senha = txtSenha3.Text;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnVerificar4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (verificarConectividadeCamera(txtIP4.Text, txtUsuario4.Text, txtSenha4.Text, picStatus4))
                {
                    PropriedadeCamera4.IP = txtIP4.Text;
                    PropriedadeCamera4.Usuario = txtUsuario4.Text;
                    PropriedadeCamera4.Senha = txtSenha4.Text;
                }
                Cursor.Current = Cursors.Default;
            }catch(Exception ex)
            {
                Console.WriteLine("Erro ao conectar a câmera: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public bool verificarConectividadeCamera(string ip, string usuario, string senha, System.Windows.Forms.PictureBox picStatus)
        {
            flagSucesso = false;
            string formataConexao = string.Format(strConexao, usuario, senha, ip);
            Thread t = new Thread(()=> processoConecta(formataConexao));
            t.Start();
            Thread.Sleep(6000);
            if (flagSucesso)
            {
                t.Abort();
                picStatus.Image = new Bitmap(string.Format("{0}{1}", buscaDiretorioImagem(), "ok.png"));
            }
            else
            {
                t.Abort();
                picStatus.Image = new Bitmap(string.Format("{0}{1}", buscaDiretorioImagem(), "error.png"));
            }
            return flagSucesso;
        }
        public void processoConecta(string con)
        {
            try
            {
                var video = new VideoFileReader();
                video.Open(string.Format(con));
                if (video.IsOpen)
                {
                    flagSucesso = true;
                    video.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar na câmera: " + ex.Message);
            }
        }
        private string buscaDiretorioImagem()
        {
            var diretorio = Directory.GetCurrentDirectory();
            while(!Directory.Exists(diretorio + "\\img") && diretorio.Contains("Sauron")){
                diretorio = diretorio.Substring(0, (diretorio.LastIndexOf("\\")));
            }
            return diretorio + "\\img\\";
        }      
    }
}
