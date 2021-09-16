using Accord.Controls;
using Accord.Imaging;
using Accord.Video.FFMPEG;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using ReconhecimentoFacial;
using ReconhecimentoFacial.Models;
using Sauron.Helpers;
using Sauron.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sauron
{
    public partial class Captura : Form
    {
        //Para iniciar teste sem configurar no sistema
        //private string camera1 = ConfigurationManager.AppSettings["camera1"].ToString();
        //private string camera2 = ConfigurationManager.AppSettings["camera2"].ToString();
        //private string camera3 = ConfigurationManager.AppSettings["camera3"].ToString();
        //private string camera4 = ConfigurationManager.AppSettings["camera4"].ToString();

        private string camera1 = string.Empty;
        private string camera2 = string.Empty;
        private string camera3 = string.Empty;
        private string camera4 = string.Empty;

        private string strConexao = ConfigurationManager.AppSettings["strConexao"].ToString();
        private string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
        private string SUBSCRIPTION_KEY = ConfigurationManager.AppSettings["apiKey"].ToString();
        static int tentativas = 0;
        private bool flagConectado = false;
        public readonly CascadeClassifier cascadeClassifier = new CascadeClassifier(@"XML\\haarcascade_frontalface_alt_tree.xml");
        private Task _workTask = Task.CompletedTask;
        private bool executandoBusca = false;
        static bool waitLstView;
        
        static List<PessoaDTO> lstView;
        static bool flagFace1 = false;
        static bool flagFace2 = false;
        static bool flagFace3 = false;
        static bool flagFace4 = false;

        Thread thr1;
        Thread thrFace1;
        Thread thrLista1;
        static List<Rectangle> lstRect1;
        static List<Pessoa> lstPessoa1;

        Thread thr2;
        Thread thrFace2;
        Thread thrLista2;
        static List<Rectangle> lstRect2;
        static List<Pessoa> lstPessoa2;

        Thread thr3;
        Thread thrFace3;
        Thread thrLista3;
        static List<Rectangle> lstRect3;
        static List<Pessoa> lstPessoa3;

        Thread thr4;
        Thread thrFace4;
        Thread thrLista4;
        static List<Rectangle> lstRect4;
        static List<Pessoa> lstPessoa4;

        private SolidBrush drawBrush;
        private Pen redPen2;
        private Pen greenPen2;
        private Font fonteProcurado;
        public Captura()
        {
            lstRect1 = new List<Rectangle>();
            lstPessoa1 = new List<Pessoa>();
            lstRect2 = new List<Rectangle>();
            lstPessoa2 = new List<Pessoa>();
            lstRect3 = new List<Rectangle>();
            lstPessoa3 = new List<Pessoa>();
            lstRect4 = new List<Rectangle>();
            lstPessoa4 = new List<Pessoa>();
            lstView = new List<PessoaDTO>();

            InitializeComponent();
            AutenticacaoClient.client = Autenticar.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            Grupo.IdGrupo = ConfigurationManager.AppSettings["idGuidGrupo"].ToString();
            drawBrush = new SolidBrush(Color.Red);
            redPen2 = new Pen(Color.Red, 3);
            greenPen2 = new Pen(Color.Green, 2);
            fonteProcurado = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
            waitLstView = false;
            IniciarCameras();
        }
        public void IniciarCameras()
        {
            if (!string.IsNullOrEmpty(camera1))
            {
                IniciarCapturaImagem(new VideoFileSource(camera1), player1);
                Thread.Sleep(3000);
                flagConectado = true;
            }
            if (!string.IsNullOrEmpty(camera2))
            {
                IniciarCapturaImagem(new VideoFileSource(camera2), player2);
                Thread.Sleep(3000);
                flagConectado = true;
            }
            if (!string.IsNullOrEmpty(camera3))
            {
                IniciarCapturaImagem(new VideoFileSource(camera3), player3);
                Thread.Sleep(3000);
                flagConectado = true;
            }
            if (!string.IsNullOrEmpty(camera4))
            {
                IniciarCapturaImagem(new VideoFileSource(camera4), player4);
                Thread.Sleep(3000);
                flagConectado = true;
            }
            if (flagConectado)
            {
                btnConectar.Text = "Desconectar Câmeras";
            }
        }
        public void IniciarCapturaImagem(Accord.Video.IVideoSource source, VideoSourcePlayer playerControl)
        {
            FinalizarCaptura(playerControl);
            playerControl.VideoSource = source;
            playerControl.Start();

        }

        private void FinalizarTodasCapturas()
        {
            FinalizarCaptura(player1);
            FinalizarCaptura(player2);
            FinalizarCaptura(player3);
            FinalizarCaptura(player4);
            if (executandoBusca)
            {
                if (thr1 != null)
                    thr1.Abort();
                if (thr2 != null)
                    thr2.Abort();
                if (thrFace4 != null)
                    thrFace4.Abort();
                if (thrLista4 != null)
                    thrLista4.Abort();
                if (thrLista1 != null)
                    thrLista1.Abort();
                if (thrLista2 != null)
                    thrLista2.Abort();
                if (thrLista3 != null)
                    thrLista3.Abort();
                if (thrLista4 != null)
                    thrLista4.Abort();
                executandoBusca = false;
                flagFace1 = false;
                flagFace2 = false;
                flagFace3 = false;
                flagFace4 = false;
            }
        }
        private void FinalizarCaptura(VideoSourcePlayer playerControl)
        {
            if (playerControl.VideoSource != null)
            {
                playerControl.SignalToStop();
                playerControl.WaitForStop();
                playerControl.VideoSource = null;

            }
        }

        private void videoSourcePlayer_Click(object sender, EventArgs e)
        {
            FinalizarTodasCapturas();
            CameraEspecifica cameraEspecifica = new CameraEspecifica(camera1, this);
            flagConectado = false;
            cameraEspecifica.Show();
        }

        private void player2_Click(object sender, EventArgs e)
        {
            FinalizarTodasCapturas();
            CameraEspecifica cameraEspecifica = new CameraEspecifica(camera2, this);
            flagConectado = false;
            cameraEspecifica.Show();
        }
        private void player3_Click(object sender, EventArgs e)
        {
            FinalizarTodasCapturas();
            CameraEspecifica cameraEspecifica = new CameraEspecifica(camera3, this);
            flagConectado = false;
            cameraEspecifica.Show();
        }

        private void player4_Click(object sender, EventArgs e)
        {
            FinalizarTodasCapturas();
            CameraEspecifica cameraEspecifica = new CameraEspecifica(camera4, this);
            flagConectado = false;
            cameraEspecifica.Show();
        }

        private async void btnBuscarPessoa_Click(object sender, EventArgs e)
        {
            if (!executandoBusca)
            {
                executandoBusca = true;
                btnBuscarPessoa.Text = "Parar Busca";
                lstPessoa1.Clear();
                lstPessoa2.Clear();
                lstPessoa3.Clear();
                lstPessoa4.Clear();
                try
                {

                    thr1 = new Thread(ReconhecimentoFace);
                    thr1.Start();
                    thr2 = new Thread(ReconhecimentoFace2);
                    thr2.Start();

                    if (!string.IsNullOrEmpty(PropriedadeCamera1.IP))
                    {
                        if (cbHistorico.Checked)
                        {
                            thrLista1 = new Thread(ThreadListaFaces);
                            thrLista1.Start();
                        }
                        player1.Paint += new System.Windows.Forms.PaintEventHandler(this.player1_Paint);
                    }
                    if (!string.IsNullOrEmpty(PropriedadeCamera2.IP))
                    {
                        if (cbHistorico.Checked)
                        {
                            thrLista2 = new Thread(ThreadListaFaces);
                            thrLista2.Start();
                        }
                        player2.Paint += new System.Windows.Forms.PaintEventHandler(this.player2_Paint);
                    }
                    if (!string.IsNullOrEmpty(PropriedadeCamera3.IP))
                    {
                        if (cbHistorico.Checked)
                        {
                            thrLista3 = new Thread(ThreadListaFaces);
                            thrLista3.Start();
                        }
                        player3.Paint += new System.Windows.Forms.PaintEventHandler(this.player3_Paint);
                    }
                    if (!string.IsNullOrEmpty(PropriedadeCamera4.IP))
                    {
                        if (cbHistorico.Checked)
                        {
                            thrLista4 = new Thread(ThreadListaFaces);
                            thrLista4.Start();
                        }
                        player4.Paint += new System.Windows.Forms.PaintEventHandler(this.player4_Paint);
                    }
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(PropriedadeCamera1.IP))
                    {
                        if (thr1 != null)
                            thr1.Abort();
                        if (thrFace1 != null)
                            thrFace1.Abort();
                        if (thrLista1 != null)
                            thrLista1.Abort();
                    }
                    if (!string.IsNullOrEmpty(PropriedadeCamera2.IP))
                    {
                        if (thr2 != null)
                            thr2.Abort();
                        if (thrFace2 != null)
                            thrFace2.Abort();
                        if (thrLista2 != null)
                            thrLista2.Abort();
                    }
                    if (!string.IsNullOrEmpty(PropriedadeCamera3.IP))
                    {
                        if (thr3 != null)
                            thr3.Abort();
                        if (thrFace3 != null)
                            thrFace3.Abort();
                        if (thrLista3 != null)
                            thrLista3.Abort();
                    }
                    if (!string.IsNullOrEmpty(PropriedadeCamera4.IP))
                    {
                        if (thr4 != null)
                            thr4.Abort();
                        if (thrFace4 != null)
                            thrFace4.Abort();
                        if (thrLista4 != null)
                            thrLista4.Abort();
                    }

                    executandoBusca = false;
                    btnBuscarPessoa.Text = "Iniciar Busca";
                }
            }
            else
            {
                btnBuscarPessoa.Text = "Iniciar Busca";
                if (!string.IsNullOrEmpty(PropriedadeCamera1.IP))
                {
                    if (thr1 != null)
                        thr1.Abort();
                    if (thrFace1 != null)
                        thrFace1.Abort();
                    if (thrLista1 != null)
                        thrLista1.Abort();
                }
                if (!string.IsNullOrEmpty(PropriedadeCamera2.IP))
                {
                    if (thr2 != null)
                        thr2.Abort();
                    if (thrFace2 != null)
                        thrFace2.Abort();
                    if (thrLista2 != null)
                        thrLista2.Abort();
                }
                if (!string.IsNullOrEmpty(PropriedadeCamera3.IP))
                {
                    if (thr3 != null)
                        thr3.Abort();
                    if (thrFace3 != null)
                        thrFace3.Abort();
                    if (thrLista3 != null)
                        thrLista3.Abort();
                }
                if (!string.IsNullOrEmpty(PropriedadeCamera4.IP))
                {
                    if (thr4 != null)
                        thr4.Abort();
                    if (thrFace4 != null)
                        thrFace4.Abort();
                    if (thrLista4 != null)
                        thrLista4.Abort();
                }
                executandoBusca = false;
                flagFace1 = false;
                flagFace2 = false;
                flagFace3 = false;
                flagFace4 = false;
            }
        }

        private void btnCadastroFaces_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false, Title = "Selecione", Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap btm = new Bitmap(System.Drawing.Image.FromFile(ofd.FileName));
                    if (btm.Width > 800 || btm.Height > 600)
                    {
                        MessageBox.Show(Convert.ToString("Tamanho da imagem superior a 800x600"));
                        return;
                    }
                    Image<Bgr, byte> img = new Image<Bgr, byte>(btm);
                    Rectangle[] rt = cascadeClassifier.DetectMultiScale(img, 1.1, 1, new Size(15, 15), new Size(1400, 1000));
                    foreach (Rectangle r in rt)
                    {
                        using (Graphics gr = Graphics.FromImage(btm))
                        {
                            using (Pen p = new Pen(Color.Green, 1))
                            {
                                gr.DrawRectangle(p, r);
                            }
                        }
                    }
                    if (rt.Length == 1)
                    {
                        var pasta = Util.buscaDiretorioImagem("faces");
                        if (!File.Exists(pasta + ofd.SafeFileName))
                            File.Copy(ofd.FileName, pasta + ofd.SafeFileName);
                        Form c = new Form();
                        c.Text = "Verificação Cadastro";
                        c.Size = new Size(btm.Width, btm.Height);
                        c.Location = new Point(btm.Width, btm.Height);
                        System.Windows.Forms.PictureBox pb = new System.Windows.Forms.PictureBox();
                        pb.Size = new Size(btm.Width, btm.Height);
                        pb.Image = btm;
                        c.Controls.Add(pb);
                        c.Show();
                    }
                    else if (rt.Length > 1)
                    {
                        MessageBox.Show(Convert.ToString("Imagem possui mais de uma face"));
                    }
                    else
                    {
                        MessageBox.Show(Convert.ToString("Imagem sem face para cadastro"));
                    }

                }

            }
        }

        private void FecharExecucao(object sender, FormClosedEventArgs e)
        {
            FinalizarTodasCapturas();
        }

        private void reiniciar(object sender, EventArgs e)
        {
            if (!flagConectado)
            {
                btnConectar.Text = "Conectar Câmeras";
            }
            if (!executandoBusca)
            {
                btnBuscarPessoa.Text = "Iniciar Busca";
            }
        }

        private void btnConfigurar_Click(object sender, EventArgs e)
        {
            Configuracao con = new Configuracao();
            con.Show();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (flagConectado)
            {
                FinalizarTodasCapturas();
                flagConectado = false;
                btnConectar.Text = "Conectar Câmeras";

            }
            else
            {
                if (!string.IsNullOrEmpty(PropriedadeCamera1.IP))
                {
                    camera1 = string.Format(strConexao, PropriedadeCamera1.Usuario, PropriedadeCamera1.Senha, PropriedadeCamera1.IP);
                    IniciarCapturaImagem(new VideoFileSource(camera1), player1);
                    flagConectado = true;
                }

                if (!string.IsNullOrEmpty(PropriedadeCamera2.IP))
                {
                    camera2 = string.Format(strConexao, PropriedadeCamera2.Usuario, PropriedadeCamera2.Senha, PropriedadeCamera2.IP);
                    IniciarCapturaImagem(new VideoFileSource(camera2), player2);
                    flagConectado = true;
                }

                if (!string.IsNullOrEmpty(PropriedadeCamera3.IP))
                {
                    camera3 = string.Format(strConexao, PropriedadeCamera3.Usuario, PropriedadeCamera3.Senha, PropriedadeCamera3.IP);
                    IniciarCapturaImagem(new VideoFileSource(camera3), player3);
                    flagConectado = true;
                }

                if (!string.IsNullOrEmpty(PropriedadeCamera4.IP))
                {
                    camera4 = string.Format(strConexao, PropriedadeCamera4.Usuario, PropriedadeCamera4.Senha, PropriedadeCamera4.IP);
                    IniciarCapturaImagem(new VideoFileSource(camera4), player4);
                    flagConectado = true;
                }
                if (flagConectado)
                {
                    btnConectar.Text = "Desconectar Câmeras";
                }
            }
            Cursor.Current = Cursors.Default;
        }
        void ReconhecimentoFace()
        {
            int i = 0;
            while (true)
            {
                i++;
                if (i == 1 && (!string.IsNullOrEmpty(PropriedadeCamera1.IP)))
                {
                    ReconhecimentoFaceCaptura(camera1, 1, player1.Width, player1.Height);
                }
                if (i == 2 && (!string.IsNullOrEmpty(PropriedadeCamera2.IP)))
                {
                    ReconhecimentoFaceCaptura(camera2, 2, player2.Width, player2.Height);
                }
                if (i == 3 && (!string.IsNullOrEmpty(PropriedadeCamera3.IP)))
                {
                    ReconhecimentoFaceCaptura(camera3, 3, player3.Width, player3.Height);
                }
                if (i == 4 && (!string.IsNullOrEmpty(PropriedadeCamera4.IP)))
                {
                    ReconhecimentoFaceCaptura(camera4, 4, player4.Width, player4.Height);
                }
                if (i > 2) i = 0;
            }
        }
        void ReconhecimentoFace2()
        {
            int i = 2;
            while (true)
            {
                i++;
                if (i == 1 && (!string.IsNullOrEmpty(PropriedadeCamera1.IP)))
                {
                    ReconhecimentoFaceCaptura(camera1, 1, player1.Width, player1.Height);                    
                }
                if (i == 2 && (!string.IsNullOrEmpty(PropriedadeCamera2.IP)))
                {
                    ReconhecimentoFaceCaptura(camera2, 2, player2.Width, player2.Height);
                }
                if (i == 3 && (!string.IsNullOrEmpty(PropriedadeCamera3.IP)))
                {
                    ReconhecimentoFaceCaptura(camera3, 3, player3.Width, player3.Height);
                }
                if (i == 4 && (!string.IsNullOrEmpty(PropriedadeCamera4.IP)))
                {
                    ReconhecimentoFaceCaptura(camera4, 4, player4.Width, player4.Height);
                }
                Thread.Sleep(2000);
                if (i > 4) i = 2;
            }
        }

        async void ReconhecimentoFaceCaptura(string camera, int thrID, int tamanhoX, int tamanhoY)
        {
            using (var video = new VideoFileReader())
            {

                Bitmap frame;
                BitmapData bitmapData;
                IFaceClient client = AutenticacaoClient.client;
                try
                {
                    video.Open(camera);
                    frame = video.ReadVideoFrame(0);
                    bitmapData = frame.LockBits(ImageLockMode.ReadWrite);
                    frame.UnlockBits(bitmapData);
                    Image<Bgr, byte> img = new Image<Bgr, byte>(frame);
                    Rectangle[] rt = cascadeClassifier.DetectMultiScale(img, 1.1, 1, new Size(15, 15), new Size(1400, 1000));

                    if (rt.Length == 0)
                    {
                        if (thrID == 1) flagFace1 = false;
                        if (thrID == 2) flagFace2 = false;
                        if (thrID == 3) flagFace3 = false;
                        if (thrID == 4) flagFace4 = false;
                    }
                    else
                    {
                        if (thrID == 1) flagFace1 = true;
                        if (thrID == 2) flagFace2 = true;
                        if (thrID == 3) flagFace3 = true;
                        if (thrID == 4) flagFace4 = true;
                        await buscaPessoaFace(camera, thrID, tamanhoX, tamanhoY);
                        var x = (tamanhoX / (decimal)frame.Width);
                        var y = (tamanhoY / (decimal)frame.Height);
                        List<Rectangle> rec1 = new List<Rectangle>();
                        List<Rectangle> rec2 = new List<Rectangle>();
                        List<Rectangle> rec3 = new List<Rectangle>();
                        List<Rectangle> rec4 = new List<Rectangle>();
                        foreach (var face in rt)
                        {
                            Rectangle r = new Rectangle();

                            r.X = Convert.ToInt32(face.X * x);
                            r.Y = Convert.ToInt32(face.Y * y);
                            r.Width = Convert.ToInt32(face.Width * x);
                            r.Height = Convert.ToInt32(face.Height * y);
                            if (thrID == 1) rec1.Add(r);
                            if (thrID == 2) rec2.Add(r);
                            if (thrID == 3) rec3.Add(r);
                            if (thrID == 4) rec4.Add(r);
                        }
                        if (thrID == 1) lstRect1 = rec1;
                        if (thrID == 2) lstRect2 = rec2;
                        if (thrID == 3) lstRect3 = rec3;
                        if (thrID == 4) lstRect4 = rec4;
                    }
                    video.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Erro: {0} - Thread: {1}", ex.Message, thrID.ToString()));
                    video.Close();
                }
            }
        }

        private async Task<bool> buscaPessoaFace(string urlGlobal, int threadId, int tamanhoX, int tamanhoY)
        {
            lstView = new List<PessoaDTO>();
            List<Rectangle> lstR = new List<Rectangle>(); ;

            int tentativas = 0;
            bool flagFace = false;

            if (threadId == 1) lstR = new List<Rectangle>(lstRect1);
            if (threadId == 2) lstR = new List<Rectangle>(lstRect2);
            if (threadId == 3) lstR = new List<Rectangle>(lstRect3);
            if (threadId == 4) lstR = new List<Rectangle>(lstRect4);

            if (threadId == 1) flagFace = flagFace1;
            if (threadId == 2) flagFace = flagFace2;
            if (threadId == 3) flagFace = flagFace3;
            if (threadId == 4) flagFace = flagFace4;

            if (flagFace)
            {
                List<Pessoa> lst = new List<Pessoa>();
                try
                {
                    using (var video = new VideoFileReader())
                    {
                        video.Open(urlGlobal);
                        var myBitmap = video.ReadVideoFrame(0);
                        if (myBitmap == null) throw new Exception("Erro captura frame");
                        var x = (tamanhoX / (decimal)myBitmap.Width);
                        var y = (tamanhoY / (decimal)myBitmap.Height);
                        Reconhecer r = new Reconhecer();
                        var result = await r.BuscarFace(AutenticacaoClient.client, myBitmap);
                        waitLstView = true;
                        lstView.Clear();
                        lst = new List<Pessoa>(result);
                        lst.ForEach(p =>
                        {
                            recalculoPosicao(p.RetanguloFace, x, y);
                            if (p.Procurado)
                            {
                                lstView.Insert(0, new PessoaDTO(p, threadId));
                            }
                        });
                        waitLstView = false;
                    }

                    if (threadId == 1) lstPessoa1 = new List<Pessoa>(lst);
                    if (threadId == 2) lstPessoa2 = new List<Pessoa>(lst);
                    if (threadId == 3) lstPessoa3 = new List<Pessoa>(lst);
                    if (threadId == 4) lstPessoa4 = new List<Pessoa>(lst);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro busca face: " + ex.Message);
                }
            }
            else
            {
                if (tentativas > 0)
                {
                    if (threadId == 1) lstPessoa1.Clear();
                    if (threadId == 2) lstPessoa2.Clear();
                    if (threadId == 3) lstPessoa3.Clear();
                    if (threadId == 4) lstPessoa4.Clear();

                    tentativas = 0;
                }
                tentativas++;
            }
            return true;
        }

        public void recalculoPosicao(FaceRectangle fr, decimal x, decimal y)
        {
            fr.Left = Convert.ToInt32(fr.Left * x);
            fr.Top = Convert.ToInt32(fr.Top * y);
            fr.Width = Convert.ToInt32(fr.Width * x);
            fr.Height = Convert.ToInt32(fr.Height * y);
        }

        private void player1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!executandoBusca) return;
            try
            {
                if (lstPessoa1.Count > 0 && flagFace1)
                {
                    foreach (var p in lstPessoa1)
                    {
                        Rectangle r = new Rectangle(p.RetanguloFace.Left, p.RetanguloFace.Top, p.RetanguloFace.Width, p.RetanguloFace.Height);
                        if (p.Procurado)
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(redPen2, rect2);
                            e.Graphics.DrawString(p.Nome.ToUpper(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Y - 16.0f);
                            e.Graphics.DrawString(p.Confianca.ToString(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Height + r.Y + 3.0f);
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(greenPen2, rect2);
                        }
                    }
                }
                else if (flagFace1)
                {
                    foreach (Rectangle r in lstRect1)
                    {
                        Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                        e.Graphics.DrawRectangle(greenPen2, rect2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro desenhar imagem: {0}", ex.Message));
            }
        }

        private void player2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!executandoBusca) return;
            try
            {
                if (lstPessoa2.Count > 0 && flagFace2)
                {
                    foreach (var p in lstPessoa2)
                    {
                        Rectangle r = new Rectangle(p.RetanguloFace.Left, p.RetanguloFace.Top, p.RetanguloFace.Width, p.RetanguloFace.Height);
                        if (p.Procurado)
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(redPen2, rect2);
                            e.Graphics.DrawString(p.Nome.ToUpper(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Y - 16.0f);
                            e.Graphics.DrawString(p.Confianca.ToString(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Height + r.Y + 3.0f);
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(greenPen2, rect2);
                        }
                    }
                }
                else if (flagFace2)
                {
                    foreach (Rectangle r in lstRect2)
                    {
                        Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                        e.Graphics.DrawRectangle(greenPen2, rect2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro desenhar imagem: {0}", ex.Message));
            }
        }

        private void player3_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!executandoBusca) return;
            try
            {
                if (lstPessoa3.Count > 0 && flagFace3)
                {
                    foreach (var p in lstPessoa3)
                    {
                        Rectangle r = new Rectangle(p.RetanguloFace.Left, p.RetanguloFace.Top, p.RetanguloFace.Width, p.RetanguloFace.Height);
                        if (p.Procurado)
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(redPen2, rect2);
                            e.Graphics.DrawString(p.Nome.ToUpper(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Y - 16.0f);
                            e.Graphics.DrawString(p.Confianca.ToString(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Height + r.Y + 3.0f);
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(greenPen2, rect2);
                        }
                    }
                }
                else if (flagFace3)
                {
                    foreach (Rectangle r in lstRect3)
                    {
                        Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                        e.Graphics.DrawRectangle(greenPen2, rect2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro desenhar imagem: {0}", ex.Message));
            }
        }

        private void player4_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!executandoBusca) return;
            try
            {
                if (lstPessoa4.Count > 0 && flagFace4)
                {
                    foreach (var p in lstPessoa4)
                    {
                        Rectangle r = new Rectangle(p.RetanguloFace.Left, p.RetanguloFace.Top, p.RetanguloFace.Width, p.RetanguloFace.Height);
                        if (p.Procurado)
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(redPen2, rect2);
                            e.Graphics.DrawString(p.Nome.ToUpper(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Y - 16.0f);
                            e.Graphics.DrawString(p.Confianca.ToString(), fonteProcurado, drawBrush, x: (float)r.X, y: (float)r.Height + r.Y + 3.0f);
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                            e.Graphics.DrawRectangle(greenPen2, rect2);
                        }
                    }
                }
                else if (flagFace4)
                {
                    foreach (Rectangle r in lstRect4)
                    {
                        Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                        e.Graphics.DrawRectangle(greenPen2, rect2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro desenhar imagem: {0}", ex.Message));
            }
        }

        private async void btnExcluirFaces_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Treinar t = new Treinar();
                if (await t.ExcluirGrupo(AutenticacaoClient.client))
                {
                    MessageBox.Show("Grupos excluídos", "Sauron");
                }
                else
                {
                    MessageBox.Show("Não foi possivel excluir os grupos", "Sauron");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Não foi possivel excluir os grupos", "Sauron");
                Cursor.Current = Cursors.Default;
            }
        }

        private async void btnTreinarReconhecimento_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Treinar treinar = new Treinar();
                if (await treinar.TreinarGrupo(AutenticacaoClient.client))
                {
                    MessageBox.Show("Treino executado com sucesso", "Sauron");
                }
                else
                {
                    MessageBox.Show("Erro ao executar treinamento", "Sauron");
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Erro ao executar treinamento", "Sauron");
                Cursor.Current = Cursors.Default;
            }
            Cursor.Current = Cursors.Default;
        }

        public void ThreadListaFaces()
        {
            try
            {
                MethodInvoker mi = new MethodInvoker(this.AlteraLista);
                while (executandoBusca)
                {
                    Thread.Sleep(5000);
                    this.BeginInvoke(mi);
                    

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao acessar lista de faces: " + ex.Message);
            }
        }
        private void AlteraLista()
        {
            try
            {
                var lst = new List<PessoaDTO>();
                if (lvFaces.Items.Count > 30)
                {
                    lvFaces.Items.Clear();
                }
                if (!waitLstView)
                {
                    lst = new List<PessoaDTO>(lstView.Select(x => x).Distinct().ToList());
                }
                for (int i = 0; i < lst.Count() && (flagFace1 || flagFace2 || flagFace3 || flagFace4); i++)
                {
                    lvFaces.Items.Add(lst[i].Pessoa.Nome, 0);
                    lvFaces.Items.Add(string.Format("* {0}  -  Câmera: {1} ", lst[i].Pessoa.Confianca.ToString(), lst[i].Camera.ToString()), 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao acessar lista - " + ex.Message);
            }
        }
    }
}
