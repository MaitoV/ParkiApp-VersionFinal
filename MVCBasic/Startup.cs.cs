namespace MVCBasic
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(); // Configurar la sesión
            services.AddMvc(); // Otra configuración de servicios si es necesaria
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession(); // Habilitar la sesión en el middleware

            // Configuración de otros middlewares y routing si es necesario
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Otra configuración de endpoints si es necesaria
            });
        }
        }
}
