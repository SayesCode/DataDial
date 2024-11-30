using System.Collections.Generic;

namespace DataDial.CountryCodeLookup
{
    public static class CountryCodeLookup
    {
        private static readonly Dictionary<int, string> countryCodes = new()
        {
            { 55, "Brasil" },
            { 1, "Estados Unidos/Canad�" },
            { 44, "Reino Unido" },
            { 91, "�ndia" },
            { 49, "Alemanha" },
            { 33, "Fran�a" },
            { 39, "It�lia" },
            { 81, "Jap�o" },  // Corrigido: removido o espa�o extra
            { 61, "Austr�lia" },
            { 86, "China" },
            { 7, "R�ssia" },
            { 34, "Espanha" },
            { 351, "Portugal" },
            { 52, "M�xico" },
            { 64, "Nova Zel�ndia" },
            { 45, "Dinamarca" },
            { 47, "Noruega" },
            { 353, "Irlanda" },
            { 30, "Gr�cia" },
            { 43, "�ustria" },
            { 32, "B�lgica" },
            { 36, "Hungria" },
            { 420, "Rep�blica Tcheca" },
            { 421, "Eslov�quia" },
            { 386, "Eslov�nia" },
            { 372, "Est�nia" },
            { 370, "Litu�nia" },
            { 371, "Let�nia" },
            { 381, "S�rvia" },
            { 373, "Mold�via" },
            { 359, "Bulg�ria" }
        };

        public static string GetCountry(int countryCode)
        {
            return countryCodes.ContainsKey(countryCode) ? countryCodes[countryCode] : "Desconhecido";
        }
    }
}
