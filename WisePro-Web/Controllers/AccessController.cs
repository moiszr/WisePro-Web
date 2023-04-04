using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Firebase.Auth;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using WisePro_Web.Models;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Google.Cloud.Firestore;

namespace WisePro_Web.Controllers
{
    public class AccessController : Controller
    {
        FirebaseAuthProvider auth;

        public AccessController()
        {
            auth = new FirebaseAuthProvider(
                            new FirebaseConfig("AIzaSyAlAAtHOAGr4QMK2MGZEH3IKCIVBcp870g"));
        }

        public IActionResult Login()
        {
            var token = HttpContext.Session.GetString("_UserToken");

            if (token != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginManager modelLogin)
        {
            try
            {
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(modelLogin.Email, modelLogin.Password);
                string token = fbAuthLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                        new Claim("OtherProperties", "Admin")
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = modelLogin.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity), properties);

                    return RedirectToAction("Index", "Home");
                }

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(modelLogin);
            }

            ViewData["ValidateMessage"] = "User not found";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterManager registerManager)
        {
            try
            {
                FirebaseAuthLink newUserAuthLink = await auth.CreateUserWithEmailAndPasswordAsync(registerManager.Email, registerManager.Password);

                await SetUserData(newUserAuthLink.User, registerManager.Name + " " + registerManager.LastName);

                return RedirectToAction("Login", "Access");
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(registerManager);
            }
        }


        private async Task SetUserData(Firebase.Auth.User user, string displayName = null, string photoURL = null)
        {
            FirestoreDb db = FirestoreDb.Create("wisepro-9d52e");

            CollectionReference usersRef = db.Collection("users");
            DocumentReference userDocRef = usersRef.Document(user.LocalId);

            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { "uid", user.LocalId },
                { "email", user.Email },
                { "displayName", displayName ?? user.DisplayName },
                { "photoURL", photoURL ?? user.PhotoUrl },
                { "emailVerified", user.IsEmailVerified }
            };

            try
            {
                await userDocRef.SetAsync(userData);
                Console.WriteLine("User data stored in Firestore");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error storing user data: " + ex.Message);
            }
        }

    }
}
