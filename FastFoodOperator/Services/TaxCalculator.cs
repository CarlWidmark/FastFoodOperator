namespace FastFoodOperator.Services
{
    public static class TaxCalculator
    {
        public static decimal CalculateVAT(decimal totalPrice)
        {
            return decimal.Round(totalPrice * 0.1071m, 2);
        }

        public static string FormatReceipt(decimal totalPrice)
        {
            decimal vatAmount = CalculateVAT(totalPrice);
            decimal priceWithoutVAT = totalPrice - vatAmount;

            return $"Price without VAT: {priceWithoutVAT:C2}\n" +
                   $"VAT (12%): {vatAmount:C2}\n" +
                   $"Total Price: {totalPrice:C2}";
        }
    }
}
