using Microsoft.EntityFrameworkCore;
using PflegeVerbundhilfe.TaskManagement;
using PflegeVerbundhilfe.TaskManagement.Repositories;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskManagementDbContext>(options => options.UseInMemoryDatabase("VerbundhilfeTaskDb"));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();
    await SeedTestDatabase(context);
}

static async Task SeedTestDatabase(TaskManagementDbContext context)
{
    if (!context.TodoTasks.Any())
    {
        context.TodoTasks.AddRange(
        new TodoTask { Description = "Setting up the Functional specification.", Deadline = new DateTime(2024,07,25) },
        new TodoTask { Description = "Creating High level design.", Deadline = new DateTime(2024, 07, 29) },
        new TodoTask { Description = "Creating low level design." , Deadline = new DateTime(2024, 08, 03) },
        new TodoTask { Description = "Creating Backend-project.",  Deadline = new DateTime(2024, 08, 10) },
        new TodoTask { Description = "Creating Frontend-project.",  Deadline = new DateTime(2024, 08, 12) },
        new TodoTask { Description = "Creating Database design.",  Deadline = new DateTime(2024, 08, 20) },
        new TodoTask { Description = "Writing Test Scenarios.",  Deadline = new DateTime(2024, 08, 26) },
        new TodoTask { Description = "Development of business logic.",  Deadline = new DateTime(2024, 08, 29) },
        new TodoTask { Description = "Development of REST Api's.",  Deadline = new DateTime(2024, 08, 03) },
        new TodoTask { Description = "Development of integration tests." , Deadline = new DateTime(2024, 09, 04) },
        new TodoTask { Description = "Perform regression testing." , Deadline = new DateTime(2024, 09, 08) },
        new TodoTask { Description = "Releasing the project to client." , Deadline = new DateTime(2024, 09, 12) },
        new TodoTask { Description = "Working on support tickets.", Deadline = new DateTime(2024, 09, 15) }
        );
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
     .AllowAnyMethod()
     .AllowAnyHeader()
     .SetIsOriginAllowed(origin => true) // allow any origin 
     .AllowCredentials());

app.Run();
