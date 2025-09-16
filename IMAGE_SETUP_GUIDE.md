# Hướng Dẫn Khắc Phục Vấn Đề Hình Ảnh Sản Phẩm

## Vấn Đề
Website bị mất hình ảnh sản phẩm do thiếu các file hình ảnh trong thư mục `wwwroot/images/`.

## Nguyên Nhân
1. Dữ liệu mẫu trong `SeedData.cs` có đường dẫn hình ảnh nhưng các file thực tế không tồn tại
2. Thư mục `wwwroot/images/products/` trống
3. Thiếu xử lý fallback khi hình ảnh không tồn tại

## Giải Pháp Đã Thực Hiện

### 1. Tạo File Placeholder
Đã tạo các file placeholder cho:
- `/images/products/default.jpg` - Hình mặc định cho sản phẩm
- `/images/banners/banner1.jpg`, `banner2.jpg`, `banner3.jpg` - Banner trang chủ
- `/images/categories/default.jpg` - Hình mặc định cho danh mục
- Các file hình ảnh sản phẩm cụ thể theo dữ liệu SeedData

### 2. Thêm JavaScript Xử Lý Lỗi Hình Ảnh
Tạo file `wwwroot/js/image-handler.js` để:
- Tự động thay thế hình ảnh bị lỗi bằng placeholder
- Xử lý lỗi cho tất cả loại hình ảnh (sản phẩm, banner, danh mục)
- Hiển thị thông báo thân thiện khi hình ảnh không có sẵn

### 3. Cập Nhật Layout
Thêm script `image-handler.js` vào `_Layout.cshtml` để áp dụng toàn website.

## Cách Thay Thế Hình Ảnh Thực

### Bước 1: Chuẩn Bị Hình Ảnh
1. Tải hình ảnh sản phẩm thực tế
2. Đặt tên file theo đúng tên trong SeedData:
   - `nike-air-max-270.jpg`
   - `adidas-ultraboost-22.jpg`
   - `converse-chuck-taylor.jpg`
   - `cao-got-mui-nhon.jpg`
   - `giay-tay-oxford.jpg`
   - `vans-old-skool.jpg`
   - `sandal-de-xuong.jpg`
   - `boot-da-nam.jpg`
   - `bitis-hunter-street.jpg`
   - `cao-got-de-bet.jpg`

### Bước 2: Upload Hình Ảnh
1. Copy các file hình ảnh vào thư mục `wwwroot/images/products/`
2. Đảm bảo kích thước hình ảnh phù hợp (khuyến nghị: 800x600px)
3. Format: JPG, PNG, WebP

### Bước 3: Tạo Hình Ảnh Phụ (Tùy Chọn)
Tạo các hình ảnh bổ sung theo format trong SeedData:
- `nike-air-max-270-1.jpg`, `nike-air-max-270-2.jpg`, `nike-air-max-270-3.jpg`
- Tương tự cho các sản phẩm khác

### Bước 4: Banner và Category
1. Thay thế banner: `wwwroot/images/banners/banner1.jpg`, `banner2.jpg`, `banner3.jpg`
2. Thay thế hình danh mục: `wwwroot/images/categories/`
   - `thethao.jpg`
   - `caogot.jpg`
   - `tay.jpg`
   - `sandal.jpg`
   - `boot.jpg`

## Kiểm Tra
1. Chạy ứng dụng: `dotnet run`
2. Truy cập trang chủ để xem banner
3. Kiểm tra danh sách sản phẩm
4. Xem chi tiết sản phẩm

## Lưu Ý
- File placeholder hiện tại chỉ là file HTML, cần thay thế bằng hình ảnh thực
- Có thể sử dụng dịch vụ placeholder online tạm thời: `https://via.placeholder.com/800x600/e9ecef/6c757d?text=Product+Image`
- Đảm bảo quyền truy cập file trong thư mục wwwroot

## Tối Ưu Hóa Thêm
1. Nén hình ảnh để tăng tốc độ tải
2. Sử dụng WebP format cho hiệu suất tốt hơn
3. Implement lazy loading cho hình ảnh
4. Tạo thumbnail cho danh sách sản phẩm