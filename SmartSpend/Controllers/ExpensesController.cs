using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSpend.Dtos;
using SmartSpend.Models;
using System.Security.Claims;

namespace SmartSpend.Controllers
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

        private string? GetUserId()
        {
            var userId = HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return userId == null ? null : userId.Value;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            // zrobic oddzielna metode GetUserId(HttpContext)
            // stworzyc jeszcze jedno DTO aby nie wysylac na front tylu danych 
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var data = await _context.Expenses
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(data);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var data = await _context.Expenses
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Id == id);

            return data == null ? NotFound() : Ok(data);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ExpenseInputDto expense)
        {
            var id = GetUserId();

            var model = expense.ToModel();

            if (id == null)
                return Unauthorized();

            model.UserId = id;

            var createdEntity = _context.Expenses.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), createdEntity.Entity); 
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Expense expense)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            if (expense == null || expense.Id == 0)
            {
                return BadRequest();
            }

            var data = await _context.Expenses
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Id == expense.Id);
            if (data == null)
                return NotFound();

            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return NoContent(); //powinno byc NoContent() zgonie z normami rest api, bo po aktualizacji danych juz nie mamy co zwracac
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            if (userId == null) 
                return Unauthorized();


            var expense = await _context.Expenses
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x =>x.Id == id);
            if (expense == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
