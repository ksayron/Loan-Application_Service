
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TriInkom.DataBase;
using TriInkom.Mapping;
using TriInkom.Middleware;

namespace TriInkom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LoanDbContext>(opt =>
            opt.UseNpgsql(connectionString));

            builder.Services.AddAutoMapper(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();
            app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
            app.MapGet("/", () => Results.Redirect("/swagger"));

            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
                ctx.Database.Migrate();
            }

            app.Run();
        }
    }
}
