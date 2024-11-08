﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data.SqlClient;
using Academy.Models;

namespace Academy.Data
{
    public partial class AcademyContext : DbContext
    {
        public AcademyContext()
        {
        }
        public AcademyContext(DbContextOptions<AcademyContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Parent> Parents { get; set; }
       // public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Trainer> Trainers { get; set; }
        public virtual DbSet<Champion> Champions { get; set; }
        public virtual DbSet<Absence> Abscenses { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<TrainerCategories> CategoryTrainers { get; set; }
       // public virtual DbSet<SubCategoryTrainer> SubCategoryTrainer { get; set; }
        public virtual DbSet<TraineeChampion> TraineeChampions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

           

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}