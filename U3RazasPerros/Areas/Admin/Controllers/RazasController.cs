using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;
using U3RazasPerros.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace U3RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RazasController : Controller
    {
        public perrosContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public RazasController(perrosContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }



        [Route("admin/Razas")]
        [Route("admin/Razas/Index")]
        //[Route("admin/")]
        public IActionResult Index()
        {
            var razas = Context.Razas.OrderBy(x => x.Nombre);

            return View(razas);
        }
        [HttpGet]
        public IActionResult Agregar()
        {
            RazasAgregarViewModel vm = new RazasAgregarViewModel();
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(RazasAgregarViewModel vm, IFormFile archivo1)
        {
            if (string.IsNullOrWhiteSpace(vm.Razas.Nombre))
            {
                ModelState.AddModelError("", "Todas las razas de perros debe de tener un nombre");
                //vm.Paises = Context.Paises.OrderBy(x => x.Nombre);

            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Descripcion))
            {
                ModelState.AddModelError("", "La descripción del dogo no puede estar en blanco, por favor describe a tu dogo uwu");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.OtrosNombres))
            {
                ModelState.AddModelError("", "No se puede dejar en blanco, los dogos deben de tener otro nombre.");
            }
            else if (vm.Razas.PesoMin > vm.Razas.PesoMax)
            {
                ModelState.AddModelError("", "El peso mínimo del perro no puede ser mayor al peso máximo");
            }
            else if (vm.Razas.PesoMax < vm.Razas.PesoMin)
            {
                ModelState.AddModelError("", "El peso máximo del perro no puede ser menor al peso mínimo");
            }
            else if (vm.Razas.PesoMin == vm.Razas.PesoMax || vm.Razas.PesoMax == vm.Razas.PesoMin)
            {
                ModelState.AddModelError("", "El peso mínimo y el peso máximo no pueden ser iguales");
            }
            else if (vm.Razas.AlturaMin > vm.Razas.AlturaMax)
            {
                ModelState.AddModelError("", "La altura mínima del perro no puede ser mayor a la altura maxima");
            }
            else if (vm.Razas.AlturaMax < vm.Razas.AlturaMin)
            {
                ModelState.AddModelError("", "La altura máxima no puede ser menor ser menor a la altura mínima del perro");
            }
            else if (vm.Razas.EsperanzaVida == 0)
            {
                ModelState.AddModelError("", "La esperanza de vida no puede ser 0 años. ");
            }
            else if (vm.Razas.EsperanzaVida >= 100)
            {
                ModelState.AddModelError("", "La esperanza de vida no puede ser mayor o igual a 100 años");
            }
            else if (vm.Razas.AlturaMin == vm.Razas.AlturaMax && vm.Razas.AlturaMax == vm.Razas.AlturaMin)
            {
                ModelState.AddModelError("", "La altura mínima y la altura máxima no pueden ser iguales.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Patas))
            {
                ModelState.AddModelError("", "El campo patas no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Cola))
            {
                ModelState.AddModelError("", "El campo cola no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Hocico))
            {
                ModelState.AddModelError("", "El campo Hocico no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Pelo))
            {
                ModelState.AddModelError("", "El campo pelo no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Color))
            {
                ModelState.AddModelError("", "El campo color no puede estar en blanco.");
            }
            else
            {

                if (archivo1 != null)
                {
                    if (archivo1.ContentType != "image/jpeg") //tipo mime
                    {
                        ModelState.AddModelError("", "Solo se permite la carga de imagen en formato jpg");
                        return View(vm);
                    }
                }
                Context.Add(vm.Razas);
                Context.SaveChanges();
                if (archivo1 != null)
                {
                    var path = Host.WebRootPath + "/imgs_perros/" + vm.Razas.Id + "_0.jpg";
                    FileStream fs = new FileStream(path, FileMode.Create);
                    archivo1.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);

        }

        #region Editar
        [HttpGet]
        public IActionResult Editar(int id)
        {
            RazasAgregarViewModel vm = new RazasAgregarViewModel();
            var razas = Context.Razas.FirstOrDefault(x => x.Id == id);
            var caracteristicas = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == id);
            if (razas == null)
            {
                RedirectToAction("Index");
            }
            vm.Razas = razas;
            vm.Caracteristicasfisicas = caracteristicas;
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(RazasAgregarViewModel vm, IFormFile archivo1)
        {
            if (string.IsNullOrWhiteSpace(vm.Razas.Nombre))
            {
                ModelState.AddModelError("", "Todas las razas de perros debe de tener un nombre");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Descripcion))
            {
                ModelState.AddModelError("", "La descripción del dogo no puede estar en blanco, por favor describe a tu dogo uwu");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.OtrosNombres))
            {
                ModelState.AddModelError("", "No se puede dejar en blanco, los dogos deben de tener otro nombre.");
            }
            else if (vm.Razas.PesoMin > vm.Razas.PesoMax)
            {
                ModelState.AddModelError("", "El peso mínimo del perro no puede ser mayor al peso máximo");
            }
            else if (vm.Razas.PesoMax < vm.Razas.PesoMin)
            {
                ModelState.AddModelError("", "El peso máximo del perro no puede ser menor al peso mínimo");
            }
            else if (vm.Razas.PesoMin == vm.Razas.PesoMax || vm.Razas.PesoMax == vm.Razas.PesoMin)
            {
                ModelState.AddModelError("", "El peso mínimo y el peso máximo no pueden ser iguales");
            }
            else if (vm.Razas.AlturaMin > vm.Razas.AlturaMax)
            {
                ModelState.AddModelError("", "La altura mínima del perro no puede ser mayor a la altura maxima");
            }
            else if (vm.Razas.AlturaMax < vm.Razas.AlturaMin)
            {
                ModelState.AddModelError("", "La altura máxima no puede ser menor ser menor a la altura mínima del perro");
            }
            else if (vm.Razas.EsperanzaVida == 0)
            {
                ModelState.AddModelError("", "La esperanza de vida no puede ser 0 años. ");
            }
            else if (vm.Razas.EsperanzaVida >= 100)
            {
                ModelState.AddModelError("", "La esperanza de vida no puede ser mayor o igual a 100 años");
            }
            else if (vm.Razas.AlturaMin == vm.Razas.AlturaMax && vm.Razas.AlturaMax == vm.Razas.AlturaMin)
            {
                ModelState.AddModelError("", "La altura mínima y la altura máxima no pueden ser iguales.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Patas))
            {
                ModelState.AddModelError("", "El campo patas no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Cola))
            {
                ModelState.AddModelError("", "El campo cola no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Hocico))
            {
                ModelState.AddModelError("", "El campo Hocico no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Pelo))
            {
                ModelState.AddModelError("", "El campo pelo no puede estar en blanco.");
            }
            else if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Color))
            {
                ModelState.AddModelError("", "El campo color no puede estar en blanco.");
            }
            else
            {
                if (archivo1 != null)
                {
                    if (archivo1.ContentType != "image/jpeg") //tipo mime
                    {
                        ModelState.AddModelError("", "Solo se permite la carga de imagen en formato jpg");
                        return View(vm);
                    }
                }

                var Editarcat = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == vm.Razas.Id);
                var EditarRas = Context.Razas.FirstOrDefault(x => x.Id == vm.Razas.Id);
                if (EditarRas == null || Editarcat == null)
                {
                    RedirectToAction("Index");
                }

                EditarRas.IdPais = vm.Razas.IdPais;
                EditarRas.Nombre = vm.Razas.Nombre;
                EditarRas.Descripcion = vm.Razas.Descripcion;
                EditarRas.OtrosNombres = vm.Razas.OtrosNombres;
                EditarRas.PesoMin = vm.Razas.PesoMin;
                EditarRas.PesoMax = vm.Razas.PesoMax;
                EditarRas.AlturaMax = vm.Razas.AlturaMax;
                EditarRas.AlturaMin = vm.Razas.AlturaMin;
                EditarRas.EsperanzaVida = vm.Razas.EsperanzaVida;
                Editarcat.Patas = vm.Razas.Caracteristicasfisicas.Patas;
                Editarcat.Cola = vm.Razas.Caracteristicasfisicas.Cola;
                Editarcat.Hocico = vm.Razas.Caracteristicasfisicas.Hocico;
                Editarcat.Pelo = vm.Razas.Caracteristicasfisicas.Pelo;
                Editarcat.Color = vm.Razas.Caracteristicasfisicas.Color;
                Context.Update(EditarRas);
                Context.Update(Editarcat);
                Context.SaveChanges();

                if (archivo1 != null)
                {
                    var path = Host.WebRootPath + "/imgs_perros/" + vm.Razas.Id + "_0.jpg";
                    FileStream fs = new FileStream(path, FileMode.Create);
                    archivo1.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);

        }
        #endregion

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var raza = Context.Razas.FirstOrDefault(x => x.Id == id);
            var caract = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == id);

            if (raza == null)
            {
                return RedirectToAction("Index");
            }
            return View(raza);

        }

        [HttpPost]
        public IActionResult Eliminar(Razas r, Caracteristicasfisicas c)
        {
            var raza = Context.Razas.FirstOrDefault(x => x.Id == r.Id);
            var carac = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == c.Id);
            if (raza == null)
            {
                ModelState.AddModelError("", "La raza ha sido eliminada");
            }
            else
            {
                Context.Remove(carac);
                Context.Remove(raza);
                Context.SaveChanges();
                var foto = Host.WebRootPath + "/imgs_perros/" + raza.Id + "_0.jpg";
                if (System.IO.File.Exists(foto))
                {
                    System.IO.File.Delete(foto);
                }
                return RedirectToAction("Index");
            }
            return View(r);
        }
    }
}
