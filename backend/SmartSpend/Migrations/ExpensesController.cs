using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSpend.Dtos;
using SmartSpend.Models;

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
        public async Task<IActionResult> Get()
        {
            var data = await _context.Expenses.ToListAsync();
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
            _context.Expenses.Add(expense.ToModel());
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { expense.Name, expense.Amount }); //zastanowic sie czy chce caly obiekt czy tylko name i amount
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
