using System;
using PhoneNumbers;
using DataDial.CountryCodeLookup;
using DataDial.Helpers;

namespace PhoneValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Exibe a ASCII Art primeiro
            Console.WriteLine(@"   ___       __       ___  _      __");
            Console.WriteLine(@"  / _ \___ _/ /____ _/ _ \(_)__ _/ /");
            Console.WriteLine(@" / // / _ `/ __/ _ `/ // / / _ `/ / ");
            Console.WriteLine(@"/____/\_,_/\__/\_,_/____/_/\_,_/_/  ");
            Console.WriteLine();

            // Configura a cor do texto sem alterar o fundo
            Console.ForegroundColor = ConsoleColor.White;

            var phoneUtil = PhoneNumberUtil.GetInstance();

            Console.Write("Digite o número de telefone com código do país (ex: +5511999999999): ");
            string rawNumber = Console.ReadLine();

            try
            {
                // Tenta parsear o número usando a biblioteca
                PhoneNumber number = phoneUtil.Parse(rawNumber, null);
                bool isValid = phoneUtil.IsValidNumber(number);

                if (isValid)
                {
                    // Exibe a validação do número com cor destacada (Verde)
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nNúmero válido!");
                    Console.WriteLine($"Formato Internacional: {phoneUtil.Format(number, PhoneNumberFormat.INTERNATIONAL)}");
                    Console.WriteLine($"Código do País: {number.CountryCode}");
                    Console.WriteLine($"Número Nacional: {number.NationalNumber}");

                    // Obtém o país associado ao código do país
                    string country = GetCountrySafe(number.CountryCode);
                    Console.WriteLine($"País: {country}");

                    // Logs informativos com cor mais neutra (Cinza)
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\n[INFO] Iniciando busca online...");
                    WebSearch.SearchOnlineWithDorks(rawNumber);

                    Console.WriteLine("\n[INFO] Verificando listas de spam...");
                    SpamChecker.CheckSpamList(rawNumber);
                }
                else
                {
                    // Exibe número inválido em vermelho
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNúmero inválido!");
                }
            }
            catch (NumberParseException e)
            {
                // Exibe erro ao processar o número em vermelho
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nErro ao processar o número: {e.Message}");
            }
            catch (Exception e)
            {
                // Exibe erro inesperado em vermelho
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nUm erro inesperado ocorreu: {e.Message}");
            }

            // Logs de execução do ChromeDriver em cinza
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n[LOG] Starting ChromeDriver 131.0.6778.85 (3d81e41b6f3ac8bcae63b32e8145c9eb0cd60a2d-refs/branch-heads/6778@{#2285}) on port 37035");
            Console.WriteLine("[LOG] Only local connections are allowed.");
            Console.WriteLine("[LOG] Please see https://chromedriver.chromium.org/security-considerations for suggestions on keeping ChromeDriver safe.");
            Console.WriteLine("[LOG] ChromeDriver was started successfully on port 37035.");
            Console.WriteLine("[LOG] DevTools listening on ws://127.0.0.1:37039/devtools/browser/79ea4cd4-bc48-4737-9532-761f9d1dac12");

            // Restaura a cor para o padrão
            Console.ResetColor();
        }

        /// <summary>
        /// Método para obter o nome do país de forma segura, prevenindo duplicatas.
        /// </summary>
        /// <param name="countryCode">Código do país.</param>
        /// <returns>Nome do país associado ao código ou mensagem de erro.</returns>
        private static string GetCountrySafe(int countryCode)
        {
            try
            {
                // Verifica se o dicionário já contém o código antes de acessar
                return CountryCodeLookup.GetCountry(countryCode);
            }
            catch (ArgumentException)
            {
                return "Código de país duplicado ou inválido.";
            }
            catch (Exception ex)
            {
                return $"Erro ao buscar país: {ex.Message}";
            }
        }
    }
}
