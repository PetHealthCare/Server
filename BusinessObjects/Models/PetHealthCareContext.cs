﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects.Models
{
    public partial class PetHealthCareContext : DbContext
    {
        public PetHealthCareContext()
        {
        }

        public PetHealthCareContext(DbContextOptions<PetHealthCareContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Pet> Pets { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(GetConnectionString());
			}
		}

		private string GetConnectionString()
		{
			IConfiguration config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true, true)
				.Build();
			var strConn = config["ConnectionStrings:DefaultConnectionString"];
			return strConn;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BillId).HasColumnName("billID");

                entity.Property(e => e.BookingId).HasColumnName("bookingID");

                entity.Property(e => e.InsDate)
                    .HasColumnType("date")
                    .HasColumnName("insDate");

                entity.Property(e => e.PaymentId).HasColumnName("paymentID");

                entity.Property(e => e.TotalAmount).HasColumnName("totalAmount");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__bookingID__6383C8BA");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__paymentID__6477ECF3");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId).HasColumnName("bookingID");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("date")
                    .HasColumnName("bookingDate");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.DoctorId).HasColumnName("doctorID");

                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("note");

                entity.Property(e => e.PetId).HasColumnName("petID");

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");

                entity.Property(e => e.Slot).HasColumnName("slot");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__custome__656C112C");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__doctorI__66603565");

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__petID__6754599E");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__schedul__68487DD7");

                entity.HasMany(d => d.Services)
                    .WithMany(p => p.Bookings)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookingService",
                        l => l.HasOne<Service>().WithMany().HasForeignKey("ServiceId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BookingSe__servi__6A30C649"),
                        r => r.HasOne<Booking>().WithMany().HasForeignKey("BookingId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BookingSe__booki__693CA210"),
                        j =>
                        {
                            j.HasKey("BookingId", "ServiceId").HasName("PK__BookingS__22853CDEF7BBE573");

                            j.ToTable("BookingService");

                            j.IndexerProperty<int>("BookingId").HasColumnName("bookingID");

                            j.IndexerProperty<int>("ServiceId").HasColumnName("serviceID");
                        });
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.UserId, "UQ__Customer__CB9A1CDEF16C435E")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fullName");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_User");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");

                entity.HasIndex(e => e.UserId, "UQ__Doctor__CB9A1CDEAEBA1A3A")
                    .IsUnique();

                entity.Property(e => e.DoctorId).HasColumnName("doctorID");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fullName");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Speciality)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("speciality");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doctor_User");

                entity.HasMany(d => d.Services)
                    .WithMany(p => p.Doctors)
                    .UsingEntity<Dictionary<string, object>>(
                        "DoctorService",
                        l => l.HasOne<Service>().WithMany().HasForeignKey("ServiceId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__DoctorSer__servi__6E01572D"),
                        r => r.HasOne<Doctor>().WithMany().HasForeignKey("DoctorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__DoctorSer__docto__6D0D32F4"),
                        j =>
                        {
                            j.HasKey("DoctorId", "ServiceId").HasName("PK__DoctorSe__967182A505C8109B");

                            j.ToTable("DoctorService");

                            j.IndexerProperty<int>("DoctorId").HasColumnName("doctorID");

                            j.IndexerProperty<int>("ServiceId").HasColumnName("serviceID");
                        });
            });

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PK__MedicalR__D825197E461BC0C9");

                entity.ToTable("MedicalRecord");

                entity.Property(e => e.RecordId).HasColumnName("recordID");

                entity.Property(e => e.Diagnosis)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("diagnosis");

                entity.Property(e => e.DoctorId).HasColumnName("doctorID");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("notes");

                entity.Property(e => e.PetId).HasColumnName("petID");

                entity.Property(e => e.Treatment)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("treatment");

                entity.Property(e => e.VisitDate)
                    .HasColumnType("date")
                    .HasColumnName("visitDate");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.MedicalRecords)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicalRe__docto__6EF57B66");

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.MedicalRecords)
                    .HasForeignKey(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicalRe__petID__6FE99F9F");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("paymentID");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.InsDate)
                    .HasColumnType("date")
                    .HasColumnName("insDate");

                entity.Property(e => e.Method)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("method");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pet");

                entity.Property(e => e.PetId).HasColumnName("petID");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Generic)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("generic");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Species)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("species");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pet__customerID__70DDC3D8");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");

                entity.Property(e => e.DoctorId).HasColumnName("doctorID");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.RoomNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("roomNo");

                entity.Property(e => e.SlotBooking).HasColumnName("slotBooking");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__doctor__71D1E811");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceId).HasColumnName("serviceID");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.LimitTime).HasColumnName("limitTime");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("serviceName");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId).HasColumnName("transactionID");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.BillId).HasColumnName("billID");

                entity.Property(e => e.PaymentId).HasColumnName("paymentID");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("transactionDate")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Bill");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Payment");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.HasIndex(e => e.UserId, "UQ__Staff__CB9A1CDEA661BA3F")
                    .IsUnique();

                entity.Property(e => e.StaffId).HasColumnName("staffID");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fullName");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.staff)
                    .HasForeignKey<staff>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
