using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Controllers;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using ModelLayer.Response;
using System.Data.SqlClient;

namespace BookstoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBusiness IUserBusiness;

        public UserController(IUserBusiness IUserBusiness)
        {
            this.IUserBusiness = IUserBusiness;
        }

        //start


        [HttpPost]
        public async Task<IActionResult> createUser(UserBody users)
        {
            try
            {
                var details = await IUserBusiness.createUser(users);
                if (details > 0)
                {
                    var response = new ResponseModel<UserBody>
                    {
                        Success = true,
                        Message = "User Added Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid input");
                }
            }
            catch (DuplicateEmailException ex)
            {
                var response = new ResponseModel<UserBody>
                {
                    Success = false,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
            catch (InvalidEmailFormatException ex)
            {
                var response = new ResponseModel<UserBody>
                {
                    Success = false,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<UserBody>
                {
                    Success = false,
                    Message = $"An error occurred while processing your request {ex.Message}"
                };
                return BadRequest(response); // Internal Server Error
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password, IConfiguration configuration)
        {
            try
            {
                var details = await IUserBusiness.Login(email, password, configuration);
                var response = new ResponseModel<string>
                {
                    Message = "Login Sucessfull",
                    Data = details
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    var response = new ResponseModel<User>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return Conflict(response);
                }
                else if (ex is InvalidPasswordException)
                {
                    var response = new ResponseModel<User>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                    return BadRequest(response);
                }
                else
                {
                    return BadRequest($"An error occurred while processing the login request: {ex.Message}");
                }
            }
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var details = await IUserBusiness.GetUserByEmail(email);

            if (details != null)
            {
                return Ok(new ResponseModel<User>
                {
                    Message = "User retrieved successfully",
                    Data = details
                });
            }
            else
            {
                return NotFound(new ResponseModel<User>
                {
                    Message = "No User found",
                    Data = null
                });
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                await IUserBusiness.ForgotPassword(request.Email);
                return Ok("Password reset email sent successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                // Handle unique constraint violation (error number 2627)
                return StatusCode(500, "Email address is already registered.");
            }
            catch (SqlException ex)
            {
                // Handle other SQL-related exceptions
                return StatusCode(500, "An error occurred while accessing the database.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email, string otp, string newPassword)
        {
            try
            {
                await IUserBusiness.ResetPassword(email, otp, newPassword);
                return Ok("Password reset successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(ex.Message);
            }
        }



    }
}
