using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace APP.Models
{
    public class LoteriaInfo
    {
        
        private readonly Lazy<HtmlDocument> Lazydoc;

        public LoteriaInfo()
        {
            Lazydoc = new Lazy<HtmlDocument>(() =>
            {
                var oweb = new HtmlWeb();
                return oweb.Load("https://loteriasdominicanas.com");
            });
            
        }
        private HtmlDocument doc => Lazydoc.Value;
        public async Task<List<Loteria>> GetDataAsync()
        {
            var informacionLoteria = new List<Loteria>();
            var fechaContenedor = doc.DocumentNode.CssSelect("[class='session-date px-2']").Select(e => e.InnerText.Trim()).ToList();
            var nombresLoterias = doc.DocumentNode.CssSelect(".game-title");
            var numerosLoterias = doc.DocumentNode.CssSelect(".game-scores.p-2.ball-mode");

            for(int i = 0; i< fechaContenedor.Count; i++)
            {
                var fecha = fechaContenedor[i];

                var nombres = nombresLoterias.ElementAtOrDefault(i)?.CssSelect("span").Select(e => e.InnerHtml).ToList() ?? new List<string>();
                var numeros = numerosLoterias.ElementAtOrDefault(i)?.CssSelect("span").Select(e => e.InnerHtml).ToList() ?? new List<string>();

                var informacion = new Loteria
                {
                    Id = i,
                    Sorteo = string.Join(", ", nombres),
                    Numeros = numeros,
                    Fecha = fecha
                };
                informacionLoteria.Add(informacion);

            }
            return informacionLoteria;

        }
    }
}
