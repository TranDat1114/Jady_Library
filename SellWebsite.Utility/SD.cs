﻿namespace SellWebsite.Utility
{
    //Tạo các role ở đây -> tiện cho việc xử lý tạo vào so sánh phân quyền,...
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Employee = "Employee";
        public const string Role_Admin = "Admin";
        public const string Role_Boss = "Boss";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "InProcess";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";
        public const string StatusReturned = "Returned";

        public const string PaypalVERIFIED = "VERIFIED";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "ApprovedForDelayPayment";
        public const string PaymentRejected = "Rejected";

        public const string SessionCart = "SessionShopingCart";
        public const string SessionShopingCarts = "SessionShopingCarts";

        //Cảm thấy sử dụng enum có vẻ không phù hợp hơn nên đổi
        //public enum Role
        //{
        //    Customer,
        //    Employee,
        //    Admin,
        //    Boss
        //}

    }
}
