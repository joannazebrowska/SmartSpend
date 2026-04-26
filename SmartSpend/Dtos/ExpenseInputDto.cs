using SmartSpend.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartSpend.Dtos
{
    public class ExpenseInputDto
    {
        [Required]
        public string Name { get; set; } //bez znaku zapytania albo ustawienia string na null, kompilator sie czepia (nullable reference types), 
        //bo ich brak oznacza - wartosc nie moze byc null. przez to siadal build migration
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateOnly Date { get; set; }

        public Expense ToModel()
        {
            return new Expense
            {
                Name = Name,
                Amount = Amount,
                Date = Date
            };
        }
    }
}
