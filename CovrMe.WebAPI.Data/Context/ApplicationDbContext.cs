//using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CovrMe.WebAPI.Data.Context
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<CivilInsurance> CivilInsurances { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<DeliveryInfo> DeliveryInfos { get; set; }

        public virtual DbSet<DocumentsBatch> DocumentsBatches { get; set; }

        public virtual DbSet<GreenCard> GreenCards { get; set; }

        public virtual DbSet<HealthInsurance> HealthInsurances { get; set; }

        public virtual DbSet<Insurance> Insurances { get; set; }

        public virtual DbSet<InsuranceCompany> InsuranceCompanies { get; set; }

        public virtual DbSet<InsurancesPaymentInformation> InsurancesPaymentInformations { get; set; }

        public virtual DbSet<MountainInsurance> MountainInsurances { get; set; }

        public virtual DbSet<MyThingsInsurance> MyThingsInsurances { get; set; }

        public virtual DbSet<PaymentInformation> PaymentInformations { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }

        public virtual DbSet<SocialUser> SocialUsers { get; set; }

        public virtual DbSet<Sticker> Stickers { get; set; }

        public virtual DbSet<TravelInsurance> TravelInsurances { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserPasswordResetCode> UserPasswordResetCodes { get; set; }

        public virtual DbSet<UsersInsurance> UsersInsurances { get; set; }

        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CivilInsurance>(entity =>
            {
                entity.HasIndex(e => e.InsuranceId, "IX_CivilInsurances").IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.FirstInstallmentPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FirstInstallmentTax).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FourthInstallmentDate).HasColumnType("datetime");
                entity.Property(e => e.FourthInstallmentPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FourthInstallmentTax).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SecondInstallmentDate).HasColumnType("datetime");
                entity.Property(e => e.SecondInstallmentPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SecondInstallmentTax).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ThirdInstallmentDate).HasColumnType("datetime");
                entity.Property(e => e.ThirdInstallmentPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ThirdInstallmentTax).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.VehicleBrand).HasMaxLength(256);
                entity.Property(e => e.VehicleModel).HasMaxLength(256);
                entity.Property(e => e.VehiclePlateNumber).HasMaxLength(256);

                entity.HasOne(d => d.GreenCard).WithMany(p => p.CivilInsurances)
                    .HasForeignKey(d => d.GreenCardId)
                    .HasConstraintName("FK_CivilInsurances_GreenCards");

                entity.HasOne(d => d.Insurance).WithOne(p => p.CivilInsurance)
                    .HasForeignKey<CivilInsurance>(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CivilInsurances_Insurances");

                entity.HasOne(d => d.Sticker).WithMany(p => p.CivilInsurances)
                    .HasForeignKey(d => d.StickerId)
                    .HasConstraintName("FK_CivilInsurances_Stickers");

                entity.HasOne(d => d.Vehicle).WithMany(p => p.CivilInsurances)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_CivilInsurances_Vehicles");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Code).HasMaxLength(256);
            });

            modelBuilder.Entity<DeliveryInfo>(entity =>
            {
                entity.ToTable("DeliveryInfo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.City).HasMaxLength(256);
                entity.Property(e => e.Names).HasMaxLength(450);
                entity.Property(e => e.PostalCode).HasMaxLength(256);
                entity.Property(e => e.Region).HasMaxLength(256);
                entity.Property(e => e.SpeedyOfficeId).HasMaxLength(256);
                entity.Property(e => e.Street).HasMaxLength(450);

                entity.HasOne(d => d.User).WithMany(p => p.DeliveryInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryInfo_Users");
            });

            modelBuilder.Entity<DocumentsBatch>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_StickerBatches");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CreationDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.InsuranceCompany).WithMany(p => p.DocumentsBatches)
                    .HasForeignKey(d => d.InsuranceCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StickerBatches_InsuranceCompanies");
            });

            modelBuilder.Entity<GreenCard>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.GreenCardNumber).HasMaxLength(256);

                entity.HasOne(d => d.DocumentsBatch).WithMany(p => p.GreenCards)
                    .HasForeignKey(d => d.DocumentsBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GreenCards_DocumentsBatches1");
            });

            modelBuilder.Entity<HealthInsurance>(entity =>
            {
                entity.HasIndex(e => e.InsuranceId, "IX_HealthInsurances").IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.InstallmentPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Insurance).WithOne(p => p.HealthInsurance)
                    .HasForeignKey<HealthInsurance>(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HealthInsurances_Insurances");
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
                entity.Property(e => e.CurrentEndDate).HasColumnType("datetime");
                entity.Property(e => e.InstallmentsNumber).HasDefaultValue(1);
                entity.Property(e => e.PolicyEndDate).HasColumnType("datetime");
                entity.Property(e => e.PolicyNo).HasMaxLength(256);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Currency).WithMany(p => p.Insurances)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Insurances_Currencies");

                entity.HasOne(d => d.InsuranceCompany).WithMany(p => p.Insurances)
                    .HasForeignKey(d => d.InsuranceCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Insurances_InsuranceCompanies");
            });

            modelBuilder.Entity<InsuranceCompany>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CompanyName).HasMaxLength(256);
            });

            modelBuilder.Entity<InsurancesPaymentInformation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Insurance).WithMany(p => p.InsurancesPaymentInformations)
                    .HasForeignKey(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InsurancesPaymentInformations_Insurances");

                entity.HasOne(d => d.PaymentInformation).WithMany(p => p.InsurancesPaymentInformations)
                    .HasForeignKey(d => d.PaymentInformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InsurancesPaymentInformations_PaymentInformations");
            });

            modelBuilder.Entity<MountainInsurance>(entity =>
            {
                entity.HasIndex(e => e.InsuranceId, "IX_MountainInsurances").IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CompensationAmount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Insurance).WithOne(p => p.MountainInsurance)
                    .HasForeignKey<MountainInsurance>(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MountainInsurances_Insurances");
            });

            modelBuilder.Entity<MyThingsInsurance>(entity =>
            {
                entity.HasIndex(e => e.InsuranceId, "IX_MyThingsInsurances").IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Brand).HasMaxLength(256);
                entity.Property(e => e.InsuranceSum).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Model).HasMaxLength(256);

                entity.HasOne(d => d.Insurance).WithOne(p => p.MyThingsInsurance)
                    .HasForeignKey<MyThingsInsurance>(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MyThingsInsurances_Insurances");
            });

            modelBuilder.Entity<PaymentInformation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.DskOrderNumber).HasMaxLength(256);
                entity.Property(e => e.LocalOrderNumber).HasMaxLength(256);
                entity.Property(e => e.Operation).HasMaxLength(256);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Answer).HasMaxLength(256);

                entity.HasOne(d => d.MyThingsInsurance).WithMany(p => p.Questions)
                    .HasForeignKey(d => d.MyThingsInsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_MyThingsInsurances");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Code).HasMaxLength(256);
                entity.Property(e => e.Value).HasMaxLength(256);
            });

            modelBuilder.Entity<SocialUser>(entity =>
            {
                entity.HasKey(e => e.SocialUserId).HasName("PK__SocialUs__2D6855EFF1B93C18");

                entity.HasIndex(e => new { e.Provider, e.ProviderId }, "IX_SocialUsers_Provider_ProviderId").IsUnique();

                entity.Property(e => e.SocialUserId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.AspUserId).HasMaxLength(450);
                entity.Property(e => e.Provider).HasMaxLength(100);
            });

            modelBuilder.Entity<Sticker>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.StickerNumber).HasMaxLength(256);

                entity.HasOne(d => d.DocumentsBatch).WithMany(p => p.Stickers)
                    .HasForeignKey(d => d.DocumentsBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stickers_DocumentsBatches");
            });

            modelBuilder.Entity<TravelInsurance>(entity =>
            {
                entity.HasIndex(e => e.InsuranceId, "IX_TravelInsurances").IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CompensationAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TravelPolicyType).HasDefaultValue((byte)9);

                entity.HasOne(d => d.Insurance).WithOne(p => p.TravelInsurance)
                    .HasForeignKey<TravelInsurance>(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TravelInsurances_Insurances");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Address).HasMaxLength(256);
                entity.Property(e => e.AspNetUserId).HasMaxLength(450);
                entity.Property(e => e.BirthDate).HasColumnType("datetime");
                entity.Property(e => e.CityId).HasMaxLength(256);
                entity.Property(e => e.CompanyName).HasMaxLength(256);
                entity.Property(e => e.CountryId).HasMaxLength(256);
                entity.Property(e => e.DrivingExperience).HasDefaultValue(0);
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.FirstName).HasMaxLength(256);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.LastName).HasMaxLength(256);
                entity.Property(e => e.LatinAddress).HasMaxLength(256);
                entity.Property(e => e.LatinCompanyName).HasMaxLength(256);
                entity.Property(e => e.LatinFirstName).HasMaxLength(256);
                entity.Property(e => e.LatinLastName).HasMaxLength(256);
                entity.Property(e => e.LatinSurName).HasMaxLength(256);
                entity.Property(e => e.MuniciplalityId).HasMaxLength(256);
                entity.Property(e => e.PhoneNumber).HasMaxLength(256);
                entity.Property(e => e.PostCode).HasMaxLength(256);
                entity.Property(e => e.RegionId).HasMaxLength(256);
                entity.Property(e => e.SurName).HasMaxLength(256);
                entity.Property(e => e.UiNumber).HasMaxLength(256);
                entity.Property(e => e.VatNumber).HasMaxLength(256);

                entity.HasOne(d => d.ParentUser).WithMany(p => p.InverseParentUser)
                    .HasForeignKey(d => d.ParentUserId)
                    .HasConstraintName("FK_Users_Users");
            });

            modelBuilder.Entity<UsersInsurance>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.IsMainUser).HasDefaultValue(true);

                entity.HasOne(d => d.Insurance).WithMany(p => p.UsersInsurances)
                    .HasForeignKey(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersInsurances_Insurances");

                entity.HasOne(d => d.User).WithMany(p => p.UsersInsurances)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersInsurances_Users");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasIndex(e => new { e.PlateNumber, e.RegistrationCertificateNumber }, "constr_uniquePlateNumber").IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.BatteryCapacity).HasDefaultValue(0);
                entity.Property(e => e.BodyType).HasDefaultValue((byte)0);
                entity.Property(e => e.Brand).HasMaxLength(256);
                entity.Property(e => e.Color).HasDefaultValue((byte)0);
                entity.Property(e => e.EngineType).HasDefaultValue((byte)0);
                entity.Property(e => e.EngineVolume).HasDefaultValue(0);
                entity.Property(e => e.FirstRegistrationDate).HasColumnType("datetime");
                entity.Property(e => e.GrossWeight).HasDefaultValue(0);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.Model).HasMaxLength(256);
                entity.Property(e => e.NetWeight).HasDefaultValue(0);
                entity.Property(e => e.Places).HasDefaultValue((byte)0);
                entity.Property(e => e.PlateNumber).HasMaxLength(256);
                entity.Property(e => e.RegistrationCertificateNumber).HasMaxLength(256);
                entity.Property(e => e.SteeringWheel).HasDefaultValue((byte)0);
                entity.Property(e => e.VehicleKilowatts).HasDefaultValue(0);
                entity.Property(e => e.VehicleType).HasMaxLength(256);
                entity.Property(e => e.VehicleUsage).HasMaxLength(256);
                entity.Property(e => e.Vin).HasMaxLength(256);

                entity.HasOne(d => d.User).WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicles_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
