using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RocketSystem.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using RocketSystem.Migrations;

namespace RocketSystem.DbLink
{
    public class DataAccessLayer: DbContext
    {
        public DataAccessLayer() : base("connectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessLayer, Configuration>());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Kalamanakaru> kalamanakarus { get; set; }
        public DbSet<Bonus> Bonuss { get; set; }
        public DbSet<BonusDetail> BonusDetails { get; set; }
        public DbSet<CsvData> CsvDatas { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PositionDetail> PositionDetails { get; set; }
        public DbSet<RejectedPosition> RejectedPositions { get; set; }
        public DbSet<StageFive> StageFives { get; set; }
        public DbSet<StageFour> StageFours { get; set; }
        public DbSet<StageOne> StageOnes { get; set; }
        public DbSet<StageThree> StageThrees { get; set; }
        public DbSet<StageTwo> StageTwoes { get; set; }
        public DbSet<TemporaryPosition> TemporaryPositions { get; set; }
        public DbSet<BalanceCsvTransaction> BalanceCsvTransactions { get; set; }
        public DbSet<LastPositionDetail> LastPositionDetails { get; set; }
        public DbSet<MemberBalanceTransaction> MemberBalanceTransactions { get; set; }
        public DbSet<PaidBonus> PaidBonuss { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<ForgetQuestion> ForgetQuestions { get; set; }
        public DbSet<UserSuspendHistory> UserSuspendHistorys { get; set; }
        public DbSet<CompaniesType> CompaniesTypes { get; set; }
        public DbSet<CsvPositionDetail> CsvPositionDetails { get; set; }
        public DbSet<CoolingOff> CoolingOffs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<RocketSystem.Models.ChangePasswordViewModel> ChangePasswordViewModels { get; set; }

        public System.Data.Entity.DbSet<RocketSystem.Models.LoginAsFourssh> LoginAsFoursshes { get; set; }
    }
}