using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace data.sismo.models
{
    public partial class SeismicContext : DbContext
    {
        private readonly string _conectionString = "data source=DESKTOP-MT0M6S4;initial catalog=Seismic;persist security info=True;user id=Lumen;password=123456;Connection Timeout=0;MultipleActiveResultSets=True;";
        public SeismicContext()
        {
          
        }
        
        public SeismicContext(DbContextOptions<SeismicContext> options, IConfiguration configuration)
            : base(options)
        {
            _conectionString = configuration.GetConnectionString("MyConnection");
        }

        public virtual DbSet<DisplacementRule> DisplacementRules { get; set; }
        public virtual DbSet<FrontGroup> FrontGroups { get; set; }
        public virtual DbSet<FrontGroupLeader> FrontGroupLeaders { get; set; }
        public virtual DbSet<Hole> Holes { get; set; }
        public virtual DbSet<HoleCoordinate> HoleCoordinates { get; set; }
        public virtual DbSet<HolesCoordinatesFile> HolesCoordinatesFiles { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<LocalCpCommandPool> LocalCpCommandPools { get; set; }
        public virtual DbSet<OperationalFront> OperationalFronts { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<ParameterGroup> ParameterGroups { get; set; }
        public virtual DbSet<PlannedStretch> PlannedStretches { get; set; }
        public virtual DbSet<PointProduction> PointProductions { get; set; }
        public virtual DbSet<PosplotCoordinate> PosplotCoordinates { get; set; }
        public virtual DbSet<PreplotLastVersionView> PreplotLastVersionViews { get; set; }
        public virtual DbSet<PreplotPoint> PreplotPoints { get; set; }
        public virtual DbSet<PreplotVersion> PreplotVersions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectBase> ProjectBases { get; set; }
        public virtual DbSet<ProjectBaseImage> ProjectBaseImages { get; set; }
        public virtual DbSet<ProjectExplosiveMaterial> ProjectExplosiveMaterials { get; set; }
        public virtual DbSet<ProjectExplosiveMaterialType> ProjectExplosiveMaterialTypes { get; set; }
        public virtual DbSet<ProspatialReferenceSystem> ProspatialReferenceSystems { get; set; }
        public virtual DbSet<ReductionRule> ReductionRules { get; set; }
        public virtual DbSet<SeismicRegister> SeismicRegisters { get; set; }
        public virtual DbSet<Stretch> Stretches { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyCoordinateSystem> SurveyCoordinateSystems { get; set; }
        public virtual DbSet<SurveyOperationalFront> SurveyOperationalFronts { get; set; }
        public virtual DbSet<SurveyParameter> SurveyParameters { get; set; }
        public virtual DbSet<SurveyPointsOfInterest> SurveyPointsOfInterests { get; set; }
        public virtual DbSet<SurveyProjectBase> SurveyProjectBases { get; set; }
        public virtual DbSet<Swath> Swaths { get; set; }
        public virtual DbSet<SyncSummary> SyncSummaries { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public virtual DbSet<TerrainChargeControl> TerrainChargeControls { get; set; }
        public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(_conectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<DisplacementRule>(entity =>
            {
                entity.ToTable("DisplacementRule");

                entity.HasIndex(e => e.SurveyId, "IX_IdSurvey");

                entity.Property(e => e.Azymuth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Distance).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ImagePath).IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.DisplacementRules)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisplacementRule_Survey");
            });

            modelBuilder.Entity<FrontGroup>(entity =>
            {
                entity.ToTable("FrontGroup");

                entity.HasIndex(e => e.OperationalFrontId, "IX_IdOperationalFront");

                entity.HasOne(d => d.OperationalFront)
                    .WithMany(p => p.FrontGroups)
                    .HasForeignKey(d => d.OperationalFrontId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrontGroup_OperationalFront");
            });

            modelBuilder.Entity<FrontGroupLeader>(entity =>
            {
                entity.ToTable("FrontGroupLeader");

                entity.HasIndex(e => e.OperationalFrontId, "IX_IdOperationalFront");

                entity.HasOne(d => d.OperationalFront)
                    .WithMany(p => p.FrontGroupLeaders)
                    .HasForeignKey(d => d.OperationalFrontId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FrontGroupLeader_OperationalFront");
            });

            modelBuilder.Entity<Hole>(entity =>
            {
                entity.HasKey(e => new { e.HoleNumber, e.PreplotPointId, e.PreplotVersionId, e.SurveyId, e.WorkNumber, e.PreplotPointType, e.OperationalFrontId })
                    .HasName("PK_Hole_1");

                entity.ToTable("Hole");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Depth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.NumberOfCharges).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.NumberOfFuses).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Holes)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hole_Survey");

                entity.HasOne(d => d.PreplotVersion)
                    .WithMany(p => p.Holes)
                    .HasForeignKey(d => new { d.PreplotVersionId, d.SurveyId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hole_PreplotVersion");

                entity.HasOne(d => d.PreplotPoint)
                    .WithMany(p => p.Holes)
                    .HasForeignKey(d => new { d.PreplotPointId, d.PreplotVersionId, d.SurveyId, d.PreplotPointType })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hole_PreplotPoint");

                entity.HasOne(d => d.PointProduction)
                    .WithMany(p => p.Holes)
                    .HasForeignKey(d => new { d.PreplotPointId, d.PreplotVersionId, d.SurveyId, d.WorkNumber, d.PreplotPointType, d.OperationalFrontId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hole_PointProduction");
            });

            modelBuilder.Entity<HoleCoordinate>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.HoleCoordinateId });

                entity.ToTable("HoleCoordinate");

                entity.Property(e => e.AcquisitionTime).HasColumnType("datetime");

                entity.Property(e => e.Line)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StationNumber).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.HoleCoordinates)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoleCoordinate_HolesCoordinatesFile");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.HoleCoordinates)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HolesCoordinates_Survey");
            });

            modelBuilder.Entity<HolesCoordinatesFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("HolesCoordinatesFile");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UploadTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.LineName, e.PreplotVersionId, e.LinePointsType });

                entity.ToTable("Line");

                entity.HasComment("LT =1; LT = 2; LA =3");

                entity.Property(e => e.LineName)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FinalStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InitialStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TotalKm).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Lines)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Line_Survey");
            });

            modelBuilder.Entity<LocalCpCommandPool>(entity =>
            {
                entity.HasKey(e => e.CommandId);

                entity.ToTable("LocalCpCommandPool");

                entity.Property(e => e.CommandId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Command).HasColumnType("text");

                entity.Property(e => e.CommandType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Keys)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SourceTable)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OperationalFront>(entity =>
            {
                entity.ToTable("OperationalFront");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OperationalFrontColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.HasOne(d => d.PreviousOperationalFront)
                    .WithMany(p => p.InversePreviousOperationalFront)
                    .HasForeignKey(d => d.PreviousOperationalFrontId)
                    .HasConstraintName("FK_OperationalFront_OperationalFront");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.OperationalFronts)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OperationalFront_Project");
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.ToTable("Parameter");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Format)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterOwner)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ParameterType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VarType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParameterGroup)
                    .WithMany(p => p.Parameters)
                    .HasForeignKey(d => d.ParameterGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Parameter_ParameterGroup");
            });

            modelBuilder.Entity<ParameterGroup>(entity =>
            {
                entity.ToTable("ParameterGroup");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlannedStretch>(entity =>
            {
                entity.HasKey(e => new { e.ExecutionDate, e.SurveyId, e.OperationalFrontId, e.InitialStation, e.FinalStation, e.Line });

                entity.ToTable("PlannedStretch");

                entity.Property(e => e.ExecutionDate).HasColumnType("date");

                entity.Property(e => e.InitialStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FinalStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Line)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Km).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PlanningDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.FrontGroup)
                    .WithMany(p => p.PlannedStretches)
                    .HasForeignKey(d => d.FrontGroupId)
                    .HasConstraintName("FK_PlannedStretch_FrontGroup");

                entity.HasOne(d => d.FrontGroupLeader)
                    .WithMany(p => p.PlannedStretches)
                    .HasForeignKey(d => d.FrontGroupLeaderId)
                    .HasConstraintName("FK_PlannedStretch_FrontGroupLeader");

                entity.HasOne(d => d.OperationalFront)
                    .WithMany(p => p.PlannedStretches)
                    .HasForeignKey(d => d.OperationalFrontId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlannedStretch_OperationalFront");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.PlannedStretches)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlannedStretch_Survey");
            });

            modelBuilder.Entity<PointProduction>(entity =>
            {
                entity.HasKey(e => new { e.PreplotPointId, e.PreplotVersionId, e.SurveyId, e.WorkNumber, e.PreplotPointType, e.OperationalFrontId })
                    .HasName("PK_PointProduction_1");

                entity.ToTable("PointProduction");

                entity.HasComment("Campo destinado a informação da ordem dos trabalhos .\r\n1 - Trabalho\r\n2 ... N Retrabalho");

                entity.HasIndex(e => new { e.SurveyId, e.OperationalFrontId, e.Date }, "IDX_POINT_PRODUCTION_PROD");

                entity.HasIndex(e => e.DisplacementRuleId, "IX_IdDisplacementRule");

                entity.HasIndex(e => e.ReductionRuleId, "IX_IdReductionRule");

                entity.HasIndex(e => e.PreplotPointId, "IX_IdStation");

                entity.HasIndex(e => new { e.SurveyId, e.OperationalFrontId, e.Date }, "IX_PointProductionDate");

                entity.Property(e => e.PreplotPointId).HasComment("Para relacionar a produção de um ponto - descoonsiderar a FK PreplotVersioId");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.LastEditorUserLogin)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observation)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.DisplacementRule)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => d.DisplacementRuleId)
                    .HasConstraintName("FK_PointProduction_DisplacementRule");

                entity.HasOne(d => d.FrontGroup)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => d.FrontGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointProduction_FrontGroup");

                entity.HasOne(d => d.FrontGroupLeader)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => d.FrontGroupLeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointProduction_FrontGroupLeader");

                entity.HasOne(d => d.OperationalFront)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => d.OperationalFrontId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointProduction_OperationalFront");

                entity.HasOne(d => d.ReductionRule)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => d.ReductionRuleId)
                    .HasConstraintName("FK_PointProduction_ReductionRule");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointProduction_Survey");

                entity.HasOne(d => d.PreplotVersion)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => new { d.PreplotVersionId, d.SurveyId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointProduction_PreplotVersion");

                entity.HasOne(d => d.PreplotPoint)
                    .WithMany(p => p.PointProductions)
                    .HasForeignKey(d => new { d.PreplotPointId, d.PreplotVersionId, d.SurveyId, d.PreplotPointType })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointProduction_PreplotPoint");
            });

            modelBuilder.Entity<PosplotCoordinate>(entity =>
            {
                entity.HasKey(e => new { e.RegistrationTime, e.SurveyId, e.OperationalFrontId, e.Line, e.StationNumber });

                entity.ToTable("PosplotCoordinate");

                entity.HasIndex(e => e.SurveyId, "PosplotCoordinateIndex");

                entity.Property(e => e.RegistrationTime).HasColumnType("datetime");

                entity.Property(e => e.Line)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StationNumber).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatorUserLogin)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LinePreplot)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StationNumberPreplot).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TopographyDescription)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.PosplotCoordinates)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PosplotCoordinate_Survey");
            });

            modelBuilder.Entity<PreplotLastVersionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PreplotLastVersion_View");

                entity.Property(e => e.Gid).HasColumnName("gid");

                entity.Property(e => e.Line)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StationNumber).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<PreplotPoint>(entity =>
            {
                entity.HasKey(e => new { e.PreplotPointId, e.PreplotVersionId, e.SurveyId, e.PreplotPointType })
                    .HasName("PK_PreplotPoint_1");

                entity.ToTable("PreplotPoint");

                entity.HasIndex(e => e.PreplotVersionId, "IX_IdPreplotVersion");

                entity.HasIndex(e => new { e.SurveyId, e.PreplotPointType }, "IX_PreplotPoint");

                entity.Property(e => e.Line)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StationNumber).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.PreplotPoints)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PreplotPoint_Survey");

                entity.HasOne(d => d.PreplotVersion)
                    .WithMany(p => p.PreplotPoints)
                    .HasForeignKey(d => new { d.PreplotVersionId, d.SurveyId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PreplotPoint_PreplotVersion");
            });

            modelBuilder.Entity<PreplotVersion>(entity =>
            {
                entity.HasKey(e => new { e.PreplotVersionId, e.SurveyId })
                    .HasName("PK_dbo.PreplotVersion");

                entity.ToTable("PreplotVersion");

                entity.HasIndex(e => e.SurveyId, "IX_IdSurvey");

                entity.Property(e => e.Comment).IsRequired();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.HasIndex(e => e.Name, "uk_Name")
                    .IsUnique();

                entity.Property(e => e.ClientName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cr)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CR");

                entity.Property(e => e.Crobservation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CRObservation");

                entity.Property(e => e.DateEnd).HasColumnType("date");

                entity.Property(e => e.DateIni).HasColumnType("date");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ProjectBase>(entity =>
            {
                entity.HasKey(e => e.BaseId);

                entity.ToTable("ProjectBase");

                entity.Property(e => e.BaseName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImageFolder)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectBases)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectBase_Project");
            });

            modelBuilder.Entity<ProjectBaseImage>(entity =>
            {
                entity.HasKey(e => new { e.ProjectBaseId, e.Image });

                entity.ToTable("ProjectBaseImage");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProjectBase)
                    .WithMany(p => p.ProjectBaseImages)
                    .HasForeignKey(d => d.ProjectBaseId)
                    .HasConstraintName("FK_ProjectBaseImage_ProjectBase");
            });

            modelBuilder.Entity<ProjectExplosiveMaterial>(entity =>
            {
                entity.HasKey(e => e.EntryId);

                entity.ToTable("ProjectExplosiveMaterial");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DifferenceDate).HasColumnType("date");

                entity.Property(e => e.Invoice)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Observation)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TrafficGuide)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Volume).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ProjectExplosiveMaterialType)
                    .WithMany(p => p.ProjectExplosiveMaterials)
                    .HasForeignKey(d => d.ProjectExplosiveMaterialTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectExplosiveMaterial_ProjectExplosiveMaterialType");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectExplosiveMaterials)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectExplosiveMaterial_Project");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.ProjectExplosiveMaterials)
                    .HasForeignKey(d => d.SurveyId)
                    .HasConstraintName("FK_ProjectExplosiveMaterial_Survey");
            });

            modelBuilder.Entity<ProjectExplosiveMaterialType>(entity =>
            {
                entity.ToTable("ProjectExplosiveMaterialType");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Unity)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Volume).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectExplosiveMaterialTypes)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectExplosiveMaterialType_Project");
            });

            modelBuilder.Entity<ProspatialReferenceSystem>(entity =>
            {
                entity.HasKey(e => e.SpatialReferenceId);

                entity.ToTable("prospatial_reference_systems");

                entity.Property(e => e.SpatialReferenceId)
                    .ValueGeneratedNever()
                    .HasColumnName("spatial_reference_id");

                entity.Property(e => e.Geogcs)
                    .HasMaxLength(50)
                    .HasColumnName("GEOGCS")
                    .IsFixedLength(true);

                entity.Property(e => e.WellKnownText).HasColumnName("well_known_text");
            });

            modelBuilder.Entity<ReductionRule>(entity =>
            {
                entity.ToTable("ReductionRule");

                entity.HasIndex(e => e.SurveyId, "IX_IdSurvey");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DistanceBetweenHoles).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.ReductionRules)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.ReductionRule_Survey");
            });

            modelBuilder.Entity<SeismicRegister>(entity =>
            {
                entity.HasKey(e => new { e.Ffid, e.SurveyId });

                entity.ToTable("SeismicRegister");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Itb).HasColumnName("ITB");

                entity.Property(e => e.LineName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PointNumber).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Swath)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpholeTime).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.PointProduction)
                    .WithMany(p => p.SeismicRegisters)
                    .HasForeignKey(d => new { d.PreplotPointId, d.PreplotVersionId, d.SurveyId, d.WorkNumber, d.PreplotPointType, d.OperationalFrontId })
                    .HasConstraintName("FK_SeismicRegister_PointProduction");
            });

            modelBuilder.Entity<Stretch>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.SurveyId, e.OperationalFrontId, e.FrontGroupLeaderId, e.FrontGroupId, e.InitialStation, e.FinalStation, e.Line });

                entity.ToTable("Stretch");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InitialStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FinalStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Line)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Km).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.FrontGroup)
                    .WithMany(p => p.Stretches)
                    .HasForeignKey(d => d.FrontGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stretch_FrontGroup");

                entity.HasOne(d => d.FrontGroupLeader)
                    .WithMany(p => p.Stretches)
                    .HasForeignKey(d => d.FrontGroupLeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stretch_FrontGroupLeader");

                entity.HasOne(d => d.OperationalFront)
                    .WithMany(p => p.Stretches)
                    .HasForeignKey(d => d.OperationalFrontId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stretch_OperationalFront");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Stretches)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stretch_Survey");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey");

                entity.Property(e => e.BaseMapId).HasDefaultValueSql("((0))");

                entity.Property(e => e.DateEnd).HasColumnType("date");

                entity.Property(e => e.DateIni).HasColumnType("date");

                entity.Property(e => e.Dimension)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DistanceBetweenHoles).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.GpsdatabaseName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("GPSDatabaseName");

                entity.Property(e => e.HolesArrangementDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HolesArrangementImagePath).IsUnicode(false);

                entity.Property(e => e.HolesBufferSizeFactor).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.HolesDepth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.LastEditorUserLogin).IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.PolygonColor)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Survey_Project");
            });

            modelBuilder.Entity<SurveyCoordinateSystem>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.SpatialReferenceId });

                entity.Property(e => e.SpatialReferenceId).HasColumnName("spatial_reference_id");

                entity.HasOne(d => d.SpatialReference)
                    .WithMany(p => p.SurveyCoordinateSystems)
                    .HasForeignKey(d => d.SpatialReferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyCoordinateSystems_prospatial_reference_systems");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyCoordinateSystems)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyCoordinateSystems_Survey");
            });

            modelBuilder.Entity<SurveyOperationalFront>(entity =>
            {
                entity.HasKey(e => new { e.OperationalFrontId, e.SurveyId });

                entity.ToTable("SurveyOperationalFront");

                entity.HasOne(d => d.OperationalFront)
                    .WithMany(p => p.SurveyOperationalFronts)
                    .HasForeignKey(d => d.OperationalFrontId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyOperationalFront_OperationalFront");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyOperationalFronts)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyOperationalFront_Survey");
            });

            modelBuilder.Entity<SurveyParameter>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.ParameterId });

                entity.ToTable("SurveyParameter");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parameter)
                    .WithMany(p => p.SurveyParameters)
                    .HasForeignKey(d => d.ParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyParameter_Parameter");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyParameters)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyParameter_Survey");
            });

            modelBuilder.Entity<SurveyPointsOfInterest>(entity =>
            {
                entity.HasKey(e => e.PointOfInterestId);

                entity.ToTable("SurveyPointsOfInterest");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Responsable)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Text)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyPointsOfInterests)
                    .HasForeignKey(d => d.SurveyId)
                    .HasConstraintName("FK_SurveyPointsOfInterest_Survey");
            });

            modelBuilder.Entity<SurveyProjectBase>(entity =>
            {
                entity.HasKey(e => new { e.SurveyId, e.ProjectBaseId });

                entity.ToTable("SurveyProjectBase");

                entity.HasOne(d => d.ProjectBase)
                    .WithMany(p => p.SurveyProjectBases)
                    .HasForeignKey(d => d.ProjectBaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyProjectBase_ProjectBase");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyProjectBases)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyProjectBase_Survey");
            });

            modelBuilder.Entity<Swath>(entity =>
            {
                entity.HasKey(e => new { e.SwathNumber, e.SurveyId, e.PreplotVersionId });

                entity.ToTable("Swath");

                entity.Property(e => e.AreaReceiverStationKm).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.AreaShotPointKm).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FinalReceiverLine)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FinalReceiverStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FinalShotLine)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FinalShotPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InitialReceiverLine)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.InitialReceiverStation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InitialShotLine)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.InitialShotPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Swaths)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Swath_Survey");
            });

            modelBuilder.Entity<SyncSummary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SyncSummary");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.SourceTable)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SystemConfiguration>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("SystemConfiguration");
            });

            modelBuilder.Entity<TerrainChargeControl>(entity =>
            {
                entity.ToTable("TerrainChargeControl");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Displacement)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FieldResponsable)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Hole1)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Hole2)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Hole3)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Hole4)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Hole5)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Hole6)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalShotLine)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalShotPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProvidenceQsms)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ProvidenceQSMS");

                entity.Property(e => e.Reazon)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecoveryDate).HasColumnType("date");

                entity.Property(e => e.RecoveryResponsable)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SafetyTechnician)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.TerrainChargeControls)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TerrainChargeControl_Survey");
            });

            modelBuilder.Entity<WeatherForecast>(entity =>
            {
                entity.HasKey(e => e.IdWeatherForecast)
                    .HasName("PK_dbo.WeatherForecast");

                entity.ToTable("WeatherForecast");

                entity.Property(e => e.ForecastDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
