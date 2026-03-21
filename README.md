# HelloKitty E-Commerce

Hệ thống thương mại điện tử bán lẻ trực tuyến, được xây dựng như đồ án tốt nghiệp với kiến trúc N-Layer Architecture, ASP.NET Core 8.

---

## Tổng quan

HelloKitty E-Commerce cung cấp đầy đủ luồng nghiệp vụ từ duyệt sản phẩm, quản lý giỏ hàng, đặt hàng đến thanh toán cho khách hàng, đồng thời trang bị bộ công cụ quản trị toàn diện cho Admin và Staff bao gồm quản lý sản phẩm, đơn hàng, kho hàng và báo cáo kinh doanh.

---

## Công nghệ sử dụng

**Backend**
- ASP.NET Core 8.0
- Entity Framework Core 8.0 (Code-First)
- SQL Server
- JWT Bearer Authentication
- BCrypt.Net-Next
- FluentValidation
---

## Kiến trúc hệ thống

Dự án được tổ chức theo N-Layer Architecture gồm 4 project:

| Project | Nội dung | Phụ thuộc |
|---|---|---|
| HelloKitty.Domain | Entities, Enums, Domain Interfaces | Không phụ thuộc project nào |
| HelloKitty.Application | Services, DTOs, Validators, Result Pattern | Domain |
| HelloKitty.Infrastructure | DbContext, Repositories, UnitOfWork, JWT | Domain, Application |
| HelloKitty.API | Controllers, Middleware, Program.cs | Application, Infrastructure |

**Các pattern áp dụng**

- Repository Pattern
- Unit of Work Pattern
- Result Pattern
- Dependency Injection
- RBAC (Role-Based Access Control)

---

## Cơ sở dữ liệu

Hệ thống gồm 33 bảng, chia thành 8 nhóm chức năng:

| Nhóm | Số bảng | Bảng chính |
|---|---|---|
| Users & Auth | 9 | User, UserCredential, UserAddress, RefreshToken, ... |
| RBAC | 4 | Role, UserRole, Permission, RolePermission |
| Catalog | 8 | Category, Product, ProductVariant, Attribute, ... |
| Cart | 2 | Cart, CartItem |
| Orders | 6 | Order, OrderItem, Payment, Shipment, Refund, ... |
| Promotions | 2 | Voucher, VoucherUsage |
| Inventory | 1 | InventoryLog |
| Logging | 2 | AuditLog, SystemLog |

---

## Tính năng chính

**Xác thực và bảo mật**
- Đăng ký, đăng nhập, refresh token, logout
- Mã hóa mật khẩu BCrypt
- Phân quyền theo Role: Admin, Staff, Customer

**Catalog**
- Danh mục sản phẩm đệ quy nhiều cấp
- Sản phẩm với nhiều biến thể (size, màu, SKU)
- Tìm kiếm, lọc theo danh mục, phân trang
- Quản lý trạng thái: Draft, Active, Inactive, Archived

**Giỏ hàng và đặt hàng**
- Thêm, sửa, xóa item trong giỏ hàng
- Kiểm tra tồn kho realtime khi thêm vào giỏ
- Snapshot giá tại thời điểm thêm vào giỏ
- Đặt hàng từ giỏ, áp mã giảm giá, chọn phương thức thanh toán
- Luồng trạng thái: Pending, Paid, Processing, Shipped, Complete, Cancelled
- Quản lý vận chuyển và hoàn tiền

**Khuyến mãi và kho hàng**
- Voucher giảm theo phần trăm hoặc số tiền cố định
- Giới hạn lượt dùng toàn hệ thống và theo từng user
- Nhập kho, lịch sử thay đổi tồn kho tự động

**Admin**
- Quản lý toàn bộ đơn hàng, cập nhật trạng thái, tạo shipment
- Gán và thu hồi role cho người dùng
- Báo cáo doanh thu, thống kê đơn hàng, top sản phẩm bán chạy

---

## Cài đặt và chạy

**Yêu cầu**
- .NET SDK 8.0
- SQL Server 2019 trở lên

**Backend**

Bước 1: Clone repository

```bash
git clone https://github.com/your-username/hellokitty-ecommerce.git
cd hellokitty-ecommerce
```

Bước 2: Cấu hình User Secrets (trong thư mục HelloKitty.API)

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\\SQLEXPRESS;Database=TMDT;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet user-secrets set "Jwt:SecretKey" "your-secret-key-here"
```

Bước 3: Chạy migration

```bash
dotnet ef database update --project HelloKitty.Infrastructure --startup-project HelloKitty.API
```

Bước 4: Chạy API

```bash
cd HelloKitty.API
dotnet run
```

Swagger UI tại: https://localhost:7173/swagger

---

## Cấu trúc thư mục



## API chính

| Method | Endpoint | Mô tả |
|---|---|---|
| POST | /api/auth/register | Đăng ký tài khoản |
| POST | /api/auth/login | Đăng nhập |
| POST | /api/auth/refresh | Làm mới token |
| GET | /api/categories | Danh sách danh mục |
| GET | /api/products | Danh sách sản phẩm |
| GET | /api/products/{id} | Chi tiết sản phẩm |
| GET | /api/cart | Xem giỏ hàng |
| POST | /api/cart/items | Thêm vào giỏ |
| POST | /api/orders | Đặt hàng |
| GET | /api/orders | Đơn hàng của tôi |
| POST | /api/vouchers/validate | Kiểm tra voucher |
| GET | /api/admin/orders | Tất cả đơn hàng |
| GET | /api/admin/reports/revenue | Báo cáo doanh thu |

Toàn bộ endpoint được document tại /swagger khi chạy môi trường Development.
