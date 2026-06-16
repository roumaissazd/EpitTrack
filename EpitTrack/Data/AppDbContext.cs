using EpitTrack.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using static EpitTrack.Controllers.PlanningController;

namespace EpitTrack.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("EpiDataConnect"));
        }

        public DbSet<contratloa> ContratsLoa { get; set; }

        public DbSet<consocarbur> ConsosCarbur { get; set; }

        public DbSet<rh> Rhs { get; set; }

        public DbSet<coutrh> CoutRhs { get; set; }
        public DbSet<frais> LesFrais { get; set; }

        public DbSet<typefrais> TypesFrais { get; set; }

        public DbSet<dashboard> dashboards { get; set; }

        public DbSet<Rentabilite> Rentabilites { get; set; }
        public DbSet<uber> ubers { get; set; }

        
        public DbSet<planning> Plannings { get; set; }

        public DbSet<Classification> Classifications { get; set; }
        public DbSet<Course> Courses { get; set; }
		public DbSet<BanqueCa> CaBanques { get; set; }
		public DbSet<CourseDeReference> CoursesDeReferences { get; set; }
        public DbSet<Chauffeur> Chauffeurs { get; set; }

        public DbSet<preplanif> preplanifs { get; set; }

        public DbSet<planif> planifs { get; set; }

        public DbSet<Indisponibilite> indisponibilites { get; set; }

        public DbSet<planification> planifications { get; set; }

    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ici, vous configurez la relation entre Chauffeur et Preplanifs
            modelBuilder.Entity<Chauffeur>()
                .HasMany(c => c.preplanifs)
                .WithOne(i => i.Chauffeur)
                .HasForeignKey(i => i.id_chauff);

            // relation entre course et prelanif
            modelBuilder.Entity<preplanif>()
              .HasOne(p => p.Lacourse)
              .WithOne()
              .HasForeignKey<preplanif>(p => p.id_course);


            // Ici, vous configurez la relation entre Chauffeur et Indisponibilites

            modelBuilder.Entity<Chauffeur>()
                .HasMany(c => c.Indisponibilites)
                .WithOne(i => i.Chauffeur)
                .HasForeignKey(i => i.id_chauff);

            // Configuration des relations
            modelBuilder.Entity<Classification>()
                .HasOne(c => c.ClassOp)
                .WithMany()
                .HasForeignKey(c => c.id_class_op);

            modelBuilder.Entity<Classification>()
                .HasOne(c => c.SousClassOp)
                .WithMany()
                .HasForeignKey(c => c.id_sous_class_op);

            modelBuilder.Entity<IntReturn>().HasNoKey();

            modelBuilder.Entity<Rentabilite>()
           .HasKey(e => new { e.annee_courses, e.mois_courses,e.no_dossier,e.no_mission });

        }

    
        public DbSet<EpitTrack.Models.ClassOperation> ClassOperations { get; set; }

    
        public DbSet<EpitTrack.Models.SousClassOp> SousClassOps { get; set; }

    }
}