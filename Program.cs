using System;
using PhoneNumbers;
using DataDial.CountryCodeLookup;
using DataDial.Helpers;
using System.Threading.Tasks; // Para usar Task

namespace PhoneValidator
{
    class Program
    {
        static async Task Main(string[] args) // Tornando o método Main assíncrono
        {
            Console.WriteLine(@"   ___       __       ___  _      __");
            Console.WriteLine(@"  / _ \___ _/ /____ _/ _ \(_)__ _/ /");
            Console.WriteLine(@" / // / _ `/ __/ _ `/ // / / _ `/ / ");
            Console.WriteLine(@"/____/\_,_/\__/\_,_/____/_/\_,_/_/  ");
            Console.WriteLine();

            bool continueRunning = true;

            while (continueRunning)
            {
                Console.ForegroundColor = ConsoleColor.White;

                var phoneUtil = PhoneNumberUtil.GetInstance();

                Console.Write("Digite o número de telefone com código do país (ex: +5511999999999): ");
                string rawNumber = Console.ReadLine();

                try
                {
                    PhoneNumber number = phoneUtil.Parse(rawNumber, null);
                    bool isValid = phoneUtil.IsValidNumber(number);

                    if (isValid)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nNúmero válido!");
                        Console.WriteLine($"Formato Internacional: {phoneUtil.Format(number, PhoneNumberFormat.INTERNATIONAL)}");
                        Console.WriteLine($"Código do País: {number.CountryCode}");
                        Console.WriteLine($"Número Nacional: {number.NationalNumber}");

                        string country = GetCountrySafe(number.CountryCode);
                        Console.WriteLine($"País: {country}");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("\n[INFO] Iniciando busca online...");
                        WebSearch.SearchOnlineWithDorks(rawNumber);

                        Console.WriteLine("\n[INFO] Verificando listas de spam...");
                        bool isSpam = await SpamChecker.CheckSpamList(rawNumber); // Usando await para chamada assíncrona

                        // Exibe o resultado da verificação de SPAM
                        Console.ForegroundColor = isSpam ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine(isSpam
                            ? "[ALERTA] O número foi identificado como SPAM!"
                            : "[INFO] O número não foi identificado como SPAM.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nNúmero inválido!");
                    }
                }
                catch (NumberParseException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nErro ao processar o número: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nUm erro inesperado ocorreu: {e.Message}");
                }

                // Pergunta ao usuário se deseja continuar apenas após processar o número
                Console.ResetColor();
                Console.Write("\nDeseja validar outro número? (s/n): ");
                string response = Console.ReadLine()?.Trim().ToLower();

                continueRunning = response == "s";

                if (continueRunning)
                {
                    Console.Clear();
                }
            }
        }

        private static string GetCountrySafe(int countryCode)
        {
            try
            {
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
