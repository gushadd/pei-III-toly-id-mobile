﻿using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using TolyID.DTO;
using TolyID.MVVM.Models;

namespace TolyID.Services.Api
{
    public class CadastrarCapturaApi
    {
        public async Task CadastrarCaptura(CapturaDTO capturaDTO,Captura captura) 
        {
            //string json = JsonConvert.SerializeObject(captura, Formatting.Indented);

            try
            {
                string url = $"http://172.20.10.8:8080/capturas/cadastrar/1";
                string token = await GerarToken.Gerar();

                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("Token inválido.");
                    return;
                }

                var content = new StringContent(JsonConvert.SerializeObject(capturaDTO), Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    // VERIFICAR SE DTO ESTÁ PREENCHIDO
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        captura.FoiEnviadoParaApi = true;
                        var banco = new CapturaService();
                        await banco.AtualizaCaptura(captura);
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Erro: {response.StatusCode} - {errorMessage}");
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
