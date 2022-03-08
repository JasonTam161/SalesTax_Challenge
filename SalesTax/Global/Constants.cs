namespace SalesTax.Global
{
    public class Constants
    {
        public const decimal GST = 10m;
        public const decimal IMPORT = 5m;
        public const string CART = "_Cart";
        //Number to round when calculating tax. 2 for 0.5, 4 for 0.25, 20 for 0.05, 100 for 0.01 etc
        public const decimal ROUND_NUMBER = 20; 
    }
}
