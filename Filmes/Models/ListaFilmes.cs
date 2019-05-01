using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmes.Models
{
    public class ListaFilmes
    {
        public ListaFilmes(){
            

            }


        public List<DadosFilmes> Filmes { get; set; }
    
        public DadosFilmes Filme { get; set; }
        public string pesquisa { get; set; }

    }
}