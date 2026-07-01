using PaymentOrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentOrderAPI.Application.Payments.Strategies
{
    public class MonedaDuraFeeStrategy : IProviderFeeStrategy
    {
        private const string UnsupportedModeMessage = "MonedaDura does not support {0}.";

        public string ProviderName => "MonedaDura";

        public bool Supports(PaymentMode mode, decimal? amount) =>
            mode == PaymentMode.TDC || mode == PaymentMode.Transfer || (mode == PaymentMode.Cash && amount.HasValue && amount > 500);

        public decimal CalculateFee(PaymentMode mode, decimal totalAmount) => mode switch
        {
            PaymentMode.Cash =>  CalculateCashFee(totalAmount),
            PaymentMode.TDC => CalculateTdcFee(totalAmount),
            PaymentMode.Transfer => CalculateTransferFee(totalAmount),
            _ => throw new NotSupportedException(string.Format(UnsupportedModeMessage, mode))
        };

        private static decimal CalculateTdcFee(decimal amount) => amount switch
        {
            <= 500m => amount * 0.035m,
            <= 1000 => amount * 0.025m,
            _ => amount * 0.022m
        };

        private static decimal CalculateTransferFee(decimal amount) => amount switch
        {
            <= 500m => 0.00m,
            >= 501m => amount * 0.025m,
            _ => amount * 0.025m
        };
        private static decimal CalculateCashFee(decimal amount) => amount switch
        {
            <= 2000 => 10m,
            _ => amount * 0.025m
        };
    }

}
