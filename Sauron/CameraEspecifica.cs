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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sauron
{

    public partial class CameraEspecifica : Form
    {
        public static readonly System.Drawing.Imaging.Encoder Transformation;
        static string urlGlobal = null;
        float a = 0;
        static int tentativas = 0;
        static List<Rectangle> lstRect;
        static List<Pessoa> lstPessoa;
        static List<Pessoa> lstView;
        static bool waitLstView;
        static bool flagFace = false;
        static bool executandoBusca = false;
        private Form fr;
        Thread thr1;
        Thread thrFace;
        Thread thrLista;
        public static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier(@"XML\\haarcascade_frontalface_alt_tree.xml");
        static string caminhoImagemCapturada = Util.buscaDiretorioImagem("Sauron\\FacesCapturadas");
        private SolidBrush drawBrush;
        private Pen redPen2;
        private Pen greenPen2;
        private Font fonteProcurado;
        public CameraEspecifica(string url, Form f)
        {
            fr = f;
            urlGlobal = url;
            InitializeComponent();
            VideoFileSource videoFileSource = new VideoFileSource(url);
            OpenVideoSource(videoFileSource);
            lstRect = new List<Rectangle>();
            lstPessoa = new List<Pessoa>();
            lstView = new List<Pessoa>();
            drawBrush = new SolidBrush(Color.Red);
            redPen2 = new Pen(Color.Red, 3);
            greenPen2 = new Pen(Color.Green, 2);
            fonteProcurado = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
            waitLstView = false;
        }

        public void OpenVideoSource(Accord.Video.IVideoSource source)
        {
            this.Cursor = Cursors.WaitCursor;
            CloseCurrentVideoSource();
            playerEspecifico.VideoSource = source;
            playerEspecifico.Start();
            this.Cursor = Cursors.Default;
        }
        private void CloseCurrentVideoSource()
        {
            if (playerEspecifico.VideoSource != null)
            {
                playerEspecifico.SignalToStop();
                playerEspecifico.VideoSource = null;
            }
        }

        private async void btnCapturarImagem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Bitmap myBitmap;
                ImageCodecInfo myImageCodecInfo;
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                DirectoryInfo di = new DirectoryInfo(caminhoImagemCapturada);
                List<string> lstGuidDelete = new List<string>();
                foreach (var img in di.GetFiles())
                {
                    lstGuidDelete.Add(img.Name.Replace(img.Extension, ""));
                    FileInfo fi = new FileInfo(img.FullName);
                    fi.Delete();
                }


                myBitmap = playerEspecifico.GetCurrentVideoFrame();
                if (PossuiFace(myBitmap))
                {
                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = Encoder.Quality;
                    myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    string idGuidCaptura = Guid.NewGuid().ToString();
                    string nomeImg = string.Format("{0}{1}.jpg", caminhoImagemCapturada, idGuidCaptura);
                    myBitmap.Save(nomeImg, myImageCodecInfo, myEncoderParameters);
                    Thread.Sleep(2000);
                    Treinar tr = new Treinar();
                    var result = await tr.TreinarFaceIndividualGrupo(AutenticacaoClient.client, nomeImg, lstGuidDelete);
                    FileInfo fi = new FileInfo(string.Format("{0}{1}.jpg", caminhoImagemCapturada, idGuidCaptura));
                    fi.MoveTo(string.Format("{0}{1}.jpg", caminhoImagemCapturada, result));
                    
                    MessageBox.Show("Face Cadastrada com sucesso", "Sauron");
                }
                else
                {
                    MessageBox.Show("Nenhuma face encontrada na imagem", "Sauron");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Erro ao capturar face", "Sauron");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        

        private bool PossuiFace(Bitmap btm)
        {            
            Image<Bgr, byte> img = new Image<Bgr, byte>(btm);
            Rectangle[] rt = cascadeClassifier.DetectMultiScale(img, 1.1, 1, new Size(15, 15), new Size(1400, 1000));
            if (rt.Length > 0) 
            {
                return true;
            }
             return false;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void btnIniciarBusca_click(object sender, EventArgs e)
        {
            if (!executandoBusca)
            {
                executandoBusca = true;
                btnIniciarBusca.Text = "Parar Busca";
                try
                {
                    thr1 = new Thread(new ThreadStart(() => ReconhecimentoFaceCaptura(1)));
                    thr1.Start();
                    thrFace = new Thread(buscaPessoaFace);
                    thrFace.Start();
                    if (cbHistorico.Checked)
                    {
                        thrLista = new Thread(ThreadListaFaces);
                        thrLista.Start();
                    }
                    playerEspecifico.Paint += new System.Windows.Forms.PaintEventHandler(this.cameraEspecifica_Paint);
                }
                catch (Exception ex)
                {
                    if (thr1 != null)
                        thr1.Abort();
                    if (thrFace != null)
                        thrFace.Abort();
                    if (thrLista != null)
                        thrLista.Abort();
                    executandoBusca = false;
                    btnIniciarBusca.Text = "Iniciar Busca";
                }
            }
            else
            {
                btnIniciarBusca.Text = "Iniciar Busca";
                if(thr1 != null)
                    thr1.Abort();
                if(thrFace != null)
                    thrFace.Abort();
                if(thrLista != null)
                    thrLista.Abort();

                executandoBusca = false;
                flagFace = false;
                waitLstView = false;
            }
        }

        private void btnBuscaApi_Click(object sender, EventArgs e)
        {
            thr1 = new Thread(new ThreadStart(() => MakeAnalysisRequest(playerEspecifico)));
            thr1.Start();
            playerEspecifico.Paint += new System.Windows.Forms.PaintEventHandler(this.cameraEspecifica_Paint);
            MakeAnalysisRequest(playerEspecifico);
        }

        static async void MakeAnalysisRequest(VideoSourcePlayer imageFilePath)
        {
            string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
            string subscriptionKey = ConfigurationManager.AppSettings["apiKey"].ToString();
            string uriBase = ConfigurationManager.AppSettings["urlSauronAzureApi"].ToString();
            HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false";
            string uri = uriBase + "?" + requestParameters;
            HttpResponseMessage response;
            while (true)
            {
                Bitmap btm = imageFilePath.GetCurrentVideoFrame();
                byte[] byteData = GetImageAsByteArray(btm);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                    string contentString = await response.Content.ReadAsStringAsync();

                    var rt = JsonSerializer.Deserialize<FaceResponse[]>(contentString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    lstRect = new List<Rectangle>();
                    if (rt.Length == 0) { flagFace = false; tentativas++; }
                    else
                    {
                        int i = 0;
                        foreach (var faces in rt)
                        {
                            Rectangle r = new Rectangle();
                            var x = (800 / (decimal)btm.Width);
                            var y = (600 / (decimal)btm.Height);
                            r.X = Convert.ToInt32(faces.FaceRectangle.Left * x);
                            r.Y = Convert.ToInt32(faces.FaceRectangle.Top * y);
                            r.Width = Convert.ToInt32(faces.FaceRectangle.Width * x);
                            r.Height = Convert.ToInt32(faces.FaceRectangle.Height * y);
                            flagFace = true;
                            lstRect.Add(r);

                        }
                    }
                }
            }
        }

        static byte[] GetImageAsByteArray(Bitmap btm)
        {

            using (var stream = new MemoryStream())
            {
                btm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }

        }

        async void ReconhecimentoFaceCaptura(int thrID)
        {
            using (var video = new VideoFileReader())
            {
                video.Open(urlGlobal);

                Bitmap frame;
                BitmapData bitmapData;
                UnmanagedImage unmanagedImage;
                IFaceClient client = AutenticacaoClient.client;
                Rectangle[] faces = new Rectangle[1];
                while (true)
                {
                    try
                    {
                        frame = video.ReadVideoFrame(0);
                        bitmapData = frame.LockBits(ImageLockMode.ReadWrite);
                        frame.UnlockBits(bitmapData);
                        Image<Bgr, byte> img = new Image<Bgr, byte>(frame);
                        Rectangle[] rt = cascadeClassifier.DetectMultiScale(img, 1.1, 1, new Size(15, 15), new Size(1400, 1000));
                        lstRect.Clear();
                        var x = (800 / (decimal)frame.Width);
                        var y = (600 / (decimal)frame.Height);
                        if (rt.Length == 0)
                        {
                            flagFace = false;
                            tentativas++;
                        }
                        else
                        {
                            int i = 0;
                            List<Rectangle> rec = new List<Rectangle>();
                            foreach (var face in rt)
                            {
                                Rectangle r = new Rectangle();

                                r.X = Convert.ToInt32(face.X * x);
                                r.Y = Convert.ToInt32(face.Y * y);
                                r.Width = Convert.ToInt32(face.Width * x);
                                r.Height = Convert.ToInt32(face.Height * y);
                                rec.Add(r);
                            }
                            lstRect = rec;
                            flagFace = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Erro: {0} - Thread: {1}", ex.Message, thrID.ToString()));
                    }
                }
            }
        }
        public void recalculoPosicao(FaceRectangle fr, decimal x, decimal y)
        {
            fr.Left = Convert.ToInt32(fr.Left * x);
            fr.Top = Convert.ToInt32(fr.Top * y);
            fr.Width = Convert.ToInt32(fr.Width * x);
            fr.Height = Convert.ToInt32(fr.Height * y);
        }
        private void FecharExecucao(object sender, FormClosedEventArgs e)
        {
            CloseCurrentVideoSource();
            if (thr1 != null)
                thr1.Abort();
            fr.Activate();
        }

        private void cameraEspecifica_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!executandoBusca) return;
            try
            {                
                if (lstPessoa.Count > 0 && flagFace)
                {                   
                    foreach (var p in lstPessoa)
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
                else if (flagFace)
                {                    
                    foreach (Rectangle r in lstRect)
                    {
                        Rectangle rect2 = new Rectangle(r.X, r.Y, r.Width, r.Height);
                        e.Graphics.DrawRectangle(greenPen2, rect2);                        
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(string.Format("Erro desenhar imagem: {0}", ex.Message));
            }
        }

        private async void buscaPessoaFace()
        {            
            lstView = new List<Pessoa>();
            while (true)
            {
                var lst = new List<Pessoa>(lstPessoa);
                var lstR = new List<Rectangle>(lstRect);
                Thread.Sleep(2000);
                if (flagFace)
                {
                    try
                    {
                        for(int i=0;i<lstR.Count(); i++)
                        {
                            var rc = lstR[i];
                            using (var video = new VideoFileReader())
                            {
                                video.Open(urlGlobal);
                                var myBitmap = video.ReadVideoFrame(0);
                                if (myBitmap == null) throw new Exception("Erro captura frame");
                                var x = (800 / (decimal)myBitmap.Width);
                                var y = (600 / (decimal)myBitmap.Height);

                                Reconhecer r = new Reconhecer();
                                var result = await r.BuscarFace(AutenticacaoClient.client, myBitmap);
                                waitLstView = true;
                                lstView.Clear();
                                lst = new List<Pessoa>(result);
                                lst.ForEach(p =>
                                {
                                    recalculoPosicao(p.RetanguloFace, x, y);
                                    if (p.Procurado) {
                                        lstView.Insert(0, p);
                                    }
                                });
                                waitLstView = false;
                            }
                        }
                        lstPessoa = new List<Pessoa>(lst);
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
                        lstPessoa.Clear();
                        tentativas = 0;
                    }
                }
            }
        }

        public void ThreadListaFaces()
        {
            try
            {
                MethodInvoker mi = new MethodInvoker(this.AlteraLista);
                while (executandoBusca)
                {
                    if(!waitLstView)
                        this.BeginInvoke(mi);
                    Thread.Sleep(5000);
                    
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
                if (lvFaces.Items.Count > 30)
                {
                    lvFaces.Items.Clear();
                }
                var lst = lstView.Select(x => x).Distinct().ToList();
                for (int i = 0; i < lst.Count() && flagFace; i++)
                {
                    lvFaces.Items.Add(lst[i].Nome, 0);
                    lvFaces.Items.Add(" - " + lst[i].Confianca.ToString(), 1);
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Alteração da lista inválido - " + ex.Message);
            }
        }
    }
}
