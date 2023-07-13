﻿using Microsoft.Extensions.Options;
using Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                var res = await _client.PostAsync($"{_options.Value.BankIDUrlV6}/auth", SerializeCustomStringContent(req));
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
                var res = await _client.PostAsync($"{_options.Value.BankIDUrlV6}/sign", SerializeCustomStringContent(req));
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
                var res = await _client.PostAsync($"{_options.Value.BankIDUrlV6}/collect", SerializeCustomStringContent(req));
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

        private CustomStringContent SerializeCustomStringContent<T>(T req)
        {
            var JSON = JsonSerializer.Serialize(req);
            var requestContent = new CustomStringContent(content: JSON, encoding: Encoding.UTF8);
            return requestContent;
        }
    }

    public record AuthRequest
    {
        public string endUserIp { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Requirment? requirment { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userVisibleData { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userNonVisibleData { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userVisibleDataFormat { get; set; }
    }
    public record Requirment
    {

        public bool pinCode { get; set; }
        public bool mrtd { get; set; }
        public string? cardReader { get; set; }
        public IEnumerable<string>? certificatePolicies { get; set; }
        public string? personalNumber { get; set; }
    }

    public record BankIdAuthResponse
    {
        public string orderRef { get; set; } = string.Empty;
        public string autoStartToken { get; set; } = string.Empty;
        public string qrStartToken { get; set; } = string.Empty;
        public string qrStartSecret { get; set; } = string.Empty;

    }

    public record SignRequest
    {
        public string endUserIp { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Requirment? requirment { get; set; }
        public string userVisibleData { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userNonVisibleData { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userVisibleDataFormat { get; set; }
    }

    public record BankIdSignResponse
    {
        public string orderRef { get; set; } = string.Empty;
        public string autoStartToken { get; set; } = string.Empty;
        public string qrStartToken { get; set; } = string.Empty;
        public string qrStartSecret { get; set; } = string.Empty;
    }

    public record CollectRequest
    {
        public string orderRef { get; set; } = string.Empty;
    }
    public record BankIdCollectResponse
    {
        public string orderRef { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string? hintCode { get; set; }
        public CompletionData? completionData { get; set; }
    }

    public record CancelRequest
    {
        public string orderRef { get; set; } = string.Empty;
    }
}
