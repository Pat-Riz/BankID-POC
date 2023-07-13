using Backend.V6.Applications;
using Models;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBankIdHttpClient, BankIdHttpClient>();
builder.Services.AddSingleton<IQRCodeGenerator, QRCodeGenerator>();
builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection("ServiceOptions"));
builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));

builder.Services.AddHttpClient(BankIdHttpClient.CLIENT_NAME)
                .ConfigureHttpClient(client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .ConfigurePrimaryHttpMessageHandler(
                   () =>
                   {
                       var handler = new HttpClientHandler();
                       handler.ClientCertificates.Add(new X509Certificate2(builder.Configuration.GetSection("ServiceOptions")["BankIDCertPath"]!, "qwerty123"));
                       //handler.ClientCertificates.Add(X509CertificateHelper.GetCertificate(builder.Configuration));

                       handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                       return handler;
                   }
                );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.MapGet("/auth", async (IBankIdHttpClient client, IQRCodeGenerator qrCodeGenerator) =>
{
    var res = await client.Auth(new AuthRequest() { endUserIp = "172.17.208.75" });
    var qrCode = qrCodeGenerator.GenerateQrCode(res.orderRef, res.qrStartToken, res.qrStartSecret, DateTime.Now);

    return TypedResults.Ok(new AuthResponse() { qrCode = qrCode, orderRef = res.orderRef });
})
.WithOpenApi();


app.MapGet("/sign", (IBankIdHttpClient client) =>
{
    client.Sign(new SignRequest() { endUserIp = "172.17.208.75", userVisibleData = "IFRoaXMgaXMgYSBzYW1wbGUgdGV4dCB0byBiZSBzaWduZWQ=" });

    return TypedResults.Ok();
})
.WithOpenApi();

app.MapPost("/collect", async (IBankIdHttpClient client, CollectRequest req, IQRCodeGenerator generator) =>
{
    var res = await client.Collect(req);
    var collectRes = new CollectResponse
    {
        status = res.status,
        hintCode = res.hintCode,
        completionData = res.completionData,
        qrCode = generator.UpdateQRCode(req.orderRef)
    };
    return TypedResults.Ok(collectRes);
})
.WithOpenApi();

app.Run();

public record AuthResponse
{
    public string qrCode { get; set; }
    public string orderRef { get; set; }
}

public record CollectResponse
{

    public string qrCode { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
    public string? hintCode { get; set; }
    public CompletionData? completionData { get; set; }
}