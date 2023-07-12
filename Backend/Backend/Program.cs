using Backend.V6.Applications;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBankIdHttpClient, BankIdHttpClient>();
builder.Services.AddScoped<IQRCodeGenerator, QRCodeGenerator>();
builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection("ServiceOptions"));
builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));


X509Certificate2 certificate = new X509Certificate2("C:\\Work\\BankIdPoc\\Backend\\FPTestcert4_20230629.p12", "qwerty123");

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
                       handler.ClientCertificates.Add(certificate);
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
    var qrCode = qrCodeGenerator.GenerateQrCode(res.qrStartToken, res.qrStartSecret, DateTime.Now);

    return TypedResults.Ok(new { orderRef = res.orderRef, qrCode = qrCode });
})
.WithOpenApi();


app.MapGet("/sign", (IBankIdHttpClient client) =>
{
    client.Sign(new SignRequest() { endUserIp = "172.17.208.75", userNonVisibleData = "JOHAN" });

    return TypedResults.Ok();
})
.WithOpenApi();

app.MapGet("/collect", async (IBankIdHttpClient client, string _orderRef) =>
{
    var res = await client.Collect(new CollectRequest() { orderRef = _orderRef });

    return TypedResults.Ok(res);
})
.WithOpenApi();

app.Run();

