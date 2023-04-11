using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using Integration.EskomSePush.Models.Response;

namespace Integration.EskomSePush
{
    public class Client : IDisposable
    {

        private HttpClient _httpClient;

        public Client(string espLicenceKey) 
        {
            Ensure.That(espLicenceKey).IsNotNullOrEmpty();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://developer.sepush.co.za/business/2.0/");
            _httpClient.DefaultRequestHeaders.Add("Token", espLicenceKey);
        }

        public StatusResponse? GetStatus(string id)
        {
            var task = InternalGetAsync<StatusResponse>("status");

            Task.WaitAll(task);

            if (task.IsCompleted && task.Result.success)
            {
                return task.Result.model!;
            }

            return null;
        }

        private async Task<(bool success, TModel? model)> InternalGetAsync<TModel>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                return (false, default);
            }

            var model = await response.Content.ReadFromJsonAsync<TModel>();
            return (true, model);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
