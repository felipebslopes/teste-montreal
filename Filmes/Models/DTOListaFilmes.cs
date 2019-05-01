using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmes.Models
{
    public class DTOListaFilmes
    {
      public List<DTOResultado> results { get; set; }
      public string page { get; set; }
      public string total_results { get; set; }
      public DTOIntervalo dates { get; set; }
      public string total_pages { get; set; }

    }
}