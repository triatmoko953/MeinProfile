using MeinProfil.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeinProfil.Controllers
{
    public class ProfileController : Controller
    {
        //DI
        private readonly ProfileContext _context;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ProfileController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IWebHostEnvironment hosting, ProfileContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hosting = hosting;

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, List<IFormFile> file)
        {
            var path = _hosting.WebRootPath + "/upload/";
            if (file.Count > 0)
            {
                var filetarget = path + file[0].FileName;
                using (var stream = new FileStream(filetarget, FileMode.Create))
                {
                    await file[0].CopyToAsync(stream);
                }
                //validasi
                if (ModelState.IsValid)
                {
                    //validasi username
                    var existingUser = await _userManager.FindByNameAsync(user.Username);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError(string.Empty, "Username sudah digunakan.");
                        return View(user);
                    }
                    //add user
                    AppUser NewUser = new AppUser
                    {
                        UserName = user.Username,
                        FullName = user.FullName,
                        Email = user.Email,
                        Address = user.Address,
                        Job = user.Job,
                        PhoneNumber = user.PhoneNumber,
                        IdentityNumber = user.IdentityNumber,
                        ProfilePicture = file[0].FileName
                    };
                    var result = _userManager.CreateAsync(NewUser, user.Password)
                    .GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        return Redirect("/home");
                    }
                };
            }
            return View(user);
        }
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginView usr, string? returnUrl)
        {
            var usrapp = _userManager.FindByNameAsync(usr.Username)
                .GetAwaiter().GetResult();
            if (usrapp != null)
            {
                //session clear
                _signInManager.SignOutAsync().GetAwaiter().GetResult();
                var hasil = _signInManager.PasswordSignInAsync(
                    usrapp, usr.Password, false, false).GetAwaiter().GetResult();
                if (hasil.Succeeded)
                    return Redirect(returnUrl ?? "/home");
            }
            return View(usr);
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Logout(string returnUrl)
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return Redirect("/home");
        }
        public IActionResult Edit()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Profile");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditView edit)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Ambil data user yang sedang login
                var currentUser = await _userManager.GetUserAsync(User);

                // Validasi data model
                if (!ModelState.IsValid)
                {
                    return View(edit);
                }

                // Update data user
                currentUser.FullName = edit.FullName;
                currentUser.Email = edit.Email;
                currentUser.Address = edit.Address;
                currentUser.Job = edit.Job;
                currentUser.PhoneNumber = edit.PhoneNumber;
                currentUser.IdentityNumber = edit.IdentityNumber;

                var result = await _userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Gagal menyimpan perubahan profil");
                    return View(edit);
                }
            }
            else
            {
                return RedirectToAction("Login", "Profile");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
