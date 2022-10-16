var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddDefaultPayslipGeneratorServices();
//services.AddRazorPages();
services
    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
//app.UseRouting();
//app.UseAuthorization();
//app.MapRazorPages();

// For simplicity, Razor view is not used.
app.Use((async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/app"))
    {
        context.Response.ContentType = "text/html";

        var html = """
<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Payslip Generator</title>
    <base href="/">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" type="image/x-icon" href="favicon.ico">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <style>
    .invalid-feedback, .validation-summary-errors {
        display: block;
    }
    </style>
</head>
<body>
    <div class="col-lg-8 py-5 px-3 mx-auto">
        <div class="row mb-5">
            <div class="col-sm-3">
                <img class="img-fluid img-datacom" src="data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPCEtLSBHZW5lcmF0b3I6IEFkb2JlIElsbHVzdHJhdG9yIDI0LjEuMCwgU1ZHIEV4cG9ydCBQbHVnLUluIC4gU1ZHIFZlcnNpb246IDYuMDAgQnVpbGQgMCkgIC0tPgo8c3ZnIHZlcnNpb249IjEuMSIgaWQ9IkxheWVyXzEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiIHg9IjBweCIgeT0iMHB4IgoJIHZpZXdCb3g9IjAgMCAxNjAgMzAiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDE2MCAzMDsiIHhtbDpzcGFjZT0icHJlc2VydmUiPgo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPgoJLnN0MHtmaWxsOiMwMDI0NzA7fQo8L3N0eWxlPgo8Zz4KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0wLjEsMC41bDAuNCwxLjN2MjYuNEwwLDI5LjFoOS45YzYuOSwwLDEyLjctNS42LDEyLjctMTUuMWMwLTYuOS0zLjQtMTMuMS0xMC42LTEzLjZMMC4xLDAuNUwwLjEsMC41egoJCSBNNS42LDMuM2gzLjJDMTMuNiwzLjMsMTcsNi4xLDE3LDE0YzAsOS43LTQsMTEuNy04LjYsMTEuN0g1LjZWMy4zeiBNNTQuNSwzLjFoNi4xbDAuOSwwLjdsMS0zLjJINDIuMkw0MS41LDRsMS42LTAuN2g1Ljh2MjUuMQoJCWwtMC40LDFoNi42bC0wLjQtMVYzLjFINTQuNXogTTEwMC41LDI0LjVjLTEuNSwxLjgtMy40LDIuNy01LjUsMi43Yy01LjIsMC04LTQuNi04LTExLjhjMC02LjIsMi4yLTEyLjcsNy43LTEyLjcKCQljMi4yLDAsNC4zLDEuMyw1LjUsMy43aDAuNFYyLjFjLTEuOS0xLjMtMy44LTEuOC02LjEtMS44Yy03LjIsMC0xMy4zLDQuOS0xMy4zLDE1LjhjMCw4LjYsNS41LDEzLjYsMTIuNSwxMy42YzIuMiwwLDQuNiwwLDYuNi0xLjMKCQl2LTQuMVYyNC41eiBNMTI3LjQsMTQuOGMwLTYuNS0yLjgtMTQuNS0xMi40LTE0LjVjLTguMywwLTEzLjEsNi4zLTEzLjEsMTQuNmMwLDkuNCw1LDE0LjgsMTIuNCwxNC44CgkJQzEyMi40LDI5LjcsMTI3LjQsMjMsMTI3LjQsMTQuOCBNMTE0LjgsMi44YzUsMCw2LjksNS44LDYuOSwxMS4yYzAsNi45LTIuMiwxMy4zLTcuNCwxMy4zYy01LDAtNi45LTUuNi02LjktMTAuOQoJCUMxMDcuNSw2LjcsMTEwLjMsMi44LDExNC44LDIuOCBNMTQ0LjIsMjFsLTguNC0yMC41aC00LjZsMC40LDAuOWwtMS45LDI2LjRsLTAuNCwxLjNoMy41bDEuMy0xNy45bDgsMTcuOWgxLjlsNy44LTE3LjdsMS41LDE3LjcKCQloNi42bC0wLjQtMS4yTDE1NywxLjJsMC4xLTAuN2gtNC42TDE0NC4yLDIxeiBNMzcuMiwyMC43bDIuOCw4LjZoNi41bC0wLjctMC43bC05LjktMjhoLTUuNmwwLjYsMS42bC05LjIsMjYuMWwtMC40LDAuN2g0bDIuOC04LjYKCQloOS4yVjIwLjd6IE0yOS4yLDE3LjNsMy40LTEwbDMuNSwxMEgyOS4yeiBNNzMuMSwyMC43bDIuOCw4LjZoNi41bC0wLjctMC45TDcxLjcsMC41aC01LjZsMC42LDEuNmwtOS4yLDI2LjFsLTAuNCwwLjdoNGwyLjgtOC42CgkJaDkuMkM3My4xLDIwLjQsNzMuMSwyMC43LDczLjEsMjAuN3ogTTY0LjksMTcuM2wzLjQtMTBsMy41LDEwSDY0Ljl6Ii8+CjwvZz4KPC9zdmc+Cg=="/>
            </div>
            <h3>Payslip Generator Coding Test</h3>
        </div>
        <div id="app" class="col-12"></div>
    </div>
    <script src="https://code.jquery.com/jquery-3.6.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.5/jquery.validate.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/4.0.0/jquery.validate.unobtrusive.min.js"></script>
    <script src="/app/main.js"></script>
</body>
</html>
""";

        await context.Response.WriteAsync(html);
    }
    else if (!context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.Redirect("/app");
    }
    else
    {
        await next();
    }
}));

app.Run();