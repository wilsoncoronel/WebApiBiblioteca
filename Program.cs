using HolaMundoWebAPI;
using HolaMundoWebAPI.Datos;
using HolaMundoWebAPI.Utilidad;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//Forma anterior de agregar una conexion 
//builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));
//Permite ignorar el problem de referencia ciclica por las relaciones entre las clases
//builder.Services.AddControllers().AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IMapeos, Mapeos>();
//Forma correcta de agregar la conexion a la bd
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

//app.MapGet("/", () => "Hola mundo esto es una prueba!!");

//area de los middlewares



app.MapControllers();
app.Run();
