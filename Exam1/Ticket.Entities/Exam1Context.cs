using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ticket.Entities;

public partial class Exam1Context : DbContext
{
   
    public Exam1Context(DbContextOptions<Exam1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableTicket> AvailableTickets { get; set; }

    public virtual DbSet<BookedTicket> BookedTickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailableTicket>(entity =>
        {
            entity.HasKey(e => e.TicketCode).HasName("PK_availTicket");

            entity.ToTable("availableTicket");

            entity.Property(e => e.TicketCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ticketCode");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("categoryName");
            entity.Property(e => e.EventDate)
                .HasColumnType("datetime")
                .HasColumnName("eventDate");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quota).HasColumnName("quota");
            entity.Property(e => e.Seat)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("seat");
            entity.Property(e => e.TicketName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ticketName");
        });

        modelBuilder.Entity<BookedTicket>(entity =>
        {
            entity.HasKey(e => e.BookedId);

            entity.ToTable("bookedTicket");

            entity.Property(e => e.BookedId).HasColumnName("bookedId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("categoryName");
            entity.Property(e => e.EventDate)
                .HasColumnType("datetime")
                .HasColumnName("eventDate");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quota).HasColumnName("quota");
            entity.Property(e => e.Seat)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("seat");
            entity.Property(e => e.TicketCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ticketCode");
            entity.Property(e => e.TicketName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ticketName");

            entity.HasOne(d => d.TicketCodeNavigation).WithMany(p => p.BookedTickets)
                .HasForeignKey(d => d.TicketCode)
                .HasConstraintName("FK_bookedTicket_availableTicket");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
