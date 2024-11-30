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

        public static async Task CheckSpamList(string phoneNumber)
        {
            var (accountSid, authToken) = GetTwilioCredentials();
            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken))
            {
                Console.WriteLine("[ERRO] As credenciais da Twilio n�o foram encontradas.");
                return;
            }

            try
            {
                var requestUrl = string.Format(TwilioApiUrl, phoneNumber);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                requestMessage.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{accountSid}:{authToken}")));

                var response = await client.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                if (IsSpam(data))
                {
                    Console.WriteLine($"[ALERTA] O n�mero {phoneNumber} foi identificado como possivelmente SPAM.");
                }
                else
                {
                    Console.WriteLine($"[INFO] O n�mero {phoneNumber} n�o foi identificado como SPAM.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] N�o foi poss�vel verificar o n�mero {phoneNumber}: {ex.Message}");
            }
        }

        private static bool IsSpam(dynamic data)
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

            return false;
        }

        private static (string, string) GetTwilioCredentials()
        {
            if (File.Exists(ConfigFilePath))
            {
                var lines = File.ReadAllLines(ConfigFilePath);
                if (lines.Length >= 2)
                {
                    return (lines[0], lines[1]);
                }
            }

            Console.WriteLine("Voc� gostaria de verificar se um n�mero � SPAM?");
            Console.WriteLine("Aviso: Ser� necess�rio fornecer o SID e o Token da sua conta Twilio para realizar a verifica��o.");
            Console.WriteLine("Digite 'sim/s', 'n�o/n' para continuar:");

            string response = Console.ReadLine()?.ToLower();
            if (string.Equals(response, "sim") || string.Equals(response, "s"))
            {
                Console.WriteLine("SID da Twilio n�o encontrado. Por favor, forne�a o SID da sua conta Twilio:");
                var userSid = Console.ReadLine();

                Console.WriteLine("Token da Twilio n�o encontrado. Por favor, forne�a o Token da sua conta Twilio:");
                var userToken = Console.ReadLine();

                File.WriteAllLines(ConfigFilePath, new[] { userSid, userToken });

                return (userSid, userToken);
            }
            else if (string.Equals(response, "n�o") || string.Equals(response, "n"))
            {
                Console.WriteLine("Opera��o cancelada. Nenhuma verifica��o realizada.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Resposta inv�lida. Por favor, responda com 'sim', 's', 'n�o' ou 'n'.");
                return GetTwilioCredentials();
            }

            return (string.Empty, string.Empty);
        }
    }
}
