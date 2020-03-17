﻿namespace ViewModels.Shared.Product
{
    public class ProductReserveInputModel
    {
        public long UserId { get; set; }

        public long TransactionId { get; set; }

        public long ProductId { get; set; }

        public int Qty { get; set; } = 1;

        public int Timeout { get; set; } = 5000; // ms
    }
}