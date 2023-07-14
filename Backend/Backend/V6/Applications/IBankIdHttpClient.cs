using Backend.V6.Models;

namespace Backend.V6.Applications
{
    public interface IBankIdHttpClient
    {
        Task<BankIdAuthResponse> Auth(AuthRequest req);
        Task<BankIdSignResponse> Sign(SignRequest req);
        Task<BankIdCollectResponse> Collect(CollectRequest req);
    }
}