using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmes.Models
{
    public class DadosFilmes
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public string titulo_original { get; set; }
        public string imagem { get; set; }
        public int id { get; set; }
        public string lancamento { get; set; }
        public string poster { get; set; }
        public string pesquisa { get; set; }
        public bool pesquisaEfetuda { get; set; }


        public DadosFilmes Filme { get; set; }
    }
}