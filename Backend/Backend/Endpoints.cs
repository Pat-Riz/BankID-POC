using Backend.V6.Applications;
using Backend.V6.Models;

namespace Backend
{
    public static class Endpoints
    {
        const string TEST_IP = "172.17.208.75";

        public static WebApplication MapEndpoints(this WebApplication app)
        {

            app.MapPost("/auth", async (IBankIdHttpClient client, IQRCodeGenerator qrCodeGenerator, AuthRequest req) =>
            {
                var res = await client.Auth(req);
                var qrCode = qrCodeGenerator.GenerateQrCode(res.orderRef, res.qrStartToken, res.qrStartSecret, DateTime.Now);

                return TypedResults.Ok(new AuthResponse() { qrCode = qrCode, orderRef = res.orderRef });
            }).WithOpenApi();


            app.MapPost("/collect", async (IBankIdHttpClient client, CollectRequest req, IQRCodeGenerator generator) =>
            {
                var res = await client.Collect(req);

                if (res.status == "complete" || res.status == "failed")
                {
                    generator.RemoveQRCode(res.orderRef);
                }

                var collectRes = new CollectResponse
                {
                    status = res.status,
                    hintCode = res.hintCode,
                    completionData = res.completionData,
                    qrCode = generator.UpdateQRCode(req.orderRef)
                };
                return TypedResults.Ok(collectRes);
            }).WithOpenApi();

            return app;
        }

        //app.MapGet("/sign", (IBankIdHttpClient client) =>
        //{
        //    client.Sign(new SignRequest() { endUserIp = TEST_IP, userVisibleData = "IFRoaXMgaXMgYSBzYW1wbGUgdGV4dCB0byBiZSBzaWduZWQ=" });

        //    return TypedResults.Ok();
        //}).WithOpenApi();
    }
}
