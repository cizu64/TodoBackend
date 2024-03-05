
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TODOBACKEND.Entities;
using TODOBACKEND.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
const string cors = "todocors";
builder.Services.AddCors(options=>{
  options.AddPolicy(cors, policy=>
  {
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
  });
});
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
        new BadRequestObjectResult(new
        {
            detail = context?.ModelState?.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage,
        });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]);
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:AUDIENCE"],
        ValidIssuer = builder.Configuration["JWT:ISSUER"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseNpgsql(connectionString, opt =>
    {
        opt.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), errorCodesToAdd: null);
    });
});
builder.Services.AddScoped<DbContext, TodoContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TodoRepository>();

var app = builder.Build();
app.UseCors(cors);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
//app.UseHttpsRedirection();
app.Run();