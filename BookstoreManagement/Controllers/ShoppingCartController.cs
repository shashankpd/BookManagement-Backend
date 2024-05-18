using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using ModelLayer.Response;
using System.Security.Claims;

namespace BookstoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingBusiness ShoppingCart;

        public ShoppingCartController(IShoppingBusiness ShoppingCart)
        {
            this.ShoppingCart = ShoppingCart;
        }

        [HttpGet("GetCartBooks")]
        public async Task<ActionResult<ResponseModel<List<Book>>>> GetCartBooks()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cartBooks = await ShoppingCart.GetCartBooks(userId);
            var response = new ResponseModel<List<Book>> { Message = "Retrieved books successfully", Data = cartBooks };
            return Ok(response);
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<ResponseModel<List<Book>>>> AddToCart([FromBody] CartBody cartRequest)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var updatedCart = await ShoppingCart.AddToCart(cartRequest, userId);
            var response = new ResponseModel<List<Book>> { Message = "Added to cart successfully", Data = updatedCart };
            return Ok(response);
        }

        [HttpPut("UpdateQuantity")]
        public async Task<ActionResult<ResponseModel<CartBody>>> UpdateQuantity([FromBody] CartBody cartRequest)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var updatedCartRequest = await ShoppingCart.UpdateQuantity(userId, cartRequest);
            var response = new ResponseModel<CartBody> { Message = "Updated quantity successfully", Data = updatedCartRequest };
            return Ok(response);
        }

        [HttpDelete("DeleteCart")]
        public async Task<ActionResult<ResponseModel<bool>>> DeleteCart([FromBody] int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isDeleted = await ShoppingCart.DeleteCart(userId, id);
            var response = new ResponseModel<bool> { Message = "Deleted from cart successfully", Data = isDeleted };
            return Ok(response);
        }


    }
}
