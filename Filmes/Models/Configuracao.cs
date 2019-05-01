using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmes.Models
{
    public class Configuracao
    {
       public Images images { get; set; }
       public List<string> change_keys { get; set; }

    }
}