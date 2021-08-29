using Microsoft.AspNetCore.Builder;

namespace SecurityProject.Api.Modules
{
    /// <summary>
    /// https://www.hanselman.com/blog/easily-adding-security-headers-to-your-aspnet-core-web-app-and-getting-an-a-grade
    /// https://www.c-sharpcorner.com/article/secure-web-application-using-http-security-headers-in-asp-net-core/
    /// https://letienthanh0212.medium.com/how-to-secure-your-net-core-web-application-with-nwebsec-c705fb5daf4b
    /// </summary>
    public static class SecurityModuleNWebsec
    {
        public static void SecurityConfigureNWebsec(this IApplicationBuilder app)
        {
            // X-XSS-Protection Header
            // prevent cross-site scripting attack
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            // Content-Security-Policy Header
            // prevent code injection attacks like cross-site scripting and clickjacking or prevent mixed mode (HTTPS and HTTP)
            // config by swagger
            app.UseCsp(options => options
                .DefaultSources(s => s.Self()
                    .CustomSources("data:")
                    .CustomSources("https:"))
                .StyleSources(s => s.Self()
                    .UnsafeInline()
                )
                .ScriptSources(s => s.Self()
                    .UnsafeInline()
                    .UnsafeEval()
                )
                .FontSources(s => s.Self()
                    .CustomSources("data:")
                    .CustomSources("https:")
                )
                .ImageSources(s => s.Self()
                    .CustomSources("data:")
                )
            );

            // X-Frame-Options Header
            // ensure that website content is not embedded into other sites and to prevent click jacking attacks
            app.UseXfo(options => options.Deny());

            // X-Content-Type-Options Header
            // disable the MIME-sniffing
            app.UseXContentTypeOptions();

            // Referrer Policy Header
            // If you don’t want to allow browsers to display your website as last visited in “Referer” header, we can use Referrer-Policy: no-referrer
            app.UseReferrerPolicy(opts => opts.NoReferrer());

            app.Use(async (context, next) =>
            {
                //Feature Policy Header
                // disable certain web platform features like microphone, camera etc. on browser and those they embedded
                if (!context.Response.Headers.ContainsKey("Permissions-Policy"))
                {
                    //https://scotthelme.co.uk/goodbye-feature-policy-and-hello-permissions-policy/
                    context.Response.Headers.Add("Permissions-Policy", "geolocation=();midi=();notifications=();push=();sync-xhr=();accelerometer=();microphone=();camera=();magnetometer=();gyroscope=();speaker 'self';vibrate=();fullscreen 'self';payment=();");
                }

                // Strict-Transport-Security Header
                // enforce that all communication is done over HTTPS                                        
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

                await next.Invoke();
            });
        }
    }
}
