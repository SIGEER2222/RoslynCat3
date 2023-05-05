using RoslynCat.Helpers;
using RoslynCat.Interface;
using RoslynCat.Roslyn;
using RoslynCat.SQL;
using SqlSugar;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddSingleton<ChatGPT>();
builder.Services.AddScoped<IWorkSpaceService,WorkSpaceService>();
builder.Services.AddScoped<ICompleteProvider,CompleteProvider>();
builder.Services.AddScoped<IHoverProvider,HoverProvider>();
builder.Services.AddScoped<ICodeCheckProvider,CodeCheckProvider>();
builder.Services.AddScoped<IGistService,CodeSharing>();
builder.Services.AddScoped<CompletionProvider>();
builder.Services.AddScoped<CodeSampleRepository>();

builder.Services.AddHttpClient("GithubApi",client => {
    client.BaseAddress = new Uri("https://api.github.com");
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ISqlSugarClient>(provider => new SqlSugarFactory().Create(provider));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
