using System.Reflection;
using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Domain.Entities.Content;

namespace LeksanaStudio.Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        /* ------------------------------------------------------------- access */

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<RoleMenu> RoleMenus { get; set; } = null!;

        /* ---------------------------------------------------- content plumbing */

        public DbSet<Locale> Locales { get; set; } = null!;
        public DbSet<Media> Media { get; set; } = null!;
        public DbSet<SlugHistory> SlugHistories { get; set; } = null!;

        /* ------------------------------------------------------------ content */

        public DbSet<CaseStudy> CaseStudies { get; set; } = null!;
        public DbSet<CaseStudyTranslation> CaseStudyTranslations { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
        public DbSet<NoteTranslation> NoteTranslations { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<ServiceTranslation> ServiceTranslations { get; set; } = null!;
        public DbSet<Vertical> Verticals { get; set; } = null!;
        public DbSet<VerticalTranslation> VerticalTranslations { get; set; } = null!;
        public DbSet<ServicePackage> ServicePackages { get; set; } = null!;
        public DbSet<ServicePackageTranslation> ServicePackageTranslations { get; set; } = null!;
        public DbSet<ProjectPhase> ProjectPhases { get; set; } = null!;
        public DbSet<ProjectPhaseTranslation> ProjectPhaseTranslations { get; set; } = null!;
        public DbSet<AddOn> AddOns { get; set; } = null!;
        public DbSet<AddOnTranslation> AddOnTranslations { get; set; } = null!;
        public DbSet<PaymentTerm> PaymentTerms { get; set; } = null!;
        public DbSet<PaymentTermTranslation> PaymentTermTranslations { get; set; } = null!;
        public DbSet<ProcessStep> ProcessSteps { get; set; } = null!;
        public DbSet<ProcessStepTranslation> ProcessStepTranslations { get; set; } = null!;
        public DbSet<PageCopy> PageCopies { get; set; } = null!;
        public DbSet<PageCopyTranslation> PageCopyTranslations { get; set; } = null!;
        public DbSet<PageCopySlotDefinition> PageCopySlotDefinitions { get; set; } = null!;
        public DbSet<PageDocument> PageDocuments { get; set; } = null!;
        public DbSet<PageDocumentTranslation> PageDocumentTranslations { get; set; } = null!;
        public DbSet<SiteProfile> SiteProfiles { get; set; } = null!;
        public DbSet<SiteProfileTranslation> SiteProfileTranslations { get; set; } = null!;

        // Base-entity property names whose physical column is named "{table}_{property}".
        // Applied by convention so entities inherit the audit and translation block
        // instead of re-declaring every [Column]. Only these (never FK/domain
        // columns, which don't follow the plain concat rule, e.g. UserId ->
        // user_role_user_id).
        private static readonly string[] _baseColumnProperties =
        [
            nameof(IBaseEntity.Id),
            nameof(IBaseEntity.IsDeleted),
            nameof(IBaseEntity.CreatedDate),
            nameof(IBaseEntity.UpdatedDate),
            nameof(IBaseEntity.DeletedDate),
            nameof(IBaseEntity.CreatedBy),
            nameof(IBaseEntity.UpdatedBy),
            nameof(IBaseEntity.DeletedBy),
            nameof(ITranslationEntity.ParentId),
            nameof(ITranslationEntity.LocaleCode),
            nameof(ITranslationEntity.Slug),
            nameof(ITranslationEntity.Status),
            nameof(ITranslationEntity.PublishedAt),
        ];

        private static readonly MethodInfo _softDeleteFilterMethod = typeof(AppDbContext).GetMethod(
            nameof(ApplySoftDeleteFilter),
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyBaseEntityConventions(modelBuilder);
            ApplyEnumsAsText(modelBuilder);

            ConfigureAccess(modelBuilder);
            ConfigureContentPlumbing(modelBuilder);
            ConfigureContent(modelBuilder);
        }

        /* -------------------------------------------------------------- access */

        private static void ConfigureAccess(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(c => new { c.IsDeleted, c.Email });

            modelBuilder
                .Entity<UserRefreshToken>()
                .HasIndex(c => new
                {
                    c.IsDeleted,
                    c.Token,
                    c.IsRevoked,
                });

            modelBuilder.Entity<Role>().HasIndex(c => new { c.IsDeleted, c.Code });

            modelBuilder
                .Entity<UserRole>()
                .HasIndex(c => new
                {
                    c.IsDeleted,
                    c.UserId,
                    c.RoleId,
                });

            modelBuilder.Entity<Menu>().HasIndex(c => new { c.IsDeleted, c.Code });

            modelBuilder
                .Entity<RoleMenu>()
                .HasIndex(c => new
                {
                    c.IsDeleted,
                    c.RoleId,
                    c.MenuId,
                });
        }

        /* --------------------------------------------------- content plumbing */

        private static void ConfigureContentPlumbing(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locale>().HasIndex(l => new { l.IsDeleted, l.Code }).IsUnique();
            modelBuilder.Entity<Locale>().HasIndex(l => new { l.IsDeleted, l.IsActive, l.Order });

            modelBuilder.Entity<Media>().HasIndex(m => new { m.IsDeleted, m.CreatedDate });

            // Read only by exact match on (type, locale, old slug) — the lookup the
            // site performs before answering 404, to answer 301 instead.
            modelBuilder
                .Entity<SlugHistory>()
                .HasIndex(h => new
                {
                    h.EntityType,
                    h.LocaleCode,
                    h.OldSlug,
                });
        }

        /* ------------------------------------------------------------- content */

        private static void ConfigureContent(ModelBuilder modelBuilder)
        {
            ConfigureTranslatable<CaseStudy, CaseStudyTranslation>(modelBuilder);
            ConfigureTranslatable<Note, NoteTranslation>(modelBuilder);
            ConfigureTranslatable<Service, ServiceTranslation>(modelBuilder);
            ConfigureTranslatable<Vertical, VerticalTranslation>(modelBuilder);
            ConfigureTranslatable<ServicePackage, ServicePackageTranslation>(modelBuilder);
            ConfigureTranslatable<ProjectPhase, ProjectPhaseTranslation>(modelBuilder);
            ConfigureTranslatable<AddOn, AddOnTranslation>(modelBuilder);
            ConfigureTranslatable<PaymentTerm, PaymentTermTranslation>(modelBuilder);
            ConfigureTranslatable<ProcessStep, ProcessStepTranslation>(modelBuilder);
            ConfigureTranslatable<PageCopy, PageCopyTranslation>(modelBuilder);
            ConfigureTranslatable<PageDocument, PageDocumentTranslation>(modelBuilder);
            ConfigureTranslatable<SiteProfile, SiteProfileTranslation>(modelBuilder);

            modelBuilder.Entity<CaseStudy>().HasIndex(c => new { c.IsDeleted, c.Order });
            modelBuilder.Entity<CaseStudy>().HasIndex(c => c.ContentKey);
            modelBuilder
                .Entity<CaseStudy>()
                .HasOne(c => c.CoverMedia)
                .WithMany()
                .HasForeignKey(c => c.CoverMediaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Note>().HasIndex(n => new { n.IsDeleted, n.Order });
            modelBuilder.Entity<Note>().HasIndex(n => n.ContentKey);

            modelBuilder.Entity<Service>().HasIndex(s => new { s.IsDeleted, s.Order });
            modelBuilder.Entity<Service>().HasIndex(s => s.ContentKey);
            // Restrict, not cascade: a case study is evidence, and deleting it must
            // not silently take a service page's proof link with it.
            modelBuilder
                .Entity<Service>()
                .HasOne(s => s.CaseStudy)
                .WithMany()
                .HasForeignKey(s => s.CaseStudyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vertical>().HasIndex(v => new { v.IsDeleted, v.Order });
            modelBuilder.Entity<Vertical>().HasIndex(v => v.ContentKey);
            modelBuilder
                .Entity<Vertical>()
                .HasOne(v => v.Service)
                .WithMany()
                .HasForeignKey(v => v.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ServicePackage>()
                .HasIndex(p => new
                {
                    p.IsDeleted,
                    p.Group,
                    p.Order,
                });

            modelBuilder.Entity<ProjectPhase>().HasIndex(p => new { p.IsDeleted, p.Order });
            modelBuilder.Entity<AddOn>().HasIndex(a => new { a.IsDeleted, a.Order });
            modelBuilder.Entity<PaymentTerm>().HasIndex(t => new { t.IsDeleted, t.Order });
            modelBuilder.Entity<ProcessStep>().HasIndex(s => new { s.IsDeleted, s.Order });

            modelBuilder.Entity<PageCopy>().HasIndex(p => new { p.IsDeleted, p.PageCode }).IsUnique();
            modelBuilder.Entity<PageDocument>().HasIndex(p => new { p.IsDeleted, p.PageCode }).IsUnique();

            modelBuilder
                .Entity<PageCopySlotDefinition>()
                .HasIndex(s => new
                {
                    s.IsDeleted,
                    s.PageCode,
                    s.Order,
                });
            modelBuilder
                .Entity<PageCopySlotDefinition>()
                .HasIndex(s => new { s.PageCode, s.SlotKey })
                .IsUnique();
        }

        /// <summary>
        /// Wires one entry to its translations and applies the two rules that make
        /// the pattern safe: one translation per language, and one address per
        /// language. Both enforced by the database — a service can forget, an index
        /// cannot.
        /// </summary>
        private static void ConfigureTranslatable<TEntity, TTranslation>(ModelBuilder modelBuilder)
            where TEntity : class, ITranslatableEntity<TTranslation>
            where TTranslation : class, ITranslationEntity
        {
            modelBuilder
                .Entity<TEntity>()
                .HasMany(e => e.Translations)
                .WithOne()
                .HasForeignKey(t => t.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<TTranslation>()
                .HasIndex(t => new { t.ParentId, t.LocaleCode })
                .IsUnique()
                .HasFilter("\"" + TranslationTableName<TTranslation>() + "_isdeleted\" = false");

            modelBuilder
                .Entity<TTranslation>()
                .HasIndex(t => new
                {
                    t.LocaleCode,
                    t.Slug,
                    t.Status,
                });
        }

        private static string TranslationTableName<TTranslation>() =>
            typeof(TTranslation)
                .GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>()
                ?.Name ?? typeof(TTranslation).Name.ToLowerInvariant();

        /* --------------------------------------------------------- conventions */

        // For every IBaseEntity: name the base/audit columns "{table}_{property}"
        // and add a global soft-delete query filter (replaces the per-method
        // !IsDeleted checks).
        private static void ApplyBaseEntityConventions(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                    continue;

                var table = entityType.GetTableName();
                if (table is null)
                    continue;

                foreach (var propertyName in _baseColumnProperties)
                {
                    var property = entityType.FindProperty(propertyName);
                    property?.SetColumnName($"{table}_{propertyName.ToLowerInvariant()}");
                }

                _softDeleteFilterMethod
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(null, [modelBuilder]);
            }
        }

        /// <summary>
        /// Enums are stored as text, not as integers.
        ///
        /// A row that reads <c>published</c> can be understood in psql at three in
        /// the morning; a row that reads <c>1</c> cannot. The storage cost is
        /// irrelevant at this scale, and reordering an enum can no longer silently
        /// rewrite the meaning of existing data.
        /// </summary>
        private static void ApplyEnumsAsText(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                    if (type.IsEnum)
                        property.SetProviderClrType(typeof(string));
                }
            }
        }

        private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : class, IBaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
