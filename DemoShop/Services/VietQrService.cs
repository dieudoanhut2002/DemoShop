using QRCoder;

namespace MyShop.Services
{
    public class VietQrService : IVietQrService
    {
        private readonly IConfiguration _configuration;

        public VietQrService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BankId => _configuration["PaymentSettings:BankId"] ?? "MB";
        public string BankName => _configuration["PaymentSettings:BankName"] ?? "Ngân hàng TMCP Quân Đội (MBBank)";
        public string AccountNo => _configuration["PaymentSettings:AccountNo"] ?? "0987654321";
        public string AccountName => _configuration["PaymentSettings:AccountName"] ?? "THOI TRANG NGOC HIEN";

        public string GenerateVietQrImageUrl(decimal amount, string orderCode)
        {
            long intAmount = (long)Math.Round(amount);
            string sanitizedCode = Uri.EscapeDataString(orderCode.Trim());
            string encodedName = Uri.EscapeDataString(AccountName.Trim());

            // URL chuẩn VietQR Template compact2
            return $"https://img.vietqr.io/image/{BankId}-{AccountNo}-compact2.png?amount={intAmount}&addInfo={sanitizedCode}&accountName={encodedName}";
        }

        public string GenerateFallbackQrBase64(string payload)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(15);
            return $"data:image/png;base64,{Convert.ToBase64String(qrCodeAsPngByteArr)}";
        }
    }
}
