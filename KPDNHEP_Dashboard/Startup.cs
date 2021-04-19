using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using System.IO;
using IdentityModel.Client;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Serilog.Formatting.Json;
using Microsoft.AspNetCore.Authorization;
using KPDNHEP.Console.Services.Models;

namespace KPDNHEP.Console.UI
{
    public class Startup
    {
        public static List<Claim> claims = new List<Claim>();
        public static string id_token = "";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(Options => Options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();
            services.AddSession();

            // ... previous configuration not shown

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                new CultureInfo("en"),
                new CultureInfo("fr"),
                new CultureInfo("el"),
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });



            //localization end
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("oidc", options =>
            {
                // Set the authority to your oidc domain
                options.Authority = $"https://" + Configuration["OIDC:Domain"];


                // Configure the oidc Client ID and Client Secret
                options.ClientId = Configuration["OIDC:ClientId"];
                options.ClientSecret = Configuration["OIDC:ClientSecret"];

                // Set response type to code
                //options.ResponseType = "code id_token";
                options.ResponseType = "code";

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.GetClaimsFromUserInfoEndpoint = true;

                // Set the callback path, so oidc will call back to http://localhost:5000/signin-oidc 
                // Also ensure that you have added the URL as an Allowed Callback URL in your oidc dashboard 
                options.CallbackPath = new PathString(Configuration["OIDC:CallbackPath"]);

                // Configure the Claims Issuer to be oidc
                options.ClaimsIssuer = "oidc";

                // Saves tokens to the AuthenticationProperties
                options.SaveTokens = true;

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = notification =>
                    {
                        var ui_locales = notification.HttpContext.Request.Headers["ui_locales"];

                        var request = notification.Request;
                        var redirectUri = Configuration["OIDC:Protocol"] + "://" + request.Host + request.PathBase + Configuration["OIDC:RedirectUri"];

                        // Add custom redirect_uri to avoid the app from guessing it based in the running web server and have issues with a reverse-proxy.
                        notification.ProtocolMessage.RedirectUri = redirectUri;


                        //Uncomment - when Passing ACR values
                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                        {
                            //notification.ProtocolMessage.AcrValues = "icrypto";

                            notification.ProtocolMessage.Parameters.Add("ui_locales", ui_locales.ToString());
                        }
                        return Task.FromResult(0);
                    },

                    //Get User info using access_token and bind to UI
                    OnTokenResponseReceived = async auth =>
                    {
                        string role = "";
                        var userInfoClient = new UserInfoClient($"https://" + Configuration["OIDC:Domain"]+ "/" + Configuration["OIDC:CustomPath"] + "/restv1/userinfo");
                        var userInfoResponse = await userInfoClient.GetAsync(auth.TokenEndpointResponse.AccessToken);
                        foreach (var item in userInfoResponse.Claims)
                        {
                            if (item.Type == "roles")
                            {
                                role = item.Value;
                            }

                        }
                        var exp = auth.TokenEndpointResponse.Parameters;
                        var ProtocolMessage = auth.ProtocolMessage.Parameters;
                        //OIDCTokenResponse token = new OIDCTokenResponse
                        //{
                        //    AccessToken = exp["access_token"],
                        //    RefreshToken = exp["refresh_token"],
                        //    IdToken = exp["id_token"],
                        //    Type = exp["token_type"],
                        //    ExpiresIn = exp["expires_in"]
                        //};
                        OIDCCallbackResponse callback = new OIDCCallbackResponse
                        {
                            Code = ProtocolMessage["code"],
                            Scope = ProtocolMessage["scope"],
                            //SessionId = ProtocolMessage["session_id"],
                            //SessionState = ProtocolMessage["session_state"],
                        };
                        //string tokenInfo = JsonConvert.SerializeObject(token);
                        string callbackInfo = JsonConvert.SerializeObject(callback);
                        claims.Clear();
                        claims.AddRange(userInfoResponse.Claims);
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        //claims.Add(new Claim("tokenInfo", tokenInfo));
                        claims.Add(new Claim("callbackInfo", callbackInfo));
                        claims.Add(new Claim("session_id", auth.ProtocolMessage.Parameters["session_id"]));

                        if (!string.IsNullOrEmpty(auth.TokenEndpointResponse.RefreshToken))
                        {
                            //claims.Add(new Claim("refresh_token", auth.TokenEndpointResponse.RefreshToken));
                        }

                        id_token = auth.TokenEndpointResponse.IdToken;
                        claims.Add(new Claim("introspect_token", Introspect(auth.TokenEndpointResponse.AccessToken)));

                        return;
                    },


                    //Add Other claims like access_token, id_token etc..
                    OnUserInformationReceived = auth =>
                    {
                        ClaimsIdentity _claims = new ClaimsIdentity();
                        _claims.AddClaims(claims);
                        auth.Principal.AddIdentity(_claims);
                        return Task.FromResult(0);
                    },


                    // handle the logout redirection 
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {

                        context.ProtocolMessage.IdTokenHint = id_token;

                        var logoutUri = $"https://" + Configuration["OIDC:Domain"] + "/" + Configuration["OIDC:CustomPath"] + "/restv1/end_session?id_token_hint=" + context.ProtocolMessage.IdTokenHint;

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = Configuration["OIDC:Protocol"] + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&post_logout_redirect_uri={ Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                };
            });


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }


        private string Introspect(string access_token)
        {

            var postData = "token=" + access_token;

            var request = (HttpWebRequest)WebRequest.Create($"https://" + Configuration["OIDC:Domain"] + $"/" + Configuration["OIDC:CustomPath"] + "/restv1/introspection");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            // allows for validation of SSL conversations
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            request.Method = "POST";
            request.ContentLength = 0;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorization", "Bearer " + access_token);

            if (!string.IsNullOrEmpty(postData))
            {
                var encoding = new UTF8Encoding();
                var bytes = Encoding.GetEncoding("utf-8").GetBytes(postData);
                request.ContentLength = bytes.Length;
                try
                {
                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                // grab the response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                }

                return responseValue;
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //localization
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.File(new JsonFormatter(), "wwwroot/log/access-log.json", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            //Create Logger
            app.Use(async (context, next) =>
            {
                // Request method, scheme, and path
                Log.Debug("Request Method: {METHOD}", context.Request.Method);
                Log.Debug("Request Scheme: {SCHEME}", context.Request.Scheme);
                Log.Debug("Request Path: {PATH}", context.Request.Path);

                // Headers
                foreach (var header in context.Request.Headers)
                {
                    Log.Debug("Header: {KEY}: {VALUE}", header.Key, header.Value);
                }

                // Connection: RemoteIp
                Log.Debug("Request RemoteIp: {REMOTE_IP_ADDRESS}",
                    context.Connection.RemoteIpAddress);

                await next();

            });
            app.UseMiddlewareAuthorization();
            app.UseRouting();
            app.UseAuthentication(); // Must be after UseRouting()
            app.UseAuthorization(); // Must be after UseAuthentication()
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }

    }
}

