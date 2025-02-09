﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")] // {0} = Name, {1} = Max, {2} = Min
        public string Name { get; set; }


        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)] // chamando mailto
        public string Email { get; set; }


        [Required(ErrorMessage = "{0} required")]
        [Display(Name = "Birth Date")] // definindo critério de data
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] // formatando a data para pt-BR
        public DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "{0} required")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")] // definindo nomenclatura do salário
        [DisplayFormat(DataFormatString = "{0:F2}")] // Informando que este atributo {0} vai ter a formatação {F2} 00.00 <--
        public double BaseSalary { get; set; }


        public Department Department { get; set; }

        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales // sr tal sr.Date seja maior q minha data inicial e e sr.Date, seja < ou igual a minha data inicial 
                .Where(sr => sr.Date >= initial && sr.Date <= final)
                .Sum(sr => sr.Amount);
        }
    }
}
