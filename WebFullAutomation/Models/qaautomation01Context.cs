using System;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using Microsoft.EntityFrameworkCore;

namespace AirSoftAutomationFramework.Models;

public partial class QaAutomation01Context : DbContext
{
    public QaAutomation01Context()
    {
    }

    public QaAutomation01Context(DbContextOptions<QaAutomation01Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DepositDetail> DepositDetails { get; set; }

    public virtual DbSet<FundsSnapshot> FundsSnapshots { get; set; }

    public virtual DbSet<FundsTransaction> FundsTransactions { get; set; }

    public virtual DbSet<MassTrade> MassTrades { get; set; }

    public virtual DbSet<MassTradeReport> MassTradeReports { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    public virtual DbSet<SnapshotQueue> SnapshotQueues { get; set; }

    public virtual DbSet<Trade> Trades { get; set; }

    public virtual DbSet<TradesView> TradesViews { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserAccountInfo> UserAccountInfos { get; set; }

    public virtual DbSet<UsersSavingAccount> UsersSavingAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var sqlDbConnectionString = Config.appSettings.SqlConnectionString;

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(sqlDbConnectionString,
                new MySqlServerVersion(new Version(8, 0, 11)));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<DepositDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("deposit_details")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.DodId, "dod_id");

            entity.HasIndex(e => e.DodType, "dod_type");

            entity.HasIndex(e => e.FundsTransactionsId, "funds_transactions_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuyerAddress)
                .HasMaxLength(255)
                .HasColumnName("buyer_address");
            entity.Property(e => e.BuyerBirthday).HasColumnName("buyer_birthday");
            entity.Property(e => e.BuyerCity)
                .HasMaxLength(255)
                .HasColumnName("buyer_city");
            entity.Property(e => e.BuyerCountry)
                .HasMaxLength(4)
                .HasColumnName("buyer_country");
            entity.Property(e => e.BuyerEmail)
                .HasMaxLength(255)
                .HasColumnName("buyer_email");
            entity.Property(e => e.BuyerFirstname)
                .HasMaxLength(50)
                .HasColumnName("buyer_firstname");
            entity.Property(e => e.BuyerIpAddress)
                .HasMaxLength(20)
                .HasColumnName("buyer_ip_address");
            entity.Property(e => e.BuyerLastname)
                .HasMaxLength(50)
                .HasColumnName("buyer_lastname");
            entity.Property(e => e.BuyerPhone)
                .HasMaxLength(255)
                .HasColumnName("buyer_phone");
            entity.Property(e => e.BuyerState)
                .HasMaxLength(4)
                .HasColumnName("buyer_state");
            entity.Property(e => e.BuyerZip)
                .HasMaxLength(50)
                .HasColumnName("buyer_zip");
            entity.Property(e => e.CustomField)
                .HasMaxLength(255)
                .HasColumnName("custom_field");
            entity.Property(e => e.DefaultKyc)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("default_kyc");
            entity.Property(e => e.DodId)
                .HasMaxLength(30)
                .HasColumnName("dod_id");
            entity.Property(e => e.DodType)
                .HasMaxLength(25)
                .HasColumnName("dod_type");
            entity.Property(e => e.DodUrl)
                .HasMaxLength(255)
                .HasColumnName("dod_url");
            entity.Property(e => e.FundsTransactionsId)
                .HasColumnType("int(11)")
                .HasColumnName("funds_transactions_id");
            entity.Property(e => e.KycCreditDebitCardDocumentation)
                .HasColumnType("text")
                .HasColumnName("kyc_credit_debit_card_documentation");
            entity.Property(e => e.KycCreditDebitCardDocumentationStatus)
                .HasMaxLength(30)
                .HasColumnName("kyc_credit_debit_card_documentation_status");
            entity.Property(e => e.KycProofOfIdentity)
                .HasColumnType("text")
                .HasColumnName("kyc_proof_of_identity");
            entity.Property(e => e.KycProofOfIdentityStatus)
                .HasMaxLength(30)
                .HasColumnName("kyc_proof_of_identity_status");
            entity.Property(e => e.KycProofOfResidency)
                .HasColumnType("text")
                .HasColumnName("kyc_proof_of_residency");
            entity.Property(e => e.KycProofOfResidencyStatus)
                .HasMaxLength(30)
                .HasColumnName("kyc_proof_of_residency_status");
        });

        modelBuilder.Entity<FundsSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("funds_snapshot");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AccountValue).HasColumnName("account_value");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.FundsTransactionId)
                .HasColumnType("int(11)")
                .HasColumnName("funds_transaction_id");
            entity.Property(e => e.IsChargeback)
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_chargeback");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_deleted");
            entity.Property(e => e.SnapDate)
                .HasColumnType("datetime")
                .HasColumnName("snap_date");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasColumnName("type");
            entity.Property(e => e.UserId)
                .HasMaxLength(24)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<FundsTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("funds_transactions")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.ErpUserId, "erp_user_id");

            entity.HasIndex(e => e.ErpUserIdAssigned, "erp_user_id_assigned");

            entity.HasIndex(e => e.ExpiryDate, "expiry_date");

            entity.HasIndex(e => e.IsDeleted, "is_deleted");

            entity.HasIndex(e => e.Status, "status");

            entity.HasIndex(e => e.TransactionRefId, "transaction_ref_id");

            entity.HasIndex(e => e.Type, "type");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AccountValue).HasColumnName("account_value");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CampaignId)
                .HasMaxLength(24)
                .HasColumnName("campaign_id");
            entity.Property(e => e.CantBeDeleted)
                .HasColumnType("tinyint(4)")
                .HasColumnName("cant_be_deleted");
            entity.Property(e => e.Cctype)
                .HasMaxLength(255)
                .HasColumnName("cctype");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnName("currency");
            entity.Property(e => e.DeleteDate)
                .HasColumnType("timestamp")
                .HasColumnName("delete_date");
            entity.Property(e => e.DeleteErpUserId)
                .HasMaxLength(24)
                .HasColumnName("delete_erp_user_id");
            entity.Property(e => e.DeleteReason)
                .HasMaxLength(255)
                .HasColumnName("delete_reason");
            entity.Property(e => e.ErpUserId)
                .HasMaxLength(24)
                .HasColumnName("erp_user_id");
            entity.Property(e => e.ErpUserIdAssigned)
                .HasMaxLength(24)
                .HasColumnName("erp_user_id_assigned");
            entity.Property(e => e.EurFixedAmount).HasColumnName("eur_fixed_amount");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("timestamp")
                .HasColumnName("expiry_date");
            entity.Property(e => e.FirstDigits)
                .HasColumnType("int(11)")
                .HasColumnName("first_digits");
            entity.Property(e => e.Ftd).HasColumnName("ftd");
            entity.Property(e => e.IsChargeback)
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_chargeback");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastDigits)
                .HasMaxLength(4)
                .HasColumnName("last_digits");
            entity.Property(e => e.ManualErpUserId)
                .HasMaxLength(24)
                .HasColumnName("manual_erp_user_id");
            entity.Property(e => e.Method)
                .HasMaxLength(255)
                .HasComment("describe the action that made (psp name / chargeback / title on manual deposits etc.)")
                .HasColumnName("method");
            entity.Property(e => e.Note)
                .HasMaxLength(150)
                .HasColumnName("note");
            entity.Property(e => e.OrderId)
                .HasMaxLength(255)
                .HasColumnName("order_id");
            entity.Property(e => e.OriginalAmount).HasColumnName("original_amount");
            entity.Property(e => e.OriginalCurrency)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnName("original_currency");
            entity.Property(e => e.PspInstanceId)
                .HasMaxLength(24)
                .HasColumnName("psp_instance_id");
            entity.Property(e => e.PspTransactionId)
                .HasColumnType("text")
                .HasColumnName("psp_transaction_id");
            entity.Property(e => e.RejectReason)
                .HasMaxLength(800)
                .HasColumnName("reject_reason");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("'pending'")
                .HasComment("'approved','rejected','pending','in_process','canceled','error'")
                .HasColumnType("enum('approved','rejected','pending','in_process','canceled','error')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
            entity.Property(e => e.TransactionRefId)
                .HasComment("used for knowing delete and chargebcaks  original ids")
                .HasColumnType("int(11)")
                .HasColumnName("transaction_ref_id");
            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("psp | bank_transfer | wallet_to_wallet | demo | bonus")
                .HasColumnName("transaction_type");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("deposit,delete_deposit,deposit_demo,delete_deposit_bonus,deposit_bonus,withdrawal,chargeback")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(24)
                .HasColumnName("user_id");
            entity.Property(e => e.WithdrawalData)
                .HasColumnType("text")
                .HasColumnName("withdrawal_data");
            entity.Property(e => e.WithdrawalProceedDate)
                .HasColumnType("timestamp")
                .HasColumnName("withdrawal_proceed_date");
        });

        modelBuilder.Entity<MassTrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("mass_trade")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AssetSymbol)
                .HasMaxLength(150)
                .HasColumnName("asset_symbol");
            entity.Property(e => e.ClosedAt)
                .HasColumnType("timestamp")
                .HasColumnName("closed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Exposure)
                .HasColumnType("float unsigned")
                .HasColumnName("exposure");
            entity.Property(e => e.IsPending).HasColumnName("is_pending");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.StopLoss).HasColumnName("stop_loss");
            entity.Property(e => e.TakeProfit).HasColumnName("take_profit");
            entity.Property(e => e.TotalUsers)
                .HasColumnType("int(11)")
                .HasColumnName("total_users");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");
        });

        modelBuilder.Entity<MassTradeReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("mass_trade_reports")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.MassTradeId, "mass_trade_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Error)
                .HasColumnType("text")
                .HasColumnName("error");
            entity.Property(e => e.MassTradeId)
                .HasColumnType("int(11)")
                .HasColumnName("mass_trade_id");
            entity.Property(e => e.Success)
                .HasDefaultValueSql("'0'")
                .HasColumnName("success");
            entity.Property(e => e.UserId)
                .HasMaxLength(24)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("quotes");

            entity.HasIndex(e => e.AssetSymbol, "asset_symbol").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AssetSymbol)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("asset_symbol");
            entity.Property(e => e.BuyPrice).HasColumnName("buy_price");
            entity.Property(e => e.DecimalDigits)
                .HasDefaultValueSql("'12'")
                .HasColumnType("int(10)")
                .HasColumnName("decimal_digits");
            entity.Property(e => e.SellPrice).HasColumnName("sell_price");
        });

        modelBuilder.Entity<SnapshotQueue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("snapshot_queue");

            entity.HasIndex(e => e.UserId, "user_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.UserId)
                .HasMaxLength(24)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("trades")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.AssetSymbol, "asset_symbol");

            entity.HasIndex(e => e.ChronoLeverage, "chrono_leverage");

            entity.HasIndex(e => e.ChronoTrade, "chrono_trade");

            entity.HasIndex(e => e.Error, "error");

            entity.HasIndex(e => e.ExpireSent, "expire_sent");

            entity.HasIndex(e => e.IsNft, "is_nft");

            entity.HasIndex(e => e.LastSwapDateCharge, "last_swap_date_charge");

            entity.HasIndex(e => e.MassTradeId, "mass_trade_id");

            entity.HasIndex(e => e.Platform, "platform");

            entity.HasIndex(e => e.RollOverSent, "roll_over_sent");

            entity.HasIndex(e => e.SpreadOnOpen, "spread_on_open");

            entity.HasIndex(e => e.Status, "status");

            entity.HasIndex(e => e.SwapTime, "swap_time");

            entity.HasIndex(e => e.TradeTimeEnd, "trade_time_end");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("float unsigned")
                .HasColumnName("amount");
            entity.Property(e => e.AssetSymbol)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("asset_symbol");
            entity.Property(e => e.ChronoLeverage).HasColumnName("chrono_leverage");
            entity.Property(e => e.ChronoTrade).HasColumnName("chrono_trade");
            entity.Property(e => e.CloseAtLoss)
                .HasDefaultValueSql("'0'")
                .HasColumnName("close_at_loss");
            entity.Property(e => e.CloseAtProfit)
                .HasDefaultValueSql("'0'")
                .HasColumnName("close_at_profit");
            entity.Property(e => e.CloseReason)
                .HasMaxLength(50)
                .HasColumnName("close_reason");
            entity.Property(e => e.ClosedProfitLoss)
                .HasDefaultValueSql("'0'")
                .HasColumnName("closed_profit_loss");
            entity.Property(e => e.ClosedRate).HasColumnName("closed_rate");
            entity.Property(e => e.Commision).HasColumnName("commision");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("currency");
            entity.Property(e => e.Demo).HasColumnName("demo");
            entity.Property(e => e.Error)
                .HasMaxLength(45)
                .HasColumnName("error");
            entity.Property(e => e.ExpireSent).HasColumnName("expire_sent");
            entity.Property(e => e.Investment)
                .HasComment("the real amount in the user currency")
                .HasColumnName("investment");
            entity.Property(e => e.IsNft)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("is_nft");
            entity.Property(e => e.LastSwapDateCharge)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("last_swap_date_charge");
            entity.Property(e => e.Leverage).HasColumnName("leverage");
            entity.Property(e => e.LossingProfitPercent).HasColumnName("lossing_profit_percent");
            entity.Property(e => e.Maintenance).HasColumnName("maintenance");
            entity.Property(e => e.MassTradeId)
                .HasColumnType("int(11)")
                .HasColumnName("mass_trade_id");
            entity.Property(e => e.MinMargin).HasColumnName("min_margin");
            entity.Property(e => e.MinimumAmount).HasColumnName("minimum_amount");
            entity.Property(e => e.MinimumStep).HasColumnName("minimum_step");
            entity.Property(e => e.OnAssetOpen).HasColumnName("on_asset_open");
            entity.Property(e => e.OriginalConversionRate).HasColumnName("original_conversion_rate");
            entity.Property(e => e.OriginalCurrency)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("original_currency");
            entity.Property(e => e.PendingRateHigher)
                .HasComment("on pending trades: indicates whether the order rate was higher than the current rate")
                .HasColumnName("pending_rate_higher");
            entity.Property(e => e.Platform)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("binary,cfd,crypto")
                .HasColumnName("platform");
            entity.Property(e => e.Rate)
                .HasComment("the rate price it was bought or wanted to buy on pending")
                .HasColumnName("rate");
            entity.Property(e => e.RollOverSent).HasColumnName("roll_over_sent");
            entity.Property(e => e.SchedulerId)
                .HasMaxLength(24)
                .HasColumnName("scheduler_id");
            entity.Property(e => e.Spread).HasColumnName("spread");
            entity.Property(e => e.SpreadOnOpen).HasColumnName("spread_on_open");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("pending,open,close")
                .HasColumnName("status");
            entity.Property(e => e.StopLoss).HasColumnName("stop_loss");
            entity.Property(e => e.SwapCommission).HasColumnName("swap_commission");
            entity.Property(e => e.SwapLong).HasColumnName("swap_long");
            entity.Property(e => e.SwapShort).HasColumnName("swap_short");
            entity.Property(e => e.SwapTime)
                .HasColumnType("time")
                .HasColumnName("swap_time");
            entity.Property(e => e.TradeCloseTime)
                .HasColumnType("datetime")
                .HasColumnName("trade_close_time");
            entity.Property(e => e.TradeTimeEnd)
                .HasColumnType("timestamp")
                .HasColumnName("trade_time_end");
            entity.Property(e => e.TradeTimeStart)
                .HasColumnType("timestamp")
                .HasColumnName("trade_time_start");
            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("buy,sell")
                .HasColumnName("transaction_type");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(24)
                .HasColumnName("user_id");
            entity.Property(e => e.WinningProfitPercent).HasColumnName("winning_profit_percent");
            entity.Property(e => e.WinningStatus).HasColumnName("winning_status");
        });

        modelBuilder.Entity<TradesView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("trades_view");

            entity.Property(e => e.Amount)
                .HasColumnType("float unsigned")
                .HasColumnName("amount");
            entity.Property(e => e.AssetSymbol)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("asset_symbol")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.ChronoLeverage).HasColumnName("chrono_leverage");
            entity.Property(e => e.ChronoTrade).HasColumnName("chrono_trade");
            entity.Property(e => e.CloseAtLoss)
                .HasDefaultValueSql("'0'")
                .HasColumnName("close_at_loss");
            entity.Property(e => e.CloseAtProfit)
                .HasDefaultValueSql("'0'")
                .HasColumnName("close_at_profit");
            entity.Property(e => e.CloseReason)
                .HasMaxLength(50)
                .HasColumnName("close_reason")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.ClosedProfitLoss)
                .HasDefaultValueSql("'0'")
                .HasColumnName("closed_profit_loss");
            entity.Property(e => e.ClosedRate).HasColumnName("closed_rate");
            entity.Property(e => e.Commision).HasColumnName("commision");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("currency")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.CurrentRate).HasColumnName("current_rate");
            entity.Property(e => e.Demo).HasColumnName("demo");
            entity.Property(e => e.Error)
                .HasMaxLength(45)
                .HasColumnName("error")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.ExpireSent).HasColumnName("expire_sent");
            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Investment)
                .HasComment("the real amount in the user currency")
                .HasColumnName("investment");
            entity.Property(e => e.IsNft)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("is_nft");
            entity.Property(e => e.LastSwapDateCharge)
                .HasColumnType("timestamp")
                .HasColumnName("last_swap_date_charge");
            entity.Property(e => e.Leverage).HasColumnName("leverage");
            entity.Property(e => e.LossingProfitPercent).HasColumnName("lossing_profit_percent");
            entity.Property(e => e.Maintenance).HasColumnName("maintenance");
            entity.Property(e => e.MassTradeId)
                .HasColumnType("int(11)")
                .HasColumnName("mass_trade_id");
            entity.Property(e => e.MinMargin).HasColumnName("min_margin");
            entity.Property(e => e.MinimumAmount).HasColumnName("minimum_amount");
            entity.Property(e => e.MinimumStep).HasColumnName("minimum_step");
            entity.Property(e => e.OnAssetOpen).HasColumnName("on_asset_open");
            entity.Property(e => e.OriginalConversionRate).HasColumnName("original_conversion_rate");
            entity.Property(e => e.OriginalCurrency)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("original_currency")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.PendingRateHigher)
                .HasComment("on pending trades: indicates whether the order rate was higher than the current rate")
                .HasColumnName("pending_rate_higher");
            entity.Property(e => e.Platform)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("binary,cfd,crypto")
                .HasColumnName("platform")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.ProfitLoss).HasColumnName("profit_loss");
            entity.Property(e => e.Rate)
                .HasComment("the rate price it was bought or wanted to buy on pending")
                .HasColumnName("rate");
            entity.Property(e => e.RollOverSent).HasColumnName("roll_over_sent");
            entity.Property(e => e.SchedulerId)
                .HasMaxLength(24)
                .HasColumnName("scheduler_id")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.Spread).HasColumnName("spread");
            entity.Property(e => e.SpreadOnOpen).HasColumnName("spread_on_open");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("pending,open,close")
                .HasColumnName("status")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.StopLoss).HasColumnName("stop_loss");
            entity.Property(e => e.SwapCommission).HasColumnName("swap_commission");
            entity.Property(e => e.SwapLong).HasColumnName("swap_long");
            entity.Property(e => e.SwapShort).HasColumnName("swap_short");
            entity.Property(e => e.SwapTime)
                .HasColumnType("time")
                .HasColumnName("swap_time");
            entity.Property(e => e.TradeCloseTime)
                .HasColumnType("datetime")
                .HasColumnName("trade_close_time");
            entity.Property(e => e.TradeTimeEnd)
                .HasColumnType("timestamp")
                .HasColumnName("trade_time_end");
            entity.Property(e => e.TradeTimeStart)
                .HasColumnType("timestamp")
                .HasColumnName("trade_time_start");
            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("buy,sell")
                .HasColumnName("transaction_type")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(24)
                .HasColumnName("user_id")
                .UseCollation("utf8_bin")
                .HasCharSet("utf8");
            entity.Property(e => e.WinningProfitPercent).HasColumnName("winning_profit_percent");
            entity.Property(e => e.WinningStatus).HasColumnName("winning_status");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("user_account")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.BalanceUpdate, "balance_update");

            entity.HasIndex(e => e.CreatedAt, "created_at");

            entity.HasIndex(e => e.ErpUserId, "erp_user_id");

            entity.HasIndex(e => e.MarginCallNotifiedAt, "margin_call_notified_at");

            entity.HasIndex(e => e.NeedToUpdateBalance, "need_to_update_balance");

            entity.HasIndex(e => e.PauseTrades, "pause_trades");

            entity.HasIndex(e => e.SavingAccountCurrentAmount, "saving_account_current_amount");

            entity.Property(e => e.UserId)
                .HasMaxLength(24)
                .HasColumnName("user_id");
            entity.Property(e => e.Available)
                .HasComputedColumnSql("((((`funds_transactions` + `closed_profit_loss`) - `open_investments`) + `open_profit_loss`) + `saving_account_withdrawals`) - `saving_account_deposits`", false)
                .HasColumnName("available");
            entity.Property(e => e.Balance)
                .HasComputedColumnSql("((`funds_transactions` + `closed_profit_loss`) + `saving_account_withdrawals`) - `saving_account_deposits`", false)
                .HasColumnName("balance");
            entity.Property(e => e.BalanceUpdate)
                .HasColumnType("timestamp")
                .HasColumnName("balance_update");
            entity.Property(e => e.Bonus).HasColumnName("bonus");
            entity.Property(e => e.ClosedProfitLoss).HasColumnName("closed_profit_loss");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("currency");
            entity.Property(e => e.DemoAvailable)
                .HasComputedColumnSql("((`demo_funds_transactions` + `demo_closed_profit_loss`) - `demo_open_investments`) + `demo_open_profit_loss`", false)
                .HasColumnName("demo_available");
            entity.Property(e => e.DemoBalance)
                .HasComputedColumnSql("`demo_funds_transactions` + `demo_closed_profit_loss`", false)
                .HasColumnName("demo_balance");
            entity.Property(e => e.DemoClosedProfitLoss).HasColumnName("demo_closed_profit_loss");
            entity.Property(e => e.DemoEquity)
                .HasComputedColumnSql("`demo_available` + `demo_open_investments`", false)
                .HasColumnName("demo_equity");
            entity.Property(e => e.DemoFundsTransactions).HasColumnName("demo_funds_transactions");
            entity.Property(e => e.DemoMinMargin).HasColumnName("demo_min_margin");
            entity.Property(e => e.DemoOpenInvestments).HasColumnName("demo_open_investments");
            entity.Property(e => e.DemoOpenProfitLoss).HasColumnName("demo_open_profit_loss");
            entity.Property(e => e.Equity)
                .HasComputedColumnSql("(`available` + `open_investments`) + `saving_account_current_amount`", false)
                .HasColumnName("equity");
            entity.Property(e => e.ErpUserId)
                .HasMaxLength(24)
                .HasColumnName("erp_user_id");
            entity.Property(e => e.FundsTransactions).HasColumnName("funds_transactions");
            entity.Property(e => e.MarginCall)
                .HasColumnType("tinyint(3)")
                .HasColumnName("margin_call");
            entity.Property(e => e.MarginCallNotifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("margin_call_notified_at");
            entity.Property(e => e.MinMargin).HasColumnName("min_margin");
            entity.Property(e => e.NeedToUpdateBalance).HasColumnName("need_to_update_balance");
            entity.Property(e => e.OpenInvestments).HasColumnName("open_investments");
            entity.Property(e => e.OpenProfitLoss).HasColumnName("open_profit_loss");
            entity.Property(e => e.PauseTrades).HasColumnName("pause_trades");
            entity.Property(e => e.SavingAccountCurrentAmount)
                .HasComputedColumnSql("(`saving_account_deposits` - `saving_account_withdrawals`) + `saving_account_profit`", false)
                .HasColumnName("saving_account_current_amount");
            entity.Property(e => e.SavingAccountDeposits).HasColumnName("saving_account_deposits");
            entity.Property(e => e.SavingAccountProfit).HasColumnName("saving_account_profit");
            entity.Property(e => e.SavingAccountProfitLastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("saving_account_profit_last_update");
            entity.Property(e => e.SavingAccountStashedProfit).HasColumnName("saving_account_stashed_profit");
            entity.Property(e => e.SavingAccountWithdrawals).HasColumnName("saving_account_withdrawals");
            entity.Property(e => e.SuspiciousProfitSent).HasColumnName("suspicious_profit_sent");
            entity.Property(e => e.TotalDeposit).HasColumnName("total_deposit");
        });

        modelBuilder.Entity<UserAccountInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("user_account_info")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.AccountTypeId, "account_type_id");

            entity.HasIndex(e => e.CampaignId, "campaign_id");

            entity.HasIndex(e => e.FullName, "full_name");

            entity.HasIndex(e => e.GroupId, "group_id");

            entity.HasIndex(e => e.SavingAccountId, "saving_account_id");

            entity.HasIndex(e => e.UserId, "user_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AccountTypeId)
                .HasMaxLength(24)
                .HasColumnName("account_type_id");
            entity.Property(e => e.ActivationStatus)
                .HasMaxLength(256)
                .HasColumnName("activation_status");
            entity.Property(e => e.CampaignId)
                .HasMaxLength(24)
                .HasColumnName("campaign_id");
            entity.Property(e => e.Country)
                .HasMaxLength(256)
                .HasColumnName("country");
            entity.Property(e => e.DodStatus)
                .HasMaxLength(45)
                .HasColumnName("dod_status");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(256)
                .HasColumnName("first_name");
            entity.Property(e => e.FreeText)
                .HasMaxLength(1200)
                .HasColumnName("free_text");
            entity.Property(e => e.FreeText2)
                .HasMaxLength(1200)
                .HasColumnName("free_text_2");
            entity.Property(e => e.FreeText3)
                .HasMaxLength(1200)
                .HasColumnName("free_text_3");
            entity.Property(e => e.FreeText4)
                .HasMaxLength(1200)
                .HasColumnName("free_text_4");
            entity.Property(e => e.FreeText5)
                .HasMaxLength(1200)
                .HasColumnName("free_text_5");
            entity.Property(e => e.FullName)
                .HasMaxLength(256)
                .HasComputedColumnSql("concat(`first_name`,' ',`last_name`)", false)
                .HasColumnName("full_name");
            entity.Property(e => e.GroupId)
                .HasMaxLength(24)
                .HasColumnName("group_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(256)
                .HasColumnName("last_name");
            entity.Property(e => e.MassTrade)
                .HasMaxLength(256)
                .HasColumnName("mass_trade");
            entity.Property(e => e.Phone)
                .HasMaxLength(45)
                .HasColumnName("phone");
            entity.Property(e => e.SalesStatus)
                .HasMaxLength(256)
                .HasColumnName("sales_status");
            entity.Property(e => e.SavingAccountId)
                .HasMaxLength(24)
                .HasColumnName("saving_account_id");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(24)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<UsersSavingAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("users_saving_accounts")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.ActionType, "action_type");

            entity.HasIndex(e => e.Balance, "balance");

            entity.HasIndex(e => e.SaId, "sa_id");

            entity.HasIndex(e => e.SaPercentage, "sa_percentage");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ActionType)
                .IsRequired()
                .HasMaxLength(12)
                .HasColumnName("action_type");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("'0'")
                .HasColumnName("balance");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.SaId)
                .HasMaxLength(45)
                .HasColumnName("sa_id");
            entity.Property(e => e.SaPercentage)
                .HasDefaultValueSql("'0'")
                .HasColumnName("sa_percentage");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(24)
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
