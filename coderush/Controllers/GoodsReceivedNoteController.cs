using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.GoodsReceivedNote.RoleName)]
    public class GoodsReceivedNoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GoodsReceivedNoteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            GoodsReceivedNote goodsReceivedNote = _context.GoodsReceivedNote.SingleOrDefault(x => x.GoodsReceivedNoteId.Equals(id));

            if (goodsReceivedNote == null)
            {
                return NotFound();
            }

            return View(goodsReceivedNote);
        }
    }
}