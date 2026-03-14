## Cac tinh nang da hoan thanh
- User Registration & Authentication (JWT)
- revoke refresh token khi đăng xuất
- Logout (revoke refresh token)


## Phat trien tinh nang user
User Management
 ├── GET    /api/users/{id}          — xem profile
 ├── PUT    /api/users/{id}          — cập nhật profile (fullName, gender, birthday)
 ├── POST   /api/users/{id}/avatar   — upload avatar
 ├── GET    /api/users/{id}/addresses — danh sách địa chỉ
 ├── POST   /api/users/{id}/addresses — thêm địa chỉ
 ├── PUT    /api/users/{id}/addresses/{addressId} — sửa địa chỉ
 └── DELETE /api/users/{id}/addresses/{addressId} — xóa địa chỉ

## Phat trien tinh nang Catalog
Category
 ├── GET    /api/categories          — danh sách (dạng cây parent/child)
 ├── GET    /api/categories/{id}
 ├── POST   /api/categories          — [Admin]
 ├── PUT    /api/categories/{id}     — [Admin]
 └── DELETE /api/categories/{id}     — [Admin]

Product
 ├── GET    /api/products            — danh sách, filter, phân trang
 ├── GET    /api/products/{id}       — chi tiết kèm variants
 ├── POST   /api/products            — [Admin/Staff]
 ├── PUT    /api/products/{id}       — [Admin/Staff]
 └── PATCH  /api/products/{id}/status — đổi trạng thái Draft/Active/Archived

ProductVariant
 ├── POST   /api/products/{id}/variants      — thêm variant
 ├── PUT    /api/products/{id}/variants/{id} — sửa variant
 └── DELETE /api/products/{id}/variants/{id} — xóa variant

Attribute
 ├── GET  /api/attributes
 ├── POST /api/attributes            — [Admin]
 └── POST /api/attributes/{id}/values — thêm giá trị

## Phat trien tinh nang Cart
Cart
 ├── GET    /api/cart               — xem giỏ hàng
 ├── POST   /api/cart/items         — thêm sản phẩm vào giỏ
 ├── PUT    /api/cart/items/{id}    — cập nhật số lượng
 └── DELETE /api/cart/items/{id}   — xóa item khỏi giỏ

 ## Phat trien tinh nang Order
 Order
 ├── POST   /api/orders             — tạo đơn hàng từ cart
 ├── GET    /api/orders             — danh sách đơn của user
 ├── GET    /api/orders/{id}        — chi tiết đơn
 └── PATCH  /api/orders/{id}/cancel — hủy đơn

Order Management [Admin/Staff]
 ├── GET    /api/admin/orders               — tất cả đơn hàng
 ├── PATCH  /api/admin/orders/{id}/status   — cập nhật trạng thái
 └── POST   /api/admin/orders/{id}/shipment — tạo shipment

 ## Phat trien tinh nang Payment & Refund
 Payment
 ├── POST /api/orders/{id}/payment       — thanh toán (COD hoặc online)
 └── GET  /api/orders/{id}/payment       — trạng thái thanh toán

Refund
 ├── POST /api/orders/{id}/refund        — yêu cầu hoàn tiền
 └── PATCH /api/admin/refunds/{id}/status — duyệt/từ chối [Admin]

 ## Phat trien tinh nang Voucher
 Voucher
 ├── POST /api/vouchers/validate    — kiểm tra voucher có hợp lệ không
 ├── GET  /api/admin/vouchers       — [Admin]
 ├── POST /api/admin/vouchers       — tạo voucher [Admin]
 └── PUT  /api/admin/vouchers/{id}  — sửa voucher [Admin]

 ## Phat trien tinh nang Review & Inventory
 Review
 ├── GET  /api/products/{id}/reviews  — danh sách review
 ├── POST /api/products/{id}/reviews  — viết review (phải đã mua)
 └── DELETE /api/reviews/{id}         — xóa review của mình

Inventory [Admin/Staff]
 ├── POST /api/inventory/import       — nhập kho
 └── GET  /api/inventory/{variantId}  — lịch sử thay đổi tồn kho

 ## Phan trien tinh nang Admin Dashboard & Analytics & Reporting
 Role Management [Admin]
 ├── GET  /api/admin/roles
 ├── POST /api/admin/users/{id}/roles — gán role cho user
 └── DELETE /api/admin/users/{id}/roles/{roleId}

Report [Admin]
 ├── GET /api/admin/reports/revenue   — doanh thu theo thời gian
 ├── GET /api/admin/reports/orders    — thống kê đơn hàng
 └── GET /api/admin/reports/products  — sản phẩm bán chạy

