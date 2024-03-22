using System.Security.Claims;
using Akalaat.DAL.Data;
using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

public class AuthorizeMenuAccessAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        int menuId = int.Parse((string)filterContext.RouteData.Values["menuId"]); // Assuming you're getting the id from the URL

        var context = filterContext.HttpContext.RequestServices.GetService(typeof(AkalaatDbContext)) as AkalaatDbContext;

      
            var menu = context.Set<Menu>().Find(menuId); // Assuming you have a DbSet<Menu> in your dbContext

           

            var currentUserId = filterContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier); // Get the currently logged-in user
            var menuVendorId = (menu!=null)?context.Vendors.Where(v=>v.Resturant_ID==menu.Resturant_ID).FirstOrDefault()?.Id??null:null; 

            if (menuVendorId != currentUserId?.Value)
            {
                var currentVendor = context.Vendors.Find(currentUserId?.Value);
                var currMenuId = context.Set<Menu>().Where(m =>currentVendor.Resturant_ID== m.Resturant_ID).FirstOrDefault()?.Id??0;
                filterContext.Result = new RedirectToActionResult("Index","Item",new {menuID=currMenuId}); // If the menu does not belong to the current user, return a 401
                return;
            }
            if (menu == null)
            {
                filterContext.Result = new NotFoundResult(); // If the menu doesn't exist, return a 404
                return;
            }
        

        base.OnActionExecuting(filterContext);
    }
}