﻿using Microsoft.EntityFrameworkCore;

namespace Mix.Cms.Messenger.Models.Data
{
    public partial class sw_chatContext : DbContext
    {
        public sw_chatContext()
        {
        }

        public sw_chatContext(DbContextOptions<sw_chatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MixMessengerHubRoom> MixMessengerHubRoom { get; set; }
        public virtual DbSet<MixMessengerMessage> MixMessengerMessage { get; set; }
        public virtual DbSet<MixMessengerNavRoomUser> MixMessengerNavRoomUser { get; set; }
        public virtual DbSet<MixMessengerNavTeamUser> MixMessengerNavTeamUser { get; set; }
        public virtual DbSet<MixMessengerTeam> MixMessengerTeam { get; set; }
        public virtual DbSet<MixMessengerUser> MixMessengerUser { get; set; }
        public virtual DbSet<MixMessengerUserDevice> MixMessengerUserDevice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //define the database to use
                string cnn = "Data Source=mix-messenger.db";
                //string cnn = "Server=(localdb)\\mssqllocaldb;Database=mix-cms.db;Trusted_Connection=True;MultipleActiveResultSets=true";
                optionsBuilder.UseSqlite(cnn);
                //define the database to use
                //string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
                //if (!string.IsNullOrEmpty(cnn))
                //{
                //    if (MixService.GetConfig<bool>("IsMysql"))
                //    {
                //        optionsBuilder.UseSqlite(cnn);
                //    }
                //    else
                //    {
                //        optionsBuilder.UseSqlServer(cnn);
                //    }
                //}
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MixMessengerHubRoom>(entity =>
            {
                entity.ToTable("mix_messenger_hub_room");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.HostId).HasMaxLength(128);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<MixMessengerMessage>(entity =>
            {
                entity.ToTable("mix_messenger_message");

                entity.HasIndex(e => e.RoomId)
                    .HasName("IX_messenger_message_RoomId");

                entity.HasIndex(e => e.TeamId)
                    .HasName("IX_messenger_message_TeamId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_messenger_message_UserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.MixMessengerMessage)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_messenger_message_messenger_hub_room");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.MixMessengerMessage)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_messenger_message_messenger_team");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MixMessengerMessage)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_messenger_message_messenger_user");
            });

            modelBuilder.Entity<MixMessengerNavRoomUser>(entity =>
            {
                entity.HasKey(e => new { e.RoomId, e.UserId });

                entity.ToTable("mix_messenger_nav_room_user");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_messenger_nav_room_user_UserId");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.JoinedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.MixMessengerNavRoomUser)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_messenger_nav_room_user_messenger_hub_room");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MixMessengerNavRoomUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_messenger_nav_room_user_messenger_user");
            });

            modelBuilder.Entity<MixMessengerNavTeamUser>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId });

                entity.ToTable("mix_messenger_nav_team_user");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_messenger_nav_team_user_UserId");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.JoinedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.MixMessengerNavTeamUser)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_messenger_nav_team_user_messenger_team");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MixMessengerNavTeamUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_messenger_nav_team_user_messenger_user");
            });

            modelBuilder.Entity<MixMessengerTeam>(entity =>
            {
                entity.ToTable("mix_messenger_team");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HostId).HasMaxLength(128);

                entity.Property(e => e.IsOpen).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<MixMessengerUser>(entity =>
            {
                entity.ToTable("mix_messenger_user");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FacebookId).HasMaxLength(50);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MixMessengerUserDevice>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.DeviceId });

                entity.ToTable("mix_messenger_user_device");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.DeviceId).HasMaxLength(50);

                entity.Property(e => e.ConnectionId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });
        }
    }
}
