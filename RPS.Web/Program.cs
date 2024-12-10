using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPS.Data;

var builder = WebApplication.CreateBuilder(args);

var tempDataContext = new PtInMemoryContext();

builder.Services.AddSingleton<IPtUserRepository>(c => new PtUserRepository(tempDataContext));
builder.Services.AddSingleton<IPtItemsRepository>(c => new PtItemsRepository(tempDataContext));
builder.Services.AddSingleton<IPtDashboardRepository>(c => new PtDashboardRepository(tempDataContext));
builder.Services.AddSingleton<IPtTasksRepository>(c => new PtTasksRepository(tempDataContext));
builder.Services.AddSingleton<IPtCommentsRepository>(c => new PtCommentsRepository(tempDataContext));

builder.Services.AddRazorPages();
builder.Services.AddMvc()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/Dashboard", "");
    });

builder.Services.AddKendo();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
