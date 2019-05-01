
using Filmes.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ConsumingWebAapiRESTinMVC.Controllers
{
    public class HomeController : Controller
    {

        string Baseurl = "http://api.themoviedb.org/3/";
        string Filmesurl = "http://api.themoviedb.org/3/";

        [HttpGet]
        public async Task<ActionResult> Index(string pesquisa, int? page)
        {
            int pageSize = 9;
            int pageIndex = 1;
            var lista = new List<DadosFilmes>();
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var filmes = new ListaFilmes();
            if (pesquisa == null || pesquisa == string.Empty)
            {
                lista = await Dados("index");
            }
            else
            {

                var urlPesquisa = "search/movie?api_key=7569f2f1ca73d5a27cd10beebcb6602e&&query=";

                var parametros = pesquisa.Split(' ');
                var query = String.Join("+", parametros);
                urlPesquisa = urlPesquisa + query;
                lista = await EfetuarPesquisa(urlPesquisa, "index");
            }
            IPagedList<DadosFilmes> Filmes = null;

            Filmes = lista.ToPagedList(pageIndex, pageSize);
            Filmes.FirstOrDefault().pesquisa = pesquisa;
            return View(Filmes);

        }


        //Transforma o DTO no model
        private List<DadosFilmes> ListaFilmes(List<DTOResultado> lista, string action)
        {

            var filmes = new List<DadosFilmes>();


            foreach (var item in lista)
            {
                var auxiliar = new DadosFilmes();
                auxiliar.descricao = item.overview;
                auxiliar.id = Convert.ToInt32(item.id);
                auxiliar.codigo = Convert.ToInt32(item.id);
                auxiliar.titulo_original = item.title;

                if (item.poster_path != null)
                {
                    if (action == "index")
                    {
                        auxiliar.imagem = "http://image.tmdb.org/t/p/w154" + item.poster_path;
                    }
                    else
                    {
                        auxiliar.imagem = "http://image.tmdb.org/t/p/w300" + item.poster_path;
                    }
                }
                else
                {
                    auxiliar.imagem = String.Empty;
                }
                auxiliar.poster = "http://image.tmdb.org/t/p/w154" + item.backdrop_path;
                try
                {
                    var aux = Convert.ToDateTime((item.release_date));
                    auxiliar.lancamento = aux.ToShortDateString();
                }
                catch
                {
                    auxiliar.lancamento = null;
                }
                filmes.Add(auxiliar);

            }

            return filmes;
        }

        [HttpGet]
        public async Task<ActionResult> Visualizar(int id, string pesquisa)
        {
            var filme = new ListaFilmes();
            var lista = new List<DadosFilmes>();
            if (pesquisa == null || pesquisa == String.Empty)
            {
                lista = await Dados("visualizar");
            }
            else
            {
                var urlPesquisa = "search/movie?api_key=7569f2f1ca73d5a27cd10beebcb6602e&&query=";
                int pageSize = 9;
                int pageIndex = 1;
                var parametros = pesquisa.Split(' ');
                var query = String.Join("+", parametros);
                urlPesquisa = urlPesquisa + query;
                lista = await EfetuarPesquisa(urlPesquisa, "visualizar");

            }
            var aux = lista.Where(x => x.codigo == id).FirstOrDefault();
            filme.Filme = aux;
            filme.pesquisa = pesquisa;
            return View(filme);

        }

        //Carrega os dados da tela inicial
        private async Task<List<DadosFilmes>> Dados(string action)
        {
            DTOListaFilmes retorno = new DTOListaFilmes();
            var listaFilmes = new List<DadosFilmes>();
            using (var client = new HttpClient())
            {



                client.BaseAddress = new Uri(Filmesurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage Res = await client.GetAsync("movie/upcoming?api_key=7569f2f1ca73d5a27cd10beebcb6602e&language=pt-BR");


                if (Res.IsSuccessStatusCode)
                {

                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;


                    retorno = JsonConvert.DeserializeObject<DTOListaFilmes>(EmpResponse);
                    listaFilmes = ListaFilmes(retorno.results, action);

                }
            }
            return listaFilmes;

        }

        //efetua a pesquisa
        [HttpPost]
        public async Task<ActionResult> Index(DadosFilmes model)
        {
            var urlPesquisa = "search/movie?api_key=7569f2f1ca73d5a27cd10beebcb6602e&&query=";
            int pageSize = 9;
            int pageIndex = 1;
            var parametros = model.pesquisa.Split(' ');
            var query = String.Join("+", parametros);
            urlPesquisa = urlPesquisa + query;
            var filmes = new ListaFilmes();
            var lista = await EfetuarPesquisa(urlPesquisa, "index");
            IPagedList<DadosFilmes> Filmes = null;
            Filmes = lista.ToPagedList(pageIndex, pageSize);
            model.pesquisaEfetuda = true;
            Filmes.FirstOrDefault().pesquisa = model.pesquisa;
            return View(Filmes);



        }


        private async Task<List<DadosFilmes>> EfetuarPesquisa(string url, string action)
        {
            DTOListaFilmes retorno = new DTOListaFilmes();
            var listaFilmes = new List<DadosFilmes>();
            using (var client = new HttpClient())
            {


                //Passing service base url  
                client.BaseAddress = new Uri(Filmesurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage Res = await client.GetAsync(url);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    retorno = JsonConvert.DeserializeObject<DTOListaFilmes>(EmpResponse);
                    listaFilmes = ListaFilmes(retorno.results, action);

                }
            }
            return listaFilmes;

        }

    }
}