using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace leave_management.Controllers
{
    // [Authorize] // De kiem tra co login chua? chua thi truy cap de trang login
    [Authorize(Roles ="Administrator")] // vua checkLogin vua checkRole    
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [Authorize] // De kiem tra co login chua? chua thi truy cap de trang login
        // GET: LeaveTypesController
        public async Task<ActionResult> Index()
        {
            var leavetypes = await _repo.FindAll();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes.ToList());
            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var isExists = await _repo.isExists(id);
            if (!isExists) 
            {
                return NotFound();
            }

            var leavetype = await _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeVM>(leavetype);

            return View(model);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                leaveType.DateCreated = DateTime.Now;

                var isSuccess = await _repo.Create(leaveType);

                if (!isSuccess)
                {
                    ModelState.AddModelError("","Something Went Wrong...");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("","Something Went Wrong...");
                return View();
            }
        }

        // GET: LeaveTypesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var isExists = await _repo.isExists(id);
            if (!isExists)
            {
                return NotFound();
            }

            var leavetype = await _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeVM>(leavetype);

            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveType model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                var isSucess = await _repo.Update(leaveType);

                if (!isSucess)
                {
                    ModelState.AddModelError("", "Something Went Wrong...");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    if (!_repo.isExists(id))
        //    {
        //        return NotFound();
        //    }

        //    var leavetype = _repo.FindById(id);
        //    var model = _mapper.Map<LeaveTypeVM>(leavetype);

        //    return View(model);
        //}

        

        // POST: LeaveTypesController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, LeaveType model)
        //{
        //    try
        //    {
        //        var leavetype = _repo.FindById(id);

        //        if (leavetype == null)
        //        {
        //            return NotFound();
        //        }

        //        var isSuccess = _repo.Delete(leavetype);

        //        if (!isSuccess)
        //        {
        //            return View(model);
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View(model);
        //    }
        //}

        // GET + DELETE: LeaveTypesController/Delete/5 -> sua lai
        public async Task<ActionResult> Delete(int id)
        {
            var leavetype = await _repo.FindById(id);

            if (leavetype == null)
            {
                return NotFound();
            }

            var isSuccess = await _repo.Delete(leavetype);

            if (!isSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
