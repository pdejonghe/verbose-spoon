﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProceedixDemoApp.Models
{
    public class LogMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public LogLevel LogLevel { get; set; }

        public Application Application { get; set; }
    }
}