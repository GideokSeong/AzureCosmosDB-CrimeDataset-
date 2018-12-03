using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using Phase3.Models;

namespace Phase3.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            
            var items = await DocumentDBRepository<Item>.GetItemAsync();
            if (items == null)
            {
                return HttpNotFound();
            }

            return View(items);
           
        }

        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(d => !d.Completed);
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Completed,Month,Type,Code,Location")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }
        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var items = await DocumentDBRepository<Item>.GetItemAsync();
            if (items == null)
            {
                return HttpNotFound();
            }

            return View(items);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Completed,Month,Type,Code,Location")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }




    }
}