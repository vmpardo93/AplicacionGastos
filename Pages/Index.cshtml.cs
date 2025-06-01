using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MovimientoGastos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(SignInManager<IdentityUser> signInManager, ILogger<IndexModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; } = "/Dashboard";

        public class InputModel
        {
            [Required(ErrorMessage = "El nombre de usuario es requerido")]
            [Display(Name = "Nombre de Usuario")]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es requerida")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Recordarme")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGet(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/Dashboard");
            }

            ReturnUrl = returnUrl ?? "/Dashboard";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= "/Dashboard";

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuario conectado.");
                    return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Cuenta de usuario bloqueada.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
