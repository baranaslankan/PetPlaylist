using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPlaylist.Data;

namespace PetPlaylist.Controllers
{
    public class OwnerWebController : Controller
    {
        private readonly Services.OwnerService _ownerService;
        public OwnerWebController(Services.OwnerService ownerService) => _ownerService = ownerService;

        public async Task<IActionResult> Details(int id)
        {
            var owner = await _ownerService.GetOwnerWithPetsAsync(id);
            if (owner == null) return NotFound();
            return View(owner);
        }
    }
}
