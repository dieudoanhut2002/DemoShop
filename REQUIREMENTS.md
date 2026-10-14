# TÀI LIỆU YÊU CẦU & THIẾT KẾ HỆ THỐNG (SYSTEM REQUIREMENTS & SPECIFICATIONS)
## HỆ THỐNG WEBSITE THƯƠNG MẠI ĐIỆN TỬ THỜI TRANG NGỌC HIỂN

---

### 1. TỔNG QUAN DỰ ÁN
- **Tên dự án**: Website Bán hàng Thời trang Ngọc Hiển (Ngoc Hien Fashion E-commerce)
- **Thương hiệu**: Hãng Thời trang Ngọc Hiển
- **Sản phẩm chủ lực**:
  1. **Áo Sơ Mi Nam/Nữ**: Sơ mi cộc tay vải thông hơi, sơ mi cộc kẻ sợi tre, sơ mi lụa vân cao cấp, sơ mi dài tay công sở trơn/kẻ sọc, cúc khuy đen sang trọng,...
  2. **Áo Rét / Áo Khoác Mùa Đông**: Áo phao siêu nhẹ cản gió chống nước, áo chần bông ấm áp, áo bomber phong cách, áo khoác đông thời thượng,...
- **Công nghệ nền tảng**:
  - ASP.NET Core 8.0 MVC (C# .NET 8)
  - Entity Framework Core 8.0 (Code First với SQL Server / LocalDB)
  - ASP.NET Core Identity (Quản lý tài khoản, Phân quyền Admin / Customer)
  - Giao diện: Bootstrap 5.3, Bootstrap Icons & FontAwesome 6, SweetAlert2, Google Fonts (Be Vietnam Pro / Inter)
  - Tích hợp thanh toán: VietQR Standard (Napas 247) sinh mã QR động theo từng đơn hàng + Hỗ trợ QRCoder offline fallback.

---

### 2. PHÂN HỆ YÊU CẦU CHỨC NĂNG (FUNCTIONAL REQUIREMENTS)

#### 2.1. Phân hệ Khách hàng (Storefront)
1. **Trang chủ (Homepage)**:
   - Banner Hero quảng bá bộ sưu tập thời trang Ngọc Hiển (Áo sơ mi & Áo rét).
   - Bộ sưu tập sản phẩm nổi bật (Featured Products).
   - Bộ sưu tập Áo Sơ Mi (Shirt Collection) & Áo Rét Mùa Đông (Winter Coat Collection).
   - Cam kết thương hiệu: Vải sồi tre thoáng khí, bông lông vũ ấm áp, may đo tỉ mỉ, giao hàng hỏa tốc, đổi trả 7 ngày.
   - Tin tức & mẹo thời trang mới nhất.
2. **Danh mục & Tìm kiếm Sản phẩm (Catalog & Search)**:
   - Xem theo danh mục: "Tất cả", "Áo sơ mi", "Áo rét".
   - Bộ lọc đa tiêu chí: Lọc theo khoảng giá, lọc theo size (M, L, XL, 2XL, 3XL), sắp xếp (Mới nhất, Giá tăng dần, Giá giảm dần, Tên A-Z).
   - Tìm kiếm nhanh theo từ khóa tên hoặc mô tả sản phẩm.
3. **Trang Chi tiết Sản phẩm (Product Detail)**:
   - Thư viện hình ảnh sản phẩm chất lượng cao (lấy từ kho ảnh `Images`).
   - Lựa chọn Size (M, L, XL, 2XL, 3XL) và Màu sắc (Trắng, Đen, Xanh biển, Kẻ caro, Ghi xám, Rêu, Navy,...).
   - Hiển thị tình trạng tồn kho, mã sản phẩm (SKU).
   - Nút "Thêm vào giỏ hàng" và "Mua ngay".
   - Bảng hướng dẫn chọn size chuẩn cho người Việt.
   - Sản phẩm liên quan (cùng danh mục).
4. **Giỏ hàng (Shopping Cart)**:
   - Thêm sản phẩm kèm size/màu sắc vào giỏ hàng (sử dụng Session an toàn).
   - Điều chỉnh số lượng, xóa từng sản phẩm hoặc làm mới giỏ hàng.
   - Tự động tính tổng tiền và phí vận chuyển (Miễn phí ship cho đơn từ 500.000 VNĐ).
5. **Thanh toán & Đặt hàng (Checkout)**:
   - Nhập thông tin người nhận: Họ tên, Số điện thoại, Địa chỉ giao hàng chi tiết, Tỉnh/Thành phố, Ghi chú đơn hàng.
   - Lựa chọn 2 phương thức thanh toán:
     + **Thanh toán quét mã QR (VietQR / Ngân hàng / MoMo / VNPay)**: Tự động chuyển đến trang quét mã QR thanh toán chuẩn liên ngân hàng Napas 247.
     + **Thanh toán khi nhận hàng (COD)**: Trả tiền mặt khi giao hàng.
6. **Trang Xác nhận & Quét mã QR Thanh toán (Order Confirmation & QR Pay)**:
   - Hiển thị mã đơn hàng dạng `#NHxxxx`.
   - Sinh mã VietQR động trực quan có logo Napas247/Ngân hàng: chứa đầy đủ Tên ngân hàng, Số tài khoản, Tên chủ tài khoản "THOI TRANG NGOC HIEN", Số tiền chính xác, Nội dung chuyển khoản theo cú pháp `NH<IdDonHang>`.
   - Tích hợp nút "Sao chép số tài khoản", "Sao chép số tiền", "Sao chép nội dung chuyển khoản", "Tải mã QR về máy".
   - Cho phép khách hàng tải lên/ghi chú ảnh ủy nhiệm chi hoặc xác nhận đã chuyển khoản.
7. **Tra cứu Đơn hàng (Order Tracking)**:
   - Cho phép khách tra cứu tiến độ đơn hàng nhanh bằng Mã đơn hàng hoặc Số điện thoại.
   - Hiển thị timeline xử lý: Chờ xác nhận -> Đã xác nhận -> Đang đóng gói & giao hàng -> Hoàn thành.
8. **Chuyên mục Bài viết / Blog Thời trang (Blog & Fashion Articles)**:
   - Danh sách bài viết: Mẹo phối áo sơ mi công sở, Cách chọn áo rét giữ ấm mùa đông, Hướng dẫn bảo quản chất liệu sợi tre / lông vũ,...
   - Đọc chi tiết bài viết với hình ảnh, chia sẻ mạng xã hội.

---

#### 2.2. Phân hệ Quản trị (Admin Dashboard & CMS)
1. **Bảo mật & Phân quyền (Authentication & Authorization)**:
   - Đăng nhập tài khoản quản trị (Role: `Admin`).
   - Mặc định: `admin@ngochien.vn` / `Admin@123`.
2. **Bảng điều khiển (Admin Dashboard)**:
   - Thống kê tổng quan: Doanh thu thực tế, Số đơn hàng mới, Tổng số sản phẩm, Số bài viết.
   - Thống kê tỷ lệ đơn theo trạng thái (Chờ xác nhận, Đã hoàn thành, Đã hủy).
   - Danh sách đơn hàng mới nhất cần xử lý.
3. **Quản lý Sản phẩm (Product Management)**:
   - Danh sách sản phẩm dạng bảng (Ảnh đại diện, Tên, Danh mục, Giá gốc, Giá bán, Tồn kho, Nổi bật, Thao tác).
   - Thêm sản phẩm mới: Tên sản phẩm, Danh mục, Giá bán, Giá gốc (khuyến mãi), Kích thước hỗ trợ, Màu sắc, Chất liệu, Tồn kho, Tải ảnh mới hoặc chọn ảnh từ thư viện Images có sẵn.
   - Cập nhật thông tin sản phẩm và xóa sản phẩm.
4. **Quản lý Danh mục (Category Management)**:
   - Thêm/sửa/xóa danh mục sản phẩm (Áo sơ mi dài tay, Áo sơ mi cộc tay, Áo phao lông vũ, Áo khoác gió rét,...).
5. **Quản lý Đơn hàng (Order Management)**:
   - Danh sách toàn bộ đơn hàng của khách với bộ lọc theo trạng thái đơn & trạng thái thanh toán.
   - Xem chi tiết đơn hàng: Thông tin khách nhận, danh sách sản phẩm (tên, size, số lượng, đơn giá, thành tiền), phương thức thanh toán, ghi chú.
   - Cập nhật trạng thái đơn hàng (Chờ xác nhận, Đang xử lý, Đang giao hàng, Đã hoàn thành, Đã hủy).
   - Cập nhật trạng thái thanh toán (Chưa thanh toán, Đã thanh toán, Hoàn tiền).
   - In hóa đơn / phiếu giao hàng đơn giản.
6. **Quản lý Bài viết (Article / Blog Management)**:
   - Danh sách bài viết tin tức & thời trang.
   - Thêm mới bài viết: Tiêu đề, Slug, Tóm tắt ngắn, Nội dung bài viết (HTML), Ảnh đại diện, Tác giả, Trạng thái (Hiển thị / Nháp).
   - Sửa và xóa bài viết.

---

### 3. THIẾT KẾ CƠ SỞ DỮ LIỆU (DATABASE SCHEMA)

1. **Categories (Danh mục)**:
   - `Id` (int, PK)
   - `Name` (nvarchar(100))
   - `Slug` (nvarchar(100))
   - `Description` (nvarchar(500))
   - `DisplayOrder` (int)
   - `IsActive` (bool)

2. **Products (Sản phẩm)**:
   - `Id` (int, PK)
   - `CategoryId` (int, FK)
   - `Name` (nvarchar(250))
   - `Slug` (nvarchar(250))
   - `Description` (nvarchar(max))
   - `Price` (decimal(18,2))
   - `OriginalPrice` (decimal(18,2), nullable)
   - `ImageUrl` (nvarchar(500))
   - `MoreImages` (nvarchar(max), JSON/chuỗi các ảnh bổ sung)
   - `Sizes` (nvarchar(100), ví dụ: "M, L, XL, XXL, 3XL")
   - `Colors` (nvarchar(100), ví dụ: "Trắng, Đen, Xanh navy")
   - `Material` (nvarchar(200), ví dụ: "Vải sợi tre cao cấp / Bông lông vũ 3 lớp")
   - `StockQuantity` (int)
   - `IsFeatured` (bool)
   - `CreatedAt` (datetime)
   - `IsActive` (bool)

3. **Orders (Đơn hàng)**:
   - `Id` (int, PK)
   - `OrderCode` (nvarchar(50), ví dụ: "NH-20261014-001")
   - `CustomerName` (nvarchar(150))
   - `CustomerPhone` (nvarchar(30))
   - `CustomerEmail` (nvarchar(150), nullable)
   - `ShippingAddress` (nvarchar(300))
   - `City` (nvarchar(100))
   - `Notes` (nvarchar(500), nullable)
   - `TotalAmount` (decimal(18,2))
   - `ShippingFee` (decimal(18,2))
   - `PaymentMethod` (nvarchar(50), ví dụ: "VietQR", "COD")
   - `PaymentStatus` (nvarchar(50), "Pending", "Paid", "Refunded")
   - `OrderStatus` (nvarchar(50), "Pending", "Processing", "Shipping", "Delivered", "Cancelled")
   - `CreatedAt` (datetime)
   - `PaidAt` (datetime, nullable)

4. **OrderItems (Chi tiết đơn hàng)**:
   - `Id` (int, PK)
   - `OrderId` (int, FK)
   - `ProductId` (int, FK)
   - `ProductName` (nvarchar(250))
   - `ProductImage` (nvarchar(500))
   - `SelectedSize` (nvarchar(50))
   - `SelectedColor` (nvarchar(50))
   - `UnitPrice` (decimal(18,2))
   - `Quantity` (int)
   - `TotalPrice` (decimal(18,2))

5. **Articles (Bài viết tin tức & thời trang)**:
   - `Id` (int, PK)
   - `Title` (nvarchar(250))
   - `Slug` (nvarchar(250))
   - `Summary` (nvarchar(500))
   - `Content` (nvarchar(max))
   - `ThumbnailUrl` (nvarchar(500))
   - `Author` (nvarchar(100))
   - `PublishedAt` (datetime)
   - `IsPublished` (bool)
   - `ViewCount` (int)

6. **Identity Tables**: `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`,... (Quản lý User & Admin).

---

### 4. ĐẶC TẢ TÍCH HỢP THANH TOÁN VIETQR (VIETNAMESE QR PAYMENT SPECIFICATION)

- **Cơ chế hoạt động**:
  1. Khi khách hàng bấm Đặt hàng chọn hình thức "Thanh toán quét mã QR (VietQR / Chuyển khoản ngân hàng)".
  2. Hệ thống lưu đơn hàng và chuyển hướng sang trang `/Order/PaymentSuccess/{orderId}`.
  3. Hệ thống tạo mã QR động theo chuẩn VietQR NAPAS 247:
     `https://img.vietqr.io/image/{BANK_ID}-{ACCOUNT_NO}-compact2.png?amount={AMOUNT}&addInfo={ORDER_CODE}&accountName={ACCOUNT_NAME}`
  4. Dự phòng QR Code: Sử dụng thư viện `QRCoder` sinh mã QR nội bộ chứa thông tin chuyển khoản nhanh.
  5. Cung cấp nút sao chép Số tài khoản, Số tiền, Mã nội dung chuyển khoản để khách dễ dàng dán vào ứng dụng ngân hàng di động bất kỳ (Vietcombank, MB Bank, Techcombank, BIDV, MoMo, VNPay,...).

- **Cấu hình tham số trong `appsettings.json`**:
```json
"PaymentSettings": {
  "BankId": "MB",
  "AccountNo": "0987654321",
  "AccountName": "THOI TRANG NGOC HIEN",
  "BankName": "Ngân hàng TMCP Quân Đội (MBBank)"
}
```

---

### 5. CHECKLIST KIỂM TRA & NGHIỆM THU (TESTING & VERIFICATION CHECKLIST)

- [ ] **Khởi tạo & CSDL**:
  - [ ] Migration tạo toàn bộ bảng: Products, Categories, Orders, OrderItems, Articles.
  - [ ] Seed Data đầy đủ: Danh mục Sơ mi, Danh mục Áo rét; toàn bộ các ảnh trong `Images` được phân loại đúng vào sản phẩm kèm giá, size, màu sắc, mô tả chân thực.
  - [ ] Tài khoản Admin được tạo sẵn (`admin@ngochien.vn` / `Admin@123`).
  - [ ] Seed bài viết mẫu hướng dẫn phối đồ và chăm sóc trang phục Ngọc Hiển.

- [ ] **Giao diện Khách hàng (Storefront)**:
  - [ ] Trang chủ hiển thị chuẩn thương hiệu Thời trang Ngọc Hiển, banner thời thượng, sản phẩm sơ mi, sản phẩm áo rét.
  - [ ] Bộ lọc danh mục, giá, tìm kiếm hoạt động chính xác.
  - [ ] Xem chi tiết sản phẩm: Thư viện ảnh, chọn size, bảng quy đổi size, chọn số lượng.
  - [ ] Thêm sản phẩm vào giỏ hàng, cập nhật số lượng, hiển thị badge giỏ hàng trên header.
  - [ ] Đặt hàng (Checkout) với thông tin địa chỉ giao hàng và 2 hình thức thanh toán (COD / VietQR).
  - [ ] Trang xác nhận đơn hàng hiển thị mã QR thanh toán chuẩn VietQR có số tiền và nội dung chuyển khoản chính xác.
  - [ ] Tra cứu đơn hàng bằng mã đơn hoặc số điện thoại.
  - [ ] Đọc bài viết tin tức/thời trang và xem bài viết mới nhất.

- [ ] **Hệ thống Quản trị (Admin CMS)**:
  - [ ] Đăng nhập quản trị viên, bảo mật chặn người dùng thông thường vào `/Admin`.
  - [ ] Dashboard hiển thị doanh thu, đơn hàng, sản phẩm, bài viết.
  - [ ] Quản lý Sản phẩm: Thêm mới, Sửa, Xóa, Xem danh sách.
  - [ ] Quản lý Đơn hàng: Xem chi tiết đơn hàng, duyệt trạng thái đơn hàng (Chờ xử lý -> Đang giao -> Đã hoàn thành), cập nhật trạng thái thanh toán.
  - [ ] Quản lý Bài viết: Thêm bài viết mới, sửa nội dung, xóa bài viết.
  - [ ] Quản lý Danh mục: Thêm, sửa, xóa danh mục.

- [ ] **Kiểm thử Build & Chạy**:
  - [ ] Dự án build thành công không lỗi `dotnet build`.
  - [ ] Ứng dụng khởi động ổn định, tải trang mượt mà và thân thiện với thiết bị di động (Responsive).
