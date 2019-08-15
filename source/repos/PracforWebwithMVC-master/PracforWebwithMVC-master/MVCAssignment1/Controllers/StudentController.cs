using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAssignment1;
using MVCAssignment1.Models;
using PagedList;

namespace MVCAssignment1.Controllers
{
    public class StudentController : Controller
    {
			private readonly SchoolContext _context;
            public IHostingEnvironment _hostingEnvironment;

			[BindProperty]
        public Student Student { get; set; }

			public StudentController(SchoolContext context, IHostingEnvironment hostingEnvironment) {
				_context = context;
                _hostingEnvironment = hostingEnvironment;
			}

			public async Task<IActionResult> List()
			{
				return View(
					await _context.Student.ToListAsync());
			}

			[HttpGet]
			public IActionResult Edit(int? id)
			{
					if (id == null)
					{
							return NotFound();
					}
					 Student = _context.Student.FirstOrDefault(m => m.ID == id);

          if (Student == null)
          {
              return NotFound();
          }

					return View(Student);
			}
			[HttpPost]
			public IActionResult OnPostEdit() 
			{
				if (!ModelState.IsValid)
            {
                return View();
            }
				_context.Attach(Student).State = EntityState.Modified;

				try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(Student.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View("List", _context.Student.ToList());
			}

			private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }

			public IActionResult Create()
			{
				return View();
			}

			[HttpPost]
			public IActionResult OnPostCreate()
			{
				if (!ModelState.IsValid)
            {
                return View();
            }

				_context.Student.Add(Student);
				_context.SaveChanges();
				return View("List", _context.Student.ToList());
			}
			
			public IActionResult Delete(int? id) {
				Student = _context.Student.FirstOrDefault(m => m.ID == id);
				return View(Student);
			}

			[HttpPost]
			public IActionResult OnPostDelete(int? id) {
				var user = _context.Student.Where(x => x.ID==id);
				_context.Student.RemoveRange(user);
				_context.SaveChanges();
				return View("List", _context.Student.ToList());
			}

			[HttpGet]
			public IActionResult Details(int? id) {
				Student = _context.Student.FirstOrDefault(m => m.ID == id);
				return View(Student);
			}
	
		
		public ViewResult OnPostSort(string sortOrder, string currentFilter, string searchString, int? page) {

			ViewBag.CurrentSort = sortOrder;
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
			
			if (searchString != null) {
				page = 1;
			}
			else {
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var students = from s in _context.Student
										 select s;
			if (!String.IsNullOrEmpty(searchString)) {
				students = students.Where(s => s.LastName.Contains(searchString)
															 || s.FirstMidName.Contains(searchString));
			}
			switch (sortOrder) {
				case "name_desc":
					students = students.OrderByDescending(s => s.LastName);
					break;
				case "Date":
					students = students.OrderBy(s => s.EnrollmentDate);
					break;
				case "date_desc":
					students = students.OrderByDescending(s => s.EnrollmentDate);
					break;
				default:
					students = students.OrderBy(s => s.LastName);
					break;
			}

			int pageSize = 3;
			int pageNumber = (page ?? 1);
			return View("List", students.ToPagedList(pageNumber, pageSize));
		}

		[HttpPost]
		public IActionResult OnPostFile(int? id) {

			IFormFile file = Request.Form.Files[0];
			string folderName = "Uploads";
			string newPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, folderName);
			string sError = string.Empty;

			System.Text.StringBuilder sb = new StringBuilder();
			if (!System.IO.Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

			if (file.Length > 0) {
				string sFileExtension = Path.GetExtension(file.FileName).ToLower();

				string fullPath = Path.Combine(newPath, file.FileName);

				using (var stream = new FileStream(fullPath, FileMode.Create)) {
					file.CopyTo(stream);
         
				}
			}
            Student = _context.Student.FirstOrDefault(m => m.ID == id);

            return View("Edit", Student);
		}
		public IActionResult OnPost(int[] SelectItems) {
				var user = _context.Student.Where(x => SelectItems.Contains(x.ID));
				_context.Student.RemoveRange(user);
				_context.SaveChanges();
				return View();
			}
	}
}