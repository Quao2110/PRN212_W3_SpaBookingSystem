using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public decimal DepositAmount { get; set; }

    public decimal Price { get; set; }

    public decimal RemainingAmount { get; set; }

    public int ServiceId { get; set; }

    public int SlotDetailId { get; set; }

    public int UserId { get; set; }

    public DateTime? CancelAt { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public string? Note { get; set; }

    public string? TxnRef { get; set; }

    public string AppointmentStatus { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual SlotDetail SlotDetail { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
