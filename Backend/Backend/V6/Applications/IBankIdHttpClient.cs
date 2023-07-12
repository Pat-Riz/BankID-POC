namespace Backend.V6.Applications
{
    public interface IBankIdHttpClient
    {
        Task<AuthResponse> Auth(AuthRequest req);
        Task<SignResponse> Sign(SignRequest req);
        Task<CollectResponse> Collect(CollectRequest req);
    }
}