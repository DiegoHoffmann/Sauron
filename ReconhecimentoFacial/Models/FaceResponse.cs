using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sauron.Models
{
    public class FaceResponse
    {
        // [{"faceId":"fdd99f59-540f-4fc3-b4d6-b5d11a974cf5","faceRectangle":{"top":325,"left":1034,"width":49,"height":49}}]
        public Guid? FaceId { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
    }
}
