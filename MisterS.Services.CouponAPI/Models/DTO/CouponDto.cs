﻿namespace MisterS.Services.CouponAPI.Models.DTO
{
    public class CouponDto
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; } = string.Empty;

        public double DiscountAmount { get; set; }
    }
}
