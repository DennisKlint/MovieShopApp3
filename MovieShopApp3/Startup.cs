using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MovieShopApp3.Models;
using Owin;
using System.Collections.Generic;
using System.Web;

[assembly: OwinStartupAttribute(typeof(MovieShopApp3.Startup))]
namespace MovieShopApp3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
           
    }
        private void createRolesandUsers()
        {
            ////create sessionobject for shopingcart
            //List<int> cartList = new List<int>();
            //HttpContext.Current.Session["CartList"] = "test";
            ////Session["products"] = null;

            //// When retrieving an object from session state, cast it to 
            //// the appropriate type.
            //ArrayList stockPicks = (ArrayList)Session["StockPicks"];

            //// Write the modified stock picks list back to session state.
            //Session["StockPicks"] = stockPicks;




            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {

                // create admin roole
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //create default admin user 
                var user = new ApplicationUser();
                user.UserName = "AdminUser";
                user.Email = "adminguser@yahoo.com";

                string userPWD = "@Lexicon11";

                var chkUser = UserManager.Create(user, userPWD);
                //find userID
                var userid = UserManager.FindByEmail(user.Email).Id;

              

              dbMSA3Entities cont = new dbMSA3Entities();
                Users obj = new Users();
                obj.UserID = userid;
                obj.UserName = user.UserName;
                obj.Email = user.Email;
                obj.Adress = "TESTadress";
                obj.City = "Krallköping";
                obj.ZipCode = "1212";
                cont.Users.Add(obj);
                cont.SaveChanges();

                //Add Stefan to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }




        }

    }
}
