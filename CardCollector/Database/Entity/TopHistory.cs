using System;
using System.ComponentModel.DataAnnotations;

namespace CardCollector.Database.Entity;

public class TopHistory
{
    [Key]
    public DateTime Date { get; set; } = DateTime.Now;
    public string Exp { get; set; }
    public string Tier4 { get; set; }
    public string Roulette { get; set; }
    public string Ladder { get; set; }
    public string Puzzle { get; set; }
    public string Gift { get; set; }
    public string Invite { get; set; }
}