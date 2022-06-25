using AppCore.DataAccess.Configs;
using Business.Services;
using Business.Services.Bases;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
List<CultureInfo> _cultures = new List<CultureInfo>()
{
    new CultureInfo("tr-TR")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name);
    options.SupportedCultures = _cultures;
    options.SupportedUICultures = _cultures;
});

#region CORS (Cross Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
            .AllowAnyOrigin() // Her kaynaktan gelen isteklere yanýt ver, kaynaklara örnek https://sahibinden.com, https://hepsiburada.com, vb. Mesela origin için baþka methodlar ile sadece sahibinden.com üzerinden gelen isteklere yanýt verilmesi ayarlanabilir.
            .AllowAnyHeader() // Ýsteklerin (request) gövdesi (body) dýþýnda baþlýk (head) içerisinde gönderilen ekstra veriler, örneðin Authorize, Content-Type, vb. Mesela header'lar için baþka methodlar ile sadece Content-Type header'ýna izin verilebilir. 
            .AllowAnyMethod() // Method'lar: get, post, put, delete, vb. Ýsteklerdeki bütün method'lara izin ver. Mesela method'lar için builder üzerinden baþka methodlar ile sadece get header'ýna yanýt verilmesi saðlanabilir.
    );
});
#endregion


ConnectionConfig.ConnectionString = builder.Configuration.GetConnectionString("ETicaretContext");

builder.Services.AddScoped<IUrunService, UrunService>();
builder.Services.AddScoped<IKategoriService, KategoriService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name),
    SupportedCultures = _cultures,
    SupportedUICultures = _cultures,
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region CORS (Cross Origin Resource Sharing)
app.UseCors();
#endregion

app.UseAuthorization();

app.MapControllers();

app.Run();
