using System.Net.Http.Json;
using EnsureThat;
using Integration.EskomSePush.Models.Responses;

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

        public StatusResponse? GetStatus()
        {
            return (StatusResponse?)GetResponse<StatusResponse>("status");
        }

        public AllowanceResponse? GetAllowance()
        {
            return (AllowanceResponse?)GetResponse<AllowanceResponse>("api_allowance");
        }

        public AreaResponse? GetArea(string id)
        {
            return (AreaResponse?)GetResponse<AreaResponse>("area", id);
        }

        private Response? GetResponse<TResponse>(string path, string? id = null) 
            where TResponse : Response
        {
            if (id is not null)
            {
                path = path.ToQueryString(new System.Collections.Specialized.NameValueCollection { { "id", id } });
            }

            var task = InternalGetAsync<StatusResponse>(path);

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
