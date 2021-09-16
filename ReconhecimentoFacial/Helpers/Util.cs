using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReconhecimentoFacial.Helpers
{
    public static class Util
    {
        public static string buscaDiretorioImagem(string pasta)
        {
            var diretorio = Directory.GetCurrentDirectory();
            var dirFormatado = string.Format("{0}\\{1}", diretorio, pasta);
            while (!Directory.Exists(dirFormatado) && diretorio.Contains("Sauron"))
            {
                diretorio = diretorio.Substring(0, (diretorio.LastIndexOf("\\")));
                dirFormatado = string.Format("{0}\\{1}", diretorio, pasta);
            }
            return dirFormatado + "\\";
        }
    }
}
