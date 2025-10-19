using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Todo_App.DB;
using Todo_App.Models;

namespace Todo_App.Pages
{
    public class TodoModel : PageModel
    {
        private readonly TodoContext _todoContext;
        public TodoModel(TodoContext todoContext)
        {
            _todoContext = todoContext;
            if (_todoContext == null)
                throw new Exception("Error");
        }

        [BindProperty]
        public string? TodoText { get; set; }

        public IEnumerable<TodoDTO> GetTodos()
        {
            if (_todoContext.todo == null)
                return new List<TodoDTO>();
            return _todoContext.todo.ToList().OrderByDescending(x=>x.id);
        }
        public void OnGet()
        {
          
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(TodoText) || _todoContext.todo == null)
                return Page();

            await _todoContext.todo.AddAsync(new TodoDTO { text = TodoText });
            var id = _todoContext.SaveChanges();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (_todoContext.todo == null)
                return Page();

            var todo = await _todoContext.todo.FindAsync(id);
            if (todo == null)
                return Page();

            _todoContext.todo.Remove(todo);
            await _todoContext.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
