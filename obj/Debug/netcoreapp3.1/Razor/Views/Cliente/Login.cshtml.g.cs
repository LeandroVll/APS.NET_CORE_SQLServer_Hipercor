#pragma checksum "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6a0b9d72607299d5a81a07618312e1238c39ac64"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cliente_Login), @"mvc.1.0.view", @"/Views/Cliente/Login.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\_ViewImports.cshtml"
using HipercoreASPNETCORE;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\_ViewImports.cshtml"
using HipercoreASPNETCORE.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6a0b9d72607299d5a81a07618312e1238c39ac64", @"/Views/Cliente/Login.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa551bbd8b3e11f5da1f3ce4d862d1efb1520e94", @"/Views/_ViewImports.cshtml")]
    public class Views_Cliente_Login : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Credenciales>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
  
    ViewData["Title"] = "Login";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 8 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
  
    ViewBag.Title = "Login";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<style>
    .login-container {
        margin-top: 5%;
        margin-bottom: 5%;
    }

    .login-form-1 {
        padding: 5%;
        box-shadow: 0 5px 8px 0 rgba(0, 0, 0, 0.2), 0 9px 26px 0 rgba(0, 0, 0, 0.19);
    }

        .login-form-1 h3 {
            text-align: center;
            color: #333;
        }

    .login-form-2 {
        padding: 5%;
        background: #0062cc;
        box-shadow: 0 5px 8px 0 rgba(0, 0, 0, 0.2), 0 9px 26px 0 rgba(0, 0, 0, 0.19);
    }

        .login-form-2 h3 {
            text-align: center;
            color: #fff;
        }

    .login-container form {
        padding: 10%;
    }

    .btnSubmit {
        width: 50%;
        border-radius: 1rem;
        padding: 1.5%;
        border: none;
        cursor: pointer;
    }

    .login-form-1 .btnSubmit {
        font-weight: 600;
        color: #fff;
        background-color: #0062cc;
    }

    .login-form-2 .btnSubmit {
        font-weight: 600;
        color: #0062");
            WriteLiteral(@"cc;
        background-color: #fff;
    }

    .login-form-2 .ForgetPwd {
        color: #fff;
        font-weight: 600;
        text-decoration: none;
    }

    .login-form-1 .ForgetPwd {
        color: #0062cc;
        font-weight: 600;
        text-decoration: none;
    }

    .glyphicons {
        top: 1px;
        display: inline-block;
        font-family: glyphicons Halflings;
        font-style: normal;
        font-weight: normal;
        line-height: 1px;
    }
</style>

");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6a0b9d72607299d5a81a07618312e1238c39ac645121", async() => {
                WriteLiteral("\r\n\r\n    <div class=\"container login-container\" style=\"position:inherit\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-6 login-form-1\" style=\"position:inherit; height:446.2px;\">\r\n                <h3>IDENTIFICACIÓN</h3>\r\n");
#nullable restore
#line 92 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                 using (Html.BeginForm("Login", "Cliente", FormMethod.Post))
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 94 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
#nullable restore
#line 95 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
               Write(Html.ValidationSummary(true));

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <div class=\"form-group\">\r\n                        ");
#nullable restore
#line 98 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.Label("Email"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        ");
#nullable restore
#line 99 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.TextBoxFor(model => model.Email, "", new { @class = "form-control", @placeholder = "Introduce Email" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        ");
#nullable restore
#line 100 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.ValidationMessageFor(model => model.Email));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        <p style=\"color:red;\">");
#nullable restore
#line 101 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                                         Write(ViewBag.Mensaje);

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\r\n                    </div>\r\n                    <div class=\"form-group\">\r\n                        ");
#nullable restore
#line 104 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.Label("Password"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        ");
#nullable restore
#line 105 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.PasswordFor(model => model.Password, new { @class = "form-control", @placeholder = "Introduce Password" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        ");
#nullable restore
#line 106 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.ValidationMessageFor(model => model.Password));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        ");
#nullable restore
#line 107 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                   Write(Html.Hidden("RePassword",1234567));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                    </div>
                    <div class=""form-group"">
                        <button type=""submit"" class=""btnSubmit"">
                            <span style=""color:white; "" class=""glyphicons glyphicon-ok"" aria-hidden=""true"">
                                ACCEDER
                            </span>
                        </button>
                    </div>
");
                WriteLiteral("                    <div class=\"form-group\">\r\n                        <a");
                BeginWriteAttribute("href", " href=\"", 3310, "\"", 3359, 1);
#nullable restore
#line 119 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
WriteAttributeValue("", 3317, Url.Action("SolicitarContraseña", "Home"), 3317, 42, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" class=\"ForgetPwd\">Olvide la Contraseña</a>\r\n                    </div>\r\n");
#nullable restore
#line 121 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"


            </div>
            <div class=""col-md-6 login-form-2"" style=""position:inherit; height:446.2px;"">
                <h3 style=""margin-bottom:2em;"">NUEVO CLIENTE</h3>

                <div class=""ForgetPwd"" style=""margin-bottom:2em;"">
                    Si desea crear una ficha de cliente pulse en continuar
                </div>

                <br />
                <br />

                <button type=""button"" class=""btnSubmit"" style=""margin: 0 auto;""");
                BeginWriteAttribute("onclick", " onclick=\"", 3937, "\"", 3993, 3);
                WriteAttributeValue("", 3947, "location.href=\'", 3947, 15, true);
#nullable restore
#line 136 "C:\Users\DEMOGON\source\repos\HipercoreASPNETCORE\Views\Cliente\Login.cshtml"
WriteAttributeValue("", 3962, Url.Action("Registro","Home"), 3962, 30, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 3992, "\'", 3992, 1, true);
                EndWriteAttribute();
                WriteLiteral(">\r\n                    <span style=\"color:blue;\" class=\"glyphicon glyphicon-pencil\">\r\n                        CONTINUAR\r\n                    </span>\r\n                </button>\r\n\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Credenciales> Html { get; private set; }
    }
}
#pragma warning restore 1591
