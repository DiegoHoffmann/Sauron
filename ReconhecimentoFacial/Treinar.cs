using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using ReconhecimentoFacial.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using ReconhecimentoFacial.Models;

namespace ReconhecimentoFacial
{

    public class Treinar
    {
        private string ENDPOINT = ConfigurationManager.AppSettings["urlSauronAzure"].ToString();
        private string SUBSCRIPTION_KEY = ConfigurationManager.AppSettings["apiKey"].ToString();
        string caminhoCaptura = Util.buscaDiretorioImagem("Sauron\\FacesCapturadas");
        string caminhoTeste = Util.buscaDiretorioImagem("Sauron\\img");

        static string sourcePersonGroup = null;
        public async Task<bool> TreinarGrupo(IFaceClient client)
        {
            var personDictionary = CriarDicionario();
            PersonGroup result;
            sourcePersonGroup = Grupo.IdGrupo;
            try
            {
                result = await client.PersonGroup.GetAsync(Grupo.IdGrupo);
            }
            catch (Exception ex)
            {
                result = null;
            }
            if (result != null)
                await ExcluirGrupo(client);
            
            await client.PersonGroup.CreateAsync(Grupo.IdGrupo, "Sauron_Group", recognitionModel: RecognitionModel.Recognition01);

            foreach (var groupedFace in personDictionary)
            {
                foreach (var gr in groupedFace.Value)
                {
                    Stream s = File.OpenRead(gr);

                    Person person = await client.PersonGroupPerson.CreateAsync(personGroupId: Grupo.IdGrupo, name: groupedFace.Key);
                    var a = await client.PersonGroupPerson.AddFaceFromStreamAsync(Grupo.IdGrupo, person.PersonId, s, detectionModel: DetectionModel.Detection01);
                }
            }
            await client.PersonGroup.TrainAsync(Grupo.IdGrupo);            
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(Grupo.IdGrupo);

                if (trainingStatus.Status != TrainingStatusType.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
            return true;
        }

        public async Task<string> TreinarFaceIndividualGrupo(IFaceClient client, string img, List<string> idGuid)
        {
            Stream s = File.OpenRead(img);
            List<Guid> lstGuid = new List<Guid>();
            idGuid.ForEach(x => lstGuid.Add(new Guid(x)));
            foreach (var x in lstGuid)
            {
                try
                {
                    await client.PersonGroupPerson.DeleteAsync(Grupo.IdGrupo, x);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao deletar pessoa: " + ex.Message);
                }
            }
            PersonGroup result;
            try
            {
                result = await client.PersonGroup.GetAsync(Grupo.IdGrupo);
            }
            catch (Exception ex)
            {
                await client.PersonGroup.CreateAsync(Grupo.IdGrupo, "Sauron_Group", recognitionModel: RecognitionModel.Recognition01);
            }

            Person person = await client.PersonGroupPerson.CreateAsync(personGroupId: Grupo.IdGrupo, name: "Urgente");
            var resultPerson = await client.PersonGroupPerson.AddFaceFromStreamAsync(Grupo.IdGrupo, person.PersonId, s, detectionModel: DetectionModel.Detection01);
            await client.PersonGroup.TrainAsync(Grupo.IdGrupo);

            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(Grupo.IdGrupo);

                if (trainingStatus.Status != TrainingStatusType.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
            return person.PersonId.ToString();
        }

        public Dictionary<string, string[]> CriarDicionario()
        {
            Dictionary<string, string[]> personDictionary = new Dictionary<string, string[]>();

            string caminho = Util.buscaDiretorioImagem("Sauron\\faces");
            DirectoryInfo di = new DirectoryInfo(caminho);
            foreach (var img in di.GetFiles())
            {
                string nome = img.Name.Replace(img.Extension, "").Split('-')[0];
                if (!personDictionary.ContainsKey(nome))
                {
                    var lst_img = di.GetFiles().Where(x => x.Name.Contains(nome));
                    List<string> lstString = new List<string>();
                    lst_img.ToList().ForEach(x => { lstString.Add(x.FullName); });
                    personDictionary.Add(nome, lstString.ToArray());
                }
            }

            return personDictionary;
        }

        public async void BuscarFace(IFaceClient client, string personGroupId)
        {
            using (Stream s = File.OpenRead(""))
            {
                var faces = await client.Face.DetectWithStreamAsync(s);
                var faceIds = faces.Select(face => face.FaceId.Value).ToArray();

                var results = await client.Face.IdentifyAsync(faceIds, personGroupId);
                foreach (var identifyResult in results)
                {
                    if (identifyResult.Candidates.Count == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await client.PersonGroupPerson.GetAsync(personGroupId, candidateId);
                    }
                }
            }
        }

        public async Task<bool> ExcluirGrupo(IFaceClient client)
        {
            try
            {
                var result = await client.PersonGroup.ListAsync();
                foreach (var r in result)
                {
                    await client.PersonGroup.DeleteAsync(r.PersonGroupId);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao limpar grupos: " + ex.Message);
                return false;
            }

        }

    }
}
