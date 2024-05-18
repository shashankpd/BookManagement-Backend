using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using ModelLayer.Response;
using RepositoryLayer.Interface;
using System.Data.SqlClient;

namespace BookstoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookBusiness IBookBusiness;

        public BookController(IBookBusiness IBookBusiness)
        {
            this.IBookBusiness = IBookBusiness;
        }

        //start

        [HttpPost]
        public async Task<IActionResult> AddBook(BookBody books)
        {
            try
            {
                // Call the business logic layer to add the book
                var details = await IBookBusiness.AddBook(books);

                if (details != null && details.Any())
                {
                    // Return success response with added books
                    return Ok(new ResponseModel<IEnumerable<Book>>
                    {
                        Message = "Book added successfully",
                        Data = details
                    });
                }
                else
                {
                    // Return success response with message indicating no books were added
                    return Ok(new ResponseModel<IEnumerable<Book>>
                    {
                        Message = "No books were added",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // Return error response with exception message
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetBook()
        {
            try
            {
                var book = await IBookBusiness.getAllBook();

                var response = new ResponseModel<IEnumerable<Book>>
                {
                    Success = true,
                    Message = "Book Details",
                    Data = book

                };
                return Ok(book);
            }
            catch (Exception ex)
            {

                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return Ok(response);
            }

        }



    }
}
