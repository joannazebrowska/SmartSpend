using System.ComponentModel.DataAnnotations;

namespace SmartSpend.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; } //bez znaku zapytania albo ustawienia string na null, kompilator sie czepia (nullable reference types), 
        //bo ich brak oznacza - wartosc nie moze byc null. przez to siadal build migration
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
