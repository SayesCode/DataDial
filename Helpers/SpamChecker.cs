using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataDial.Helpers
{
    public static class SpamChecker
    {
        private const string ConfigFilePath = "config.txt";
        private const string TwilioApiUrl = "https://lookups.twilio.com/v1/PhoneNumbers/{0}";
        private static readonly HttpClient client = new HttpClient();

        public static async Task<bool> CheckSpamList(string phoneNumber)  // Mudança para retornar bool
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                Console.WriteLine("[ERRO] Número de telefone inválido ou vazio.");
                return false;  // Retorna false se o número for inválido
            }

            var (accountSid, authToken) = GetTwilioCredentials();
            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken))
            {
                Console.WriteLine("[ERRO] As credenciais da Twilio não foram encontradas.");
                return false;  // Retorna false se as credenciais forem inválidas
            }

            try
            {
                var requestUrl = string.Format(TwilioApiUrl, phoneNumber);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                requestMessage.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{accountSid}:{authToken}")));

                var response = await client.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ERRO] Não foi possível consultar o número {phoneNumber}. Código HTTP: {response.StatusCode}");
                    return false;  // Retorna false se a consulta falhar
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                Console.WriteLine("\n[INFO] Resultados da consulta:");
                DisplayTwilioData(data);

                bool isSpam = IsSpam(data);  // Determina se é SPAM

                if (isSpam)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ALERTA] O número {phoneNumber} foi identificado como possivelmente SPAM.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[INFO] O número {phoneNumber} não foi identificado como SPAM.");
                }
                Console.ResetColor();

                return isSpam;  // Retorna se o número é SPAM ou não
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ERRO] Problema na solicitação HTTP: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Ocorreu um erro: {ex.Message}");
                return false;
            }
        }

        private static void DisplayTwilioData(dynamic data)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine($"\n- Número Nacional: {data?.national_format ?? "Não disponível"}");
            Console.WriteLine($"- Número Internacional: {data?.phone_number ?? "Não disponível"}");
            Console.WriteLine($"- Código do País: {data?.country_code ?? "Não disponível"}");

            var carrier = data?.carrier;
            if (carrier != null)
            {
                Console.WriteLine($"- Operadora: {carrier?.name ?? "Não disponível"}");
                Console.WriteLine($"- Tipo: {carrier?.type ?? "Não disponível"}");
            }

            Console.WriteLine($"\nDados brutos (compactados para fins de log):\n{JsonConvert.SerializeObject(data, Formatting.Indented)}");
        }

        private static bool IsSpam(dynamic data)
        {
            try
            {
                if (data?.carrier != null)
                {
                    var carrierName = data.carrier?.name?.ToString()?.ToLower() ?? string.Empty;
                    var carrierType = data.carrier?.type?.ToString()?.ToLower() ?? string.Empty;

                    if (carrierName.Contains("spam") || carrierType.Contains("voip") || carrierType.Contains("virtual"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[ERRO] Não foi possível verificar os dados de SPAM. Resposta inesperada.");
            }

            return false;
        }

        private static (string, string) GetTwilioCredentials()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var lines = File.ReadAllLines(ConfigFilePath);
                    if (lines.Length >= 2)
                    {
                        return (lines[0] ?? string.Empty, lines[1] ?? string.Empty);
                    }
                }

                Console.WriteLine("Você gostaria de verificar se um número é SPAM?");
                Console.WriteLine("Aviso: Será necessário fornecer o SID e o Token da sua conta Twilio para realizar a verificação.");
                Console.WriteLine("Digite 'sim/s' para continuar ou 'não/n' para cancelar:");

                string response = Console.ReadLine()?.ToLower() ?? string.Empty;
                if (response == "sim" || response == "s")
                {
                    Console.WriteLine("Por favor, insira o SID da sua conta Twilio:");
                    var userSid = Console.ReadLine() ?? string.Empty;

                    Console.WriteLine("Por favor, insira o Token da sua conta Twilio:");
                    var userToken = Console.ReadLine() ?? string.Empty;

                    File.WriteAllLines(ConfigFilePath, new[] { userSid, userToken });

                    return (userSid ?? string.Empty, userToken ?? string.Empty);
                }

                Console.WriteLine("Operação cancelada.");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Não foi possível configurar as credenciais: {ex.Message}");
            }

            return (string.Empty, string.Empty);
        }
    }
}
