using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Report.Domain.ViewModels;
using Report.Services.Contracts;


[Authorize(Roles = "1,2")]
public class UserManagementController : Controller
{
    private readonly IUserManagementService _service;


    public UserManagementController(IUserManagementService service)
    {
        _service = service;
    }


    public async Task<IActionResult> Index(string search = "", string role = "", string status = "")
    {
        var users = await _service.GetUsersAsync(search, role, status);
        return View(users);
    }

    #region UpdateUserInfo
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetUserForEditAsync(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.UpdateUserAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model); 
        }
    }




    #endregion

    #region DelateUser
    [Authorize(Roles = "1")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
    #endregion


}
