using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;
using E_Commerce_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        [HttpGet("get-order")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderWithCartItemDto>>> GetOrder()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var orderExist = await _orderRepository.GetOrderAsync(userId);

            if (orderExist == null)
            {
                return BadRequest("Order is empty.");
            }

            var ordersWithCartItems = new List<OrderWithCartItemDto>();

            foreach (var order in orderExist)
            {
                var orderWithCartItems = new OrderWithCartItemDto
                {
                    Id = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    CartItems = order.CartItems.Select(cartItem => new CartItemWithProductDetailsDto
                    {
                        ProductId = cartItem.AllProductId,
                        ProductTitle = cartItem.AllProduct.Title,
                        ProductDescription = cartItem.AllProduct.Description,
                        ProductCategory = cartItem.AllProduct.Category,
                        ProductCount = cartItem.AllProduct.Rating.Count,
                        ProductRate = cartItem.AllProduct.Rating.Rate,
                        ProductImage = cartItem.AllProduct.Image,
                        Quantity = cartItem.Quantity,
                        Amount = cartItem.Amount,
                        Price = cartItem.Price
                    }).ToList()
                };

                ordersWithCartItems.Add(orderWithCartItems);
            }
            return Ok(ordersWithCartItems);
        }

        [HttpPost("create-order")]
        [Authorize]
        public async Task<ActionResult> CreateOrder(OrderDto orderDto)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            //var user = await _orderRepository.GetUserCartAsync(userId);

            //if (user == null || user.Cart == null || !user.Cart.CartItems.Any() || user.Cart.IsActive == false)
            //{
            //    return BadRequest("Cart is empty or user/cart not found.");
            //}

            var cart = await _orderRepository.GetIsActiveCartAsync(userId);

            if (cart == null)
            {
                return NotFound("Cart is empty");
            }

            foreach (var orderCartItem in orderDto.CartItems)
            {
                var matchingCartItem = cart.CartItems.FirstOrDefault(c => c.AllProductId == orderCartItem.AllProductId);

                if (matchingCartItem != null)
                {
                    matchingCartItem.Quantity = orderCartItem.Quantity;
                }
            }

            // Update cart 
            await _orderRepository.UpdateCartAsync(cart);

            //////////////////////////////////////////////////////////////////////////////////////////////

            var order = new Order
            {
                UserId = userId,
                CartId = cart.Id,
                TotalAmount = orderDto.TotalAmount,
                OrderDate = DateTime.UtcNow,
                Status = "Completed",
            };

            await _orderRepository.AddOrderAsync(order);

            var orderId = await _orderRepository.GetOrderAsync(userId);

            var cartItems = cart.CartItems.ToList();
            foreach (var cartItem in cartItems)
            {
                cartItem.OrderId = order.OrderId;
            }

            await _orderRepository.SaveChangesAsync();

            cart.IsActive = false;

            await _orderRepository.UpdateCartAsync(cart);

            return Ok();
        }
    }
}
