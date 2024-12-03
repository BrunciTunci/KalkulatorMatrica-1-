using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KalkulatorMatrica.Pages
{
    public class KontaktModel : PageModel
    {
        public bool ImaPoruka = false;
        public string Ime = "";
        public string Prezime = "";
        public string Poruka = "";
        
        public void OnGet()
        {
        }

        public void OnPost()
        {
            ImaPoruka = true;
            Ime = Request.Form["Ime"];
            Prezime = Request.Form["Prezime"];
            Poruka = Request.Form["Poruka"];
            var poruka = $"Ime: {Request.Form["Ime"]}\nPrezime: {Request.Form["Prezime"]}\nPoruka: {Request.Form["Poruka"]}\nDatum: {DateTime.Now}\n\n";
            System.IO.File.AppendAllText("Poruke.txt", poruka);
            ImaPoruka = true;
        }
    }
}
