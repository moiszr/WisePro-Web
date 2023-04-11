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
using Microsoft.VisualBasic;
using NuGet.Common;

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
                    var user = fbAuthLink.User;

                    HttpContext.Session.SetString("_UserToken", token);

                    var userData = await GetUserData(user.LocalId); // Recupera la información del usuario de Firestore y la almacena en userData

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                        new Claim("uid", user.LocalId)
                    };

                    if (userData.ContainsKey("displayName") && userData["displayName"] != null)
                    {
                        claims.Add(new Claim("displayName", userData["displayName"].ToString()));
                    }

                    if (userData.ContainsKey("email") && userData["email"] != null)
                    {
                        claims.Add(new Claim("email", userData["email"].ToString()));
                    }

                    if (userData.ContainsKey("emailVerified") && userData["emailVerified"] != null)
                    {
                        claims.Add(new Claim("emailVerified", userData["emailVerified"].ToString()));
                    }

                    if (userData.ContainsKey("photoURL") && userData["photoURL"] != null)
                    {
                        claims.Add(new Claim("photoURL", userData["photoURL"].ToString()));
                    }

                    claims.Add(new Claim("OtherProperties", "Admin"));

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

                var uid = newUserAuthLink.User.LocalId;
                var name = registerManager.Name;
                var lastName = registerManager.LastName;
                var email = registerManager.Email;

                // Aqui puedes implementar la logica para que guarde la informacion de firebase en la base de datos de nosotros.

                // utilizalas variables definidas mas arriba para enviarselas a nuestra propia base de datos.

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

        private async Task<Dictionary<string, object>> GetUserData(string uid)
        {
            FirestoreDb db = FirestoreDb.Create("wisepro-9d52e");

            CollectionReference usersRef = db.Collection("users");
            DocumentReference userDocRef = usersRef.Document(uid);

            try
            {
                DocumentSnapshot snapshot = await userDocRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    return snapshot.ToDictionary();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error retrieving user data: " + ex.Message);
            }

            return null;
        }

    }
}
