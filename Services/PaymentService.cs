using Braintree;
using stayWithMeApi.Models;
using stayWithMeApi.DTOS;
using System.Runtime.CompilerServices;

namespace stayWithMeApi.Services
{
    public class PaymentService
    {

        private readonly IConfiguration config;
        private BraintreeGateway braintreeGateway;
        private AppDbContext dbContext;

        public PaymentService(IConfiguration configuration, AppDbContext applicationDbContext)
        {
            config = configuration;
            dbContext = applicationDbContext;
            braintreeGateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = configuration["Braintree:MerchantId"],
                PublicKey = configuration["Braintree:PublicKey"],
                PrivateKey = configuration["Braintree:PrivateKey"]
            };
        }
        public string GenerateClientToken()
        {
            return braintreeGateway.ClientToken.Generate();
        }
        public async Task<string> addUser(int userId)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null) throw new Exception("user not found");

                var requst = new CustomerRequest
                {
                    FirstName = user.userName,
                    LastName = user.Name,
                    Email = user.Email,
                };

                var response = await braintreeGateway.Customer.CreateAsync(requst);



                user.brainTreeId = response.Target.Id;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();

                return response.Target.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> addCard(int userId, string nonce)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null) throw new Exception("user not found");

                var braintreeId = user.brainTreeId ?? await addUser(user.Id);

                var requst = new CreditCardRequest
                {
                    CustomerId = braintreeId,
                    PaymentMethodNonce = nonce,
                };

                var response = await braintreeGateway.CreditCard.CreateAsync(requst);
                if (!response.IsSuccess()) throw new Exception("something went wrong");

                return response.Target.Token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CreditCardsDto>> getCards(int userId)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null) throw new Exception("user not found");

                var braintreeId = user.brainTreeId ?? await addUser(user.Id);

                var requst = new CustomerRequest
                {
                    CustomerId = braintreeId,
                };

                var response = await braintreeGateway.Customer.FindAsync(braintreeId);

                List<CreditCardsDto> cards = new();

                foreach (var creditCard in response.CreditCards)
                {
                    cards.Add(new CreditCardsDto
                    {
                        lastFour = creditCard.LastFour,
                        imgUrl = creditCard.ImageUrl,
                        Token = creditCard.Token,
                    });
                }

                return cards;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> pay(int userId, string token, int effectid)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null) throw new Exception("user not found");

                var braintreeId = user.brainTreeId ?? await addUser(user.Id);

                var requst = new CustomerRequest
                {
                    CustomerId = braintreeId,
                };

                var response = await braintreeGateway.Customer.FindAsync(braintreeId);


                if (!response.CreditCards.Any(c => c.Token == token)) throw new Exception("steal try");

                throw new NotImplementedException();


            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
