using Microsoft.AspNetCore.Identity;
using MyShop.Models;

namespace MyShop.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Đảm bảo CSDL đã được tạo
            await context.Database.EnsureCreatedAsync();

            // 1. Tạo các vai trò Role
            string[] roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Tạo tài khoản Admin mặc định
            string adminEmail = "admin@ngochien.vn";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "0988889999"
                };
                var createResult = await userManager.CreateAsync(adminUser, "Admin@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. Seed Danh mục Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Áo Sơ Mi Nam Cao Cấp",
                        Slug = "ao-so-mi-nam",
                        Description = "Bộ sưu tập áo sơ mi dài tay, cộc tay vải sợi tre, thông hơi chống nhăn thời thượng thương hiệu Ngọc Hiển.",
                        DisplayOrder = 1,
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Áo Rét & Áo Khoác Mùa Đông",
                        Slug = "ao-ret-mua-dong",
                        Description = "Bộ sưu tập áo phao lông vũ, áo khoác chần bông siêu nhẹ cản gió, chống rét đại hàn của Hãng thời trang Ngọc Hiển.",
                        DisplayOrder = 2,
                        IsActive = true
                    }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            var catSoMi = context.Categories.First(c => c.Slug == "ao-so-mi-nam");
            var catAoRet = context.Categories.First(c => c.Slug == "ao-ret-mua-dong");

            // 4. Seed Sản phẩm Products từ kho ảnh thực tế
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    // --- ÁO SƠ MI ---
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Họa Tiết Cao Cấp Ngọc Hiển",
                        Slug = "ao-so-mi-coc-tay-hoa-tiet-cao-cap",
                        Description = "Thiết kế cộc tay trẻ trung, họa tiết tinh tế phối nền sang trọng. Chất liệu Cotton kết hợp Poly chống nhăn, thấm hút mồ hôi tối ưu, đem lại cảm giác thoáng mát cả ngày dài.",
                        Material = "Chất cotton lụa dệt vân chìm cao cấp",
                        Price = 385000,
                        OriginalPrice = 490000,
                        ImageUrl = "/images/products/so-mi/somi_01.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Họa tiết ghi xám, Xanh pastel",
                        StockQuantity = 120,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Kẻ Sọc Xanh Thanh Lịch 201",
                        Slug = "ao-so-mi-coc-ke-201",
                        Description = "Phom Regular Fit tôn dáng, kẻ sọc xanh thanh nhã phù hợp đi làm công sở, gặp gỡ đối tác. Vải mềm mịn, giữ phom cực tốt sau nhiều lần giặt.",
                        Material = "Sợi Bamboo tự nhiên kháng khuẩn",
                        Price = 420000,
                        OriginalPrice = 550000,
                        ImageUrl = "/images/products/so-mi/somi_02.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Kẻ xanh sọc trắng",
                        StockQuantity = 85,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-14)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Kẻ Caro Thời Thượng 206",
                        Slug = "ao-so-mi-coc-ke-206",
                        Description = "Mẫu sơ mi kẻ caro hiện đại giúp quý ông trông trẻ trung, tràn đầy năng lượng. Đường may kỹ lưỡng theo tiêu chuẩn may đo cao cấp của Hãng Ngọc Hiển.",
                        Material = "Vải Spun dệt nano chống nhăn",
                        Price = 390000,
                        OriginalPrice = 520000,
                        ImageUrl = "/images/products/so-mi/somi_03.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Kẻ caro xám ghi, Kẻ xanh",
                        StockQuantity = 95,
                        IsFeatured = false,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-13)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Kẻ Sợi Tre Thoáng Khí 78",
                        Slug = "ao-so-mi-coc-ke-soi-tre-78",
                        Description = "Sản phẩm chủ lực dệt từ sợi tre thiên nhiên, khả năng chống tia UV, kháng mùi vượt trội. Giữ cơ thể luôn mát mẻ ngay cả trong thời tiết hè oi ả.",
                        Material = "65% Bamboo Organic, 35% Microfiber",
                        Price = 450000,
                        OriginalPrice = 590000,
                        ImageUrl = "/images/products/so-mi/somi_04.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Kẻ tăm xanh nhạt",
                        StockQuantity = 110,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-12)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Kẻ Sợi Tre Cao Cấp 86",
                        Slug = "ao-so-mi-coc-ke-soi-tre-86",
                        Description = "Màu sắc nhã nhặn, thiết kế cổ bẻ chuẩn form áo sơ mi công sở. Độ co giãn nhẹ mang lại sự thoải mái khi vận động cả ngày.",
                        Material = "Sợi tre tự nhiên siêu mềm",
                        Price = 460000,
                        OriginalPrice = 600000,
                        ImageUrl = "/images/products/so-mi/somi_05.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Kẻ xanh navy",
                        StockQuantity = 75,
                        IsFeatured = false,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Lụa Vân Quý Phái 46",
                        Slug = "ao-so-mi-coc-lua-van-46",
                        Description = "Dòng sơ mi cao cấp với bề mặt vải ánh vân chìm sang trọng, tạo ấn tượng lịch thiệp và đẳng cấp trong các buổi tiệc hay hội nghị quan trọng.",
                        Material = "Lụa dệt vân cao cấp",
                        Price = 480000,
                        OriginalPrice = 650000,
                        ImageUrl = "/images/products/so-mi/somi_06.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Trắng ngà vân lụa, Xanh ngọc",
                        StockQuantity = 60,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-9)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Cộc Tay Vải Thông Hơi Thoát Nhiệt 03-2026",
                        Slug = "ao-so-mi-coc-thong-hoi-03-2026",
                        Description = "Công nghệ dệt lỗ thoáng khí li ti giúp luồng không khí lưu thông tối đa. Khô nhanh cấp tốc, chống nhăn tuyệt đối không cần là ủi.",
                        Material = "Vải thông hơi công nghệ Cool-Air",
                        Price = 430000,
                        OriginalPrice = 580000,
                        ImageUrl = "/images/products/so-mi/somi_08.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Xanh nhạt, Trắng tinh khôi",
                        StockQuantity = 130,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-8)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Dài Tay Đen Trơn Phong Cách Quý Tộc 2026",
                        Slug = "ao-so-mi-dai-tay-den-tron-2026",
                        Description = "Màu đen tuyền cuốn hút, phom áo chuẩn may đo tôn vóc dáng nam tính. Khuy áo dập chìm thương hiệu Ngọc Hiển sang trọng.",
                        Material = "Cotton chống nhăn phủ nano",
                        Price = 490000,
                        OriginalPrice = 650000,
                        ImageUrl = "/images/products/so-mi/somi_14.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Đen tuyền",
                        StockQuantity = 140,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Dài Tay Kẻ Sợi Tre Công Sở 81",
                        Slug = "ao-so-mi-dai-tay-ke-soi-tre-81",
                        Description = "Sự kết hợp hoàn hảo giữa vẻ đẹp lịch sự công sở và sự thoải mái của chất liệu sợi tre tự nhiên. Thấm hút mồ hôi và kháng khuẩn tối đa.",
                        Material = "Bamboo lụa cao cấp",
                        Price = 490000,
                        OriginalPrice = 620000,
                        ImageUrl = "/images/products/so-mi/somi_17.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Kẻ tăm xám, Kẻ xanh thanh lịch",
                        StockQuantity = 80,
                        IsFeatured = false,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-6)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Dài Tay Trắng Trơn Chuẩn Phong Độ 2026",
                        Slug = "ao-so-mi-dai-tay-trang-tron-2026",
                        Description = "Chiếc áo sơ mi trắng kinh điển không thể thiếu của mọi quý ông. Cổ đứng định hình sắc nét, chất vải trắng sáng bền màu không ố vàng.",
                        Material = "100% Cotton Poplin xử lý chống nhăn",
                        Price = 450000,
                        OriginalPrice = 590000,
                        ImageUrl = "/images/products/so-mi/somi_24.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Trắng tinh khôi",
                        StockQuantity = 200,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new Product
                    {
                        CategoryId = catSoMi.Id,
                        Name = "Áo Sơ Mi Dài Tay Trắng Khuy Đen Độc Bản 2026",
                        Slug = "ao-so-mi-dai-tay-trang-khuy-den-2026",
                        Description = "Điểm nhấn tinh xảo với dàn khuy cúc đen tương phản nổi bật trên nền vải trắng tuyết. Phù hợp diện trong các dịp kỷ niệm, sự kiện và tiệc cưới.",
                        Material = "Cotton dệt chìm cao cấp",
                        Price = 495000,
                        OriginalPrice = 680000,
                        ImageUrl = "/images/products/so-mi/somi_25.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Trắng phối khuy đen",
                        StockQuantity = 90,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    },

                    // --- ÁO RÉT & ÁO KHOÁC MÙA ĐÔNG ---
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Phao Lông Vũ Siêu Nhẹ Cản Gió Ngọc Hiển Pro",
                        Slug = "ao-phao-long-vu-sieu-nhe-can-gio-ngoc-hien",
                        Description = "Dòng áo phao thế hệ mới siêu nhẹ nhưng giữ ấm vượt trội nhờ lớp lông vũ ép nhiệt 3 lớp. Bề mặt vải phủ Nano chống thấm nước mưa phùn và cản gió bão.",
                        Material = "Lông vũ tự nhiên + Lớp ngoài Poly tráng Nano",
                        Price = 850000,
                        OriginalPrice = 1250000,
                        ImageUrl = "/images/products/ao-ret/aoret_01.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Đen nhám, Xanh Navy, Ghi xám",
                        StockQuantity = 95,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-14)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Khoác Chần Bông Giữ Nhiệt 3 Lớp Cao Cấp",
                        Slug = "ao-khoac-chan-bong-giu-nhiet-3-lop",
                        Description = "Thiết kế đường chần bông hình học tinh tế, không xô lệch bông khi giặt máy. Khóa kéo đồng cao cấp siêu bền, có túi trong tiện dụng.",
                        Material = "Bông tổng hợp Bio-Fiber giữ ấm",
                        Price = 790000,
                        OriginalPrice = 1100000,
                        ImageUrl = "/images/products/ao-ret/aoret_02.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Rêu bộ đội, Đen huyền bí",
                        StockQuantity = 70,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-12)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Khoác Rét Dáng Ngắn Trẻ Trung Kèm Mũ Tháo Rời",
                        Slug = "ao-khoac-ret-dang-ngan-mu-thao-roi",
                        Description = "Phong cách năng động cho mùa đông se lạnh. Mũ trùm ấm áp có thể tháo rời linh hoạt tùy theo hoàn cảnh sử dụng. Phom dáng ôm gọn thanh thoát.",
                        Material = "Vải gió 2 lớp lót nỉ mềm",
                        Price = 720000,
                        OriginalPrice = 980000,
                        ImageUrl = "/images/products/ao-ret/aoret_03.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Xanh than, Xám chì",
                        StockQuantity = 60,
                        IsFeatured = false,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-11)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Phao Nam Mùa Đông Đệm Bông Siêu Ấm Dáng Thể Thao",
                        Slug = "ao-phao-nam-mua-dong-dem-bong-sieu-am",
                        Description = "Bảo vệ tối đa cơ thể trong những đợt rét đại hàn miền Bắc. Bo chun cổ tay và gấu áo ngăn gió lùa tuyệt đối.",
                        Material = "Bông gòn tinh khiết siêu nhẹ",
                        Price = 820000,
                        OriginalPrice = 1150000,
                        ImageUrl = "/images/products/ao-ret/aoret_04.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Đen phối xám, Xanh coban",
                        StockQuantity = 88,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Bomber Gió Mùa Đông Lót Nỉ Giữ Nhiệt HeatTech",
                        Slug = "ao-bomber-gio-mua-dong-lot-ni-heattech",
                        Description = "Phom bomber kinh điển không bao giờ lỗi mốt, phối viền cổ và tay dệt kim cao cấp. Bên trong lót nỉ lông cừu ấm áp như lò sưởi.",
                        Material = "Lớp ngoài cản gió + Lót lông cừu nhân tạo",
                        Price = 760000,
                        OriginalPrice = 1050000,
                        ImageUrl = "/images/products/ao-ret/aoret_05.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Xanh rêu, Đen tuyền",
                        StockQuantity = 75,
                        IsFeatured = false,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-8)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Khoác Rét Cản Gió Chống Nước Phom Dài Vừa",
                        Slug = "ao-khoac-ret-can-gio-chong-nuoc",
                        Description = "Chiều dài vừa qua hông, vừa giúp giữ ấm phần lưng bụng vừa tạo dáng vóc thanh tao, mạnh mẽ. Vải trượt nước hoàn hảo khi gặp mưa phùn đông.",
                        Material = "Vải Gore-Tex công nghệ chống thấm",
                        Price = 890000,
                        OriginalPrice = 1350000,
                        ImageUrl = "/images/products/ao-ret/aoret_06.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Xám bạc, Đen carbon",
                        StockQuantity = 55,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-6)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Phao Đại Hàn 4 Lớp Chống Thấm Tuyệt Đối",
                        Slug = "ao-phao-dai-han-4-lop-chong-tham",
                        Description = "Sản phẩm chuyên biệt cho những chuyến công tác vùng núi cao Sa Pa, Hà Giang hay mùa đông băng giá. Đảm bảo thân nhiệt luôn ấm áp từ 5 đến 15 độ C.",
                        Material = "Bông lông vũ cao cấp 4 lớp cách nhiệt",
                        Price = 950000,
                        OriginalPrice = 1450000,
                        ImageUrl = "/images/products/ao-ret/aoret_07.jpg",
                        Sizes = "M, L, XL, 2XL, 3XL",
                        Colors = "Đen bóng thời thượng, Xanh lam",
                        StockQuantity = 65,
                        IsFeatured = true,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    },
                    new Product
                    {
                        CategoryId = catAoRet.Id,
                        Name = "Áo Khoác Rét Thể Thao Phối Màu Hiện Đại",
                        Slug = "ao-khoac-ret-the-thao-phoi-mau",
                        Description = "Đường nét phối màu khỏe khoắn, thích hợp cho các hoạt động ngoài trời, dạo phố, lái xe máy đường dài mùa rét.",
                        Material = "Vải dù chần bông siêu bền",
                        Price = 690000,
                        OriginalPrice = 920000,
                        ImageUrl = "/images/products/ao-ret/aoret_08.jpg",
                        Sizes = "M, L, XL, 2XL",
                        Colors = "Đỏ đô phối đen, Xanh navy",
                        StockQuantity = 80,
                        IsFeatured = false,
                        IsActive = true,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            // 5. Seed Bài viết cẩm nang & tin tức thời trang
            if (!context.Articles.Any())
            {
                var articles = new List<Article>
                {
                    new Article
                    {
                        Title = "Bí Quyết Chọn Áo Sơ Mi Chuẩn Quý Ông Công Sở Từ Hãng Ngọc Hiển",
                        Slug = "bi-quyet-chon-ao-so-mi-chuan-quy-ong",
                        Summary = "Hướng dẫn chi tiết cách chọn kích thước cổ áo, chiều dài tay, màu sắc và chất liệu sơ mi sợi tre giúp bạn luôn tự tin và đẳng cấp tại công sở.",
                        Content = @"
                            <p>Đối với mỗi quý ông, chiếc áo sơ mi không chỉ đơn thuần là trang phục hàng ngày, mà còn là thước đo ngầm cho sự chỉn chu, đĩnh đạc và gu thẩm mỹ. Tại Hãng Thời Trang Ngọc Hiển, chúng tôi luôn chăm chút từng đường kim mũi chỉ để mang đến những chiếc áo sơ mi hoàn hảo nhất.</p>
                            <h4>1. Độ vừa vặn của cổ áo và vai</h4>
                            <p>Khi cài hết cúc cổ, khoảng cách giữa cổ áo và cổ của bạn nên vừa vặn hai ngón tay trượt vào thoải mái. Đường chỉ vai cần nằm đúng ngay điểm tiếp giáp giữa xương vai và cánh tay.</p>
                            <h4>2. Ưu thế của chất liệu sợi tre Bamboo</h4>
                            <p>Chất liệu sợi tre Bamboo tự nhiên tại Ngọc Hiển sở hữu đặc tính tự điều hòa nhiệt độ: mát mẻ vào mùa hè và ấm dịu vào mùa thu đông. Khả năng kháng khuẩn, khử mùi tự nhiên giúp bạn luôn thơm tho suốt 12 tiếng làm việc liên tục.</p>
                            <h4>3. Phối màu thông minh cho từng hoàn cảnh</h4>
                            <p>Sơ mi trắng khuy đen hoặc trắng trơn là lựa chọn số 1 cho các cuộc họp cấp cao. Đối với ngày làm việc thông thường, các mẫu sơ mi kẻ tăm xanh hoặc họa tiết nhã nhặn sẽ mang lại vẻ tươi mới, giàu năng lượng.</p>
                        ",
                        ThumbnailUrl = "/images/products/so-mi/somi_25.jpg",
                        Author = "Ngọc Hiển Stylist",
                        PublishedAt = DateTime.Now.AddDays(-10),
                        IsPublished = true,
                        ViewCount = 1250
                    },
                    new Article
                    {
                        Title = "Cách Chọn Và Bảo Quản Áo Rét Lông Vũ, Áo Phao Mùa Đông Luôn Bền Đẹp",
                        Slug = "cach-chon-va-bao-quan-ao-ret-long-vu",
                        Summary = "Những mẹo vàng bỏ túi giúp áo khoác phao, áo chần bông không bị xẹp, vón cục lông và giữ nhiệt tối đa qua nhiều mùa đông lạnh giá.",
                        Content = @"
                            <p>Mùa đông đến cùng những đợt gió mùa đông bắc rét buốt. Một chiếc áo phao lông vũ hay áo khoác chần bông cao cấp của Ngọc Hiển là trợ thủ đắc lực bảo vệ sức khỏe cho cả gia đình.</p>
                            <h4>1. Giặt áo phao đúng cách bằng máy giặt</h4>
                            <p>Nên sử dụng chế độ giặt nhẹ (Delicate hoặc Wool) với nước lạnh dưới 30 độ C. Dùng nước giặt trung tính thay vì bột giặt có chất tẩy mạnh. Cho 2-3 quả bóng tennis sạch vào lồng giặt để bóng đập đều, giúp lông vũ không bị vón cục lại một chỗ.</p>
                            <h4>2. Phơi và bảo quản</h4>
                            <p>Tránh phơi trực tiếp dưới nắng gắt nhiệt độ cao. Hãy phơi ở nơi thoáng gió, khi áo đã khô 80%, vỗ nhẹ khắp bề mặt áo để các sợi bông nở phồng tơi xốp trở lại.</p>
                            <h4>3. Cất giữ khi hết mùa rét</h4>
                            <p>Sử dụng túi vải bọc thoáng khí, không nên hút chân không quá chặt đối với áo lông vũ tự nhiên vì có thể làm gãy cấu trúc sợi lông giữ nhiệt.</p>
                        ",
                        ThumbnailUrl = "/images/products/ao-ret/aoret_01.jpg",
                        Author = "Ban Chăm Sóc Khách Hàng Ngọc Hiển",
                        PublishedAt = DateTime.Now.AddDays(-5),
                        IsPublished = true,
                        ViewCount = 980
                    },
                    new Article
                    {
                        Title = "Xu Hướng Thời Trang Nam Mùa Đông 2026: Đỉnh Cao Tối Giản & Đa Năng",
                        Slug = "xu-huong-thoi-trang-nam-mua-dong-2026",
                        Summary = "Phong cách Minimalist kết hợp tính năng vượt trội: Áo phao siêu nhẹ, bomber lót lông và sơ mi sợi tre phối nhiều lớp đang chiếm lĩnh xu hướng thời trang.",
                        Content = @"
                            <p>Năm 2026 đánh dấu sự lên ngôi của phong cách ăn mặc Layering (nhiều lớp) thông minh. Một chiếc áo sơ mi dài tay bên trong, khoác ngoài một áo len mỏng và chốt lại bằng chiếc áo phao cản gió của Hãng Thời Trang Ngọc Hiển sẽ tạo nên diện mạo vừa ấm áp vừa đậm chất quý ông hiện đại.</p>
                            <h4>Sự hòa quyện giữa công nghệ vải và thời trang</h4>
                            <p>Các công nghệ như Nano chống nước, sợi vải phát nhiệt HeatTech và bông hữu cơ đang được Ngọc Hiển ứng dụng sâu rộng vào từng sản phẩm, đáp ứng hoàn hảo tiêu chí: Nhẹ - Ấm - Đẹp.</p>
                        ",
                        ThumbnailUrl = "/images/products/ao-ret/aoret_05.jpg",
                        Author = "Thời Trang Ngọc Hiển",
                        PublishedAt = DateTime.Now.AddDays(-2),
                        IsPublished = true,
                        ViewCount = 640
                    }
                };

                context.Articles.AddRange(articles);
                await context.SaveChangesAsync();
            }

            // 6. Seed Đơn hàng mẫu để Admin Dashboard có dữ liệu sinh động
            if (!context.Orders.Any())
            {
                var p1 = context.Products.First(p => p.Slug.Contains("so-mi"));
                var p2 = context.Products.First(p => p.Slug.Contains("phao"));

                var sampleOrder1 = new Order
                {
                    OrderCode = "NH202601",
                    CustomerName = "Nguyễn Văn Hùng",
                    CustomerPhone = "0912345678",
                    CustomerEmail = "hung.nguyen@gmail.com",
                    ShippingAddress = "Số 18 Phố Huế, Quận Hai Bà Trưng",
                    City = "Hà Nội",
                    Notes = "Giao giờ hành chính giúp mình",
                    SubTotal = 1270000,
                    ShippingFee = 0,
                    TotalAmount = 1270000,
                    PaymentMethod = "VietQR",
                    PaymentStatus = "Paid",
                    OrderStatus = "Delivered",
                    CreatedAt = DateTime.Now.AddDays(-3),
                    PaidAt = DateTime.Now.AddDays(-3),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = p1.Id,
                            ProductName = p1.Name,
                            ProductImage = p1.ImageUrl,
                            SelectedSize = "XL",
                            SelectedColor = "Tiêu chuẩn",
                            UnitPrice = p1.Price,
                            Quantity = 1,
                            TotalPrice = p1.Price
                        },
                        new OrderItem
                        {
                            ProductId = p2.Id,
                            ProductName = p2.Name,
                            ProductImage = p2.ImageUrl,
                            SelectedSize = "L",
                            SelectedColor = "Đen nhám",
                            UnitPrice = p2.Price,
                            Quantity = 1,
                            TotalPrice = p2.Price
                        }
                    }
                };

                var sampleOrder2 = new Order
                {
                    OrderCode = "NH202602",
                    CustomerName = "Trần Thị Bích Ngọc",
                    CustomerPhone = "0987112233",
                    CustomerEmail = "bichngoc@outlook.com",
                    ShippingAddress = "Chung cư Vinhomes Central Park, Bình Thạnh",
                    City = "TP. Hồ Chí Minh",
                    Notes = "Gọi trước khi giao 15 phút",
                    SubTotal = 450000,
                    ShippingFee = 30000,
                    TotalAmount = 480000,
                    PaymentMethod = "VietQR",
                    PaymentStatus = "Pending",
                    OrderStatus = "Processing",
                    CreatedAt = DateTime.Now.AddHours(-5),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = p1.Id,
                            ProductName = p1.Name,
                            ProductImage = p1.ImageUrl,
                            SelectedSize = "M",
                            SelectedColor = "Trắng",
                            UnitPrice = p1.Price,
                            Quantity = 1,
                            TotalPrice = p1.Price
                        }
                    }
                };

                context.Orders.AddRange(sampleOrder1, sampleOrder2);
                await context.SaveChangesAsync();
            }
        }
    }
}
