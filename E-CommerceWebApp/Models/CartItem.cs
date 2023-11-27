namespace E_CommerceWebApp.Models
{
    public class CartItem
    {
        /*
         * id
         * user
         * product
         * amount
         * singlePrice
         */
        public int ID { get; set; }
        public int CartID { get; set; }
        // user
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public float SinglePrice { get; set; }
    }
}
