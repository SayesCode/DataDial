using System.Collections.Generic;

namespace DataDial.CountryCodeLookup
{
    public static class CountryCodeLookup
    {
        private static readonly Dictionary<int, string> countryCodes = new()
        {
            { 55, "Brasil" },
            { 1, "Estados Unidos/Canadá" },
            { 44, "Reino Unido" },
            { 91, "Índia" },
            { 49, "Alemanha" },
            { 33, "França" },
            { 39, "Itália" },
            { 81, "Japão" },  // Corrigido: removido o espaço extra
            { 61, "Austrália" },
            { 86, "China" },
            { 7, "Rússia" },
            { 34, "Espanha" },
            { 351, "Portugal" },
            { 52, "México" },
            { 64, "Nova Zelândia" },
            { 45, "Dinamarca" },
            { 47, "Noruega" },
            { 353, "Irlanda" },
            { 30, "Grécia" },
            { 43, "Áustria" },
            { 32, "Bélgica" },
            { 36, "Hungria" },
            { 420, "República Tcheca" },
            { 421, "Eslováquia" },
            { 386, "Eslovênia" },
            { 372, "Estônia" },
            { 370, "Lituânia" },
            { 371, "Letônia" },
            { 381, "Sérvia" },
            { 373, "Moldávia" },
            { 359, "Bulgária" }
        };

        public static string GetCountry(int countryCode)
        {
            return countryCodes.ContainsKey(countryCode) ? countryCodes[countryCode] : "Desconhecido";
        }
    }
}
