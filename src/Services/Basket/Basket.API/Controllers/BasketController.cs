using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart shoppingCart)
        {

            return Ok(await _repository.UpdateBasket(shoppingCart));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);

            return Ok();
        }
    }
}
