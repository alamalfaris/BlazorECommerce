using Stripe;
using Stripe.Checkout;

namespace BlazorECommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "whsec_a48767b79a973f7066240640bf223e3d76091ccf39bff429a6a30413bc4e040b";

        public PaymentService(ICartService cartService, IAuthService authService, 
            IOrderService orderService)
        {
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;

            StripeConfiguration.ApiKey = "sk_test_51LpSwOCNVMO3MeksnkrJ2jJMEGXpYWXCEfCCnItwSOkqE2JFqsjT7uUkgFZVSn29LdPmTI3iSOF7Lz2jUrB9o2ut00i09S9Vvk";
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetCartProductsDb()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions 
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string>
                    {
                        "US"
                    }
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7283/order-success",
                CancelUrl = "https://localhost:7283/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secret
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = e.Message
                };
            }
        }
    }
}
