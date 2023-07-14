using Backend;
using Backend.V6.Applications;
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
                       var options = new ServiceOptions();
                       builder.Configuration.GetSection("ServiceOptions").Bind(options);

                       var handler = new HttpClientHandler();
                       handler.ClientCertificates.Add(new X509Certificate2(options.BankIDCertPath, options.BankIdCertPassword));

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

app.MapEndpoints();

app.Run();
