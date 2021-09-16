using ReconhecimentoFacial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sauron.Models
{
    public class PessoaDTO
    {
        public PessoaDTO(Pessoa pessoa, int camera)
        {
            this.Pessoa = pessoa;
            this.Camera = camera;
        }
        public Pessoa Pessoa { get; set; }
        public int Camera { get; set; }
    }
}
