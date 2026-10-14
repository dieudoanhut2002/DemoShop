namespace MyShop.Services
{
    public interface IVietQrService
    {
        string BankId { get; }
        string BankName { get; }
        string AccountNo { get; }
        string AccountName { get; }

        string GenerateVietQrImageUrl(decimal amount, string orderCode);
        string GenerateFallbackQrBase64(string payload);
    }
}
