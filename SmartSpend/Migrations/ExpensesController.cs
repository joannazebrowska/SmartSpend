using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartSpend.Dtos;
using SmartSpend.Models;
using System.Security.Claims;

namespace SmartSpend.Migrations
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            // oddzielna metoda GetUserId(HttpContext)
            var userId = HttpContext?.User?.Claims?.First(x => x.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var data = await _context.Expenses
                .Where(x => x.UserId == userId.Value)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var data = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseInputDto expense)
        {
            var model = expense.ToModel();

            var Claim = HttpContext?.User?.Claims?.First(x => x.Type == ClaimTypes.NameIdentifier);

            if (Claim == null)
                return Unauthorized(model);

            model.UserId = Claim.Value;

            var createdEntity = _context.Expenses.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), createdEntity.Entity); //zastanowic sie czy chce caly obiekt czy tylko name i amount
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Expense expense)
        {
            if (expense == null || expense.Id == 0)
            {
                return BadRequest();
            }

            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x =>x.Id == id);
            if (expense == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
