﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeptApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace DeptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        private readonly DeptDbContext _context;

        public DeptController(DeptDbContext context)
        {
            _context = context;
        }

        // GET: api/Dept
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dept>>> GetDepartments()
        {
            return await _context.Departments
                                 .Include(d => d.Manager)
                                 .ToListAsync();
        }

        // GET: api/Dept/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dept>> GetDept(int id)
        {
            var dept = await _context.Departments
                                     .Include(d => d.Manager)
                                     .FirstOrDefaultAsync(d => d.DeptId == id);

            if (dept == null)
            {
                return NotFound();
            }

            return dept;
        }

        // POST: api/Dept
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost]
        public async Task<ActionResult<Dept>> PostDept(Dept dept)
        {
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDept), new { id = dept.DeptId }, dept);
        }

        // PUT: api/Dept/5
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDept(int id, Dept dept)
        {
            if (id != dept.DeptId)
            {
                return BadRequest();
            }

            _context.Entry(dept).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(e => e.DeptId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Dept/5
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDept(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
