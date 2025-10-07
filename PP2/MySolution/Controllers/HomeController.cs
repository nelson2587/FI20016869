using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Services;

namespace MySolution.Controllers;

public class HomeController : Controller
{
    private readonly BinaryService _svc = new();

    [HttpGet]
    public IActionResult Index()
    {
        return View(new BinaryPageViewModel { Input = new BinaryInput() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(BinaryInput input)
    {
        var vm = new BinaryPageViewModel { Input = input };

        if (!ModelState.IsValid)
        {
            // Muestra el formulario con mensajes de error
            return View(vm);
        }

        vm.Results = _svc.Compute(input.A, input.B);
        return View(vm);
    }
}
