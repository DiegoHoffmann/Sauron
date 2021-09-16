using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ReconhecimentoFacial.Models
{
    public class Pessoa
    {
        public string FaceId { get; set; }
        public string Nome { get; set; }
        public FaceRectangle RetanguloFace { get; set; }
        public double Confianca { get; set; }
        public bool Procurado { get; set; }

    }
}
