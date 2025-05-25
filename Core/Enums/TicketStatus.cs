using System.ComponentModel.DataAnnotations;

namespace Core.Enums;
public enum TicketStatus
{
	[Display(Name ="باز")]
	Open = 0,
	[Display(Name = "پاسخ داده شده")]
	Answered = 1,
	[Display(Name = "بسته شده")]
	Closed = 2,
}