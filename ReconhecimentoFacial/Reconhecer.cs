using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using ReconhecimentoFacial.Helpers;
using ReconhecimentoFacial.Models;
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
using System.Threading.Tasks;


namespace ReconhecimentoFacial
{

    public class Reconhecer
    {
        static string caminhoTeste = Util.buscaDiretorioImagem("Sauron\\Faces");
        static string caminhoImagemCapturada = Util.buscaDiretorioImagem("Sauron\\FacesCapturadas");
        static string sourceImageFileNameTeste = Util.buscaDiretorioImagem("Sauron\\FacesRetorno") + "teste2.jpg";
        public async Task<IList<Pessoa>> BuscarFace(IFaceClient client, string btmEntrada)
        {
            using (Stream s = File.OpenRead(btmEntrada))
            {
                var faces = await client.Face.DetectWithStreamAsync(s);                
                var faceIds = faces.Select(face => face.FaceId.Value).ToArray();

                var results = await client.Face.IdentifyAsync(faceIds, Grupo.IdGrupo);
                return null;
            }            
        }
        public async Task<IList<Pessoa>> BuscarFace(IFaceClient client, Bitmap btmEntrada)
        {
            IList<DetectedFace> retorno = null;
            IList<IdentifyResult> results = null;
            IList<Pessoa> lstPessoa = new List<Pessoa>();
            try
            {   
                var facesId = await DetectarFaces(btmEntrada);
                Guid[] guids;
                guids = facesId.Select(x=>x.FaceId.Value).ToArray();

                results = await client.Face.IdentifyAsync(guids, Grupo.IdGrupo);

                Pessoa pessoa;                
                foreach(var r in results)
                {
                    var candidate = r.Candidates.FirstOrDefault();
                    pessoa = new Pessoa();
                    pessoa.Nome = string.Empty;
                    pessoa.RetanguloFace = facesId.Where(x => x.FaceId?.ToString() == r.FaceId.ToString())?.FirstOrDefault()?.FaceRectangle;
                    if (r.Candidates.Count == 0)
                    {
                        pessoa.Nome = string.Empty;
                        pessoa.Procurado = false;
                    }
                    else
                    {                        
                        Person person = await client.PersonGroupPerson.GetAsync(Grupo.IdGrupo, candidate.PersonId);
                        pessoa.Procurado = true;
                        pessoa.Nome = person.Name;
                        pessoa.Confianca = candidate.Confidence;
                    }                    
                    lstPessoa.Add(pessoa);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstPessoa;
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

        static byte[] GetImageAsByteArray(Bitmap btm)
        {

            using (var stream = new MemoryStream())
            {
                btm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }

        }
        public async Task<IList<Pessoa>> IdentificarPessoaRequest(Bitmap frame)
        {
            var facesId = await DetectarFaces(frame);

            string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
            string subscriptionKey = ConfigurationManager.AppSettings["apiKey"].ToString();
            string uriBase = ConfigurationManager.AppSettings["urlSauronAzureApi"].ToString();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false";
            string uri = uriBase + "?" + requestParameters;
            HttpResponseMessage response;
            
            return null;
        }

        public async Task<FaceResponse[]> DetectarFaces(Bitmap btm)
        {
            string contentString = string.Empty;
            try
            {
                string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
                string subscriptionKey = ConfigurationManager.AppSettings["apiKey"].ToString();
                string uriBase = ConfigurationManager.AppSettings["urlSauronAzureApi"].ToString();
                HttpClient client = new HttpClient();
                
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "returnFaceId=true&returnFaceLandmarks=false";
                string uri = uriBase + "?" + requestParameters;
                HttpResponseMessage response;
                
                byte[] byteData = GetImageAsByteArray(btm);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                    contentString = await response.Content.ReadAsStringAsync();

                    var rt = JsonSerializer.Deserialize<FaceResponse[]>(contentString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                    return rt;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Erro Azure: {0} - {1}", ex.Message, contentString));
            }
            return null;
        }
    }
}
