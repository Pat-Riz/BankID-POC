using Backend.V6.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Backend.V6.Applications
{
    public class BankIdHttpClient : IBankIdHttpClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<BankIdHttpClient> _logger;
        private readonly IOptions<ServiceOptions> _options;
        private readonly HttpClient _client;

        public const string CLIENT_NAME = "bankIdClient";

        public BankIdHttpClient(IHttpClientFactory factory, ILogger<BankIdHttpClient> logger, IOptions<ServiceOptions> options)
        {
            _factory = factory;
            _logger = logger;
            _options = options;
            _client = _factory.CreateClient(CLIENT_NAME);
        }


        public async Task<BankIdAuthResponse> Auth(AuthRequest req)
        {
            try
            {
                var res = await _client.PostAsync($"{_options.Value.BankIDUrlV6}/auth", SerializeStringContent(req));
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<BankIdAuthResponse>();
                return data!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<BankIdSignResponse> Sign(SignRequest req)
        {
            try
            {
                var res = await _client.PostAsync($"{_options.Value.BankIDUrlV6}/sign", SerializeStringContent(req));
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<BankIdSignResponse>();
                return data!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<BankIdCollectResponse> Collect(CollectRequest req)
        {
            try
            {
                var res = await _client.PostAsync($"{_options.Value.BankIDUrlV6}/collect", SerializeStringContent(req));
                res.EnsureSuccessStatusCode();
                var data = await res.Content.ReadFromJsonAsync<BankIdCollectResponse>();
                return data!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        private CustomStringContent SerializeStringContent<T>(T req)
        {
            var JSON = JsonSerializer.Serialize(req);
            var requestContent = new CustomStringContent(content: JSON, encoding: Encoding.UTF8);
            return requestContent;
        }
    }
}
