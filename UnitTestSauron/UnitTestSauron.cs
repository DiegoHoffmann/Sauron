using System;
using System.Configuration;
using System.Drawing;
using System.Threading;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReconhecimentoFacial;
using ReconhecimentoFacial.Helpers;
using ReconhecimentoFacial.Models;

namespace UnitTestSauron
{
    [TestClass]
    public class UnitTestSauron
    {
        [TestMethod]
        public void TreinarGrupoTeste()
        {
            string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
            string SUBSCRIPTION_KEY = ConfigurationManager.AppSettings["apiKey"].ToString();
            IFaceClient client = Autenticar.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            Treinar treinar = new Treinar();
            var treinamento = treinar.TreinarGrupo(client);
            Thread.Sleep(30000);
        }
        [TestMethod]
        public void CriarDicionarioTeste()
        {
            Treinar r = new Treinar();
            r.CriarDicionario();
        }
        [TestMethod]
        public void ReconhecerTeste()
        {
            string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
            string SUBSCRIPTION_KEY = ConfigurationManager.AppSettings["apiKey"].ToString();
            string caminhoTeste = Util.buscaDiretorioImagem("Sauron\\faces");
            string sourceImageFileNameTeste = caminhoTeste + "Diego-01.png";
            IFaceClient client = Autenticar.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            Reconhecer reconhecer = new Reconhecer();
            Bitmap b = new Bitmap(sourceImageFileNameTeste);
            Grupo.IdGrupo = "30d3ce3f-a761-4a3e-a2bd-b6b5d19af500";
            var buscaFace = reconhecer.BuscarFace(client, Util.buscaDiretorioImagem("Sauron\\FacesCapturadas") + "face.jpg");
            Thread.Sleep(60000);
        }

        [TestMethod]
        public void IdentificarPessoaRequestTeste()
        {
            Reconhecer r = new Reconhecer();
            Bitmap btm = new Bitmap(Util.buscaDiretorioImagem("Sauron\\FacesCapturadas") + "face.jpg");
            string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
            string SUBSCRIPTION_KEY = ConfigurationManager.AppSettings["apiKey"].ToString();
            IFaceClient client = Autenticar.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            Grupo.IdGrupo = "30d3ce3f-a761-4a3e-a2bd-b6b5d19af500";
            var buscaFace = r.BuscarFace(client, btm);
            Thread.Sleep(90000);

        }
    }
}
