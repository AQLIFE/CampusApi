using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using ApiDB.Comon;
using ApiDB.Model;

namespace ApiDB.Controller
{
    [EnableCors]
    [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly WebContext _context;

        public PartsController(WebContext context)
        {
            _context = context;
        }

        // GET: api/Parts =>获得所有的数据
        [HttpGet]
        [Authorize(Roles = "999")]
        public async Task<ActionResult<IEnumerable<Parts>>> GetPart()
        {
            if (_context.Part == null)
            {
                return NotFound();
            }
            return await _context.Part.ToListAsync();
        }

        // GET: api/Parts/5 => 获得指定数据
        [HttpGet("{id}")]
        public async Task<ActionResult<Parts>> GetParts(int id)
        {
            if (_context.Part == null)
            {
                return NotFound();
            }
            Parts parts = await _context.Part.FindAsync(id);

            if (parts == null)
            {
                return NotFound();
            }

            return parts;
        }

        // PUT: api/Parts/5 => 更新指定数据
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParts(int id, Parts parts)
        {
            if (id != parts.Id)
            {
                return BadRequest();
            }

            _context.Entry(parts).State = EntityState.Modified;
            // 正确写法 => 设置 EF 操作为 一个操作枚举量

            try
            {
                await _context.SaveChangesAsync();//异步缓存修改
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartsExists(id))
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

        // POST: api/Parts => 新增 Part 用户
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parts>> PostParts(Parts parts)
        {
            if (_context.Part == null)
            {
                return Problem("Entity set 'WebContext.Part'  is null.");
            }
            //_context.Part.Add(parts);
            _context.Entry(parts).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParts", new { id = parts.Id }, parts);
        }

        // DELETE: api/Parts/5 => 删除指定Part 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParts(int id)
        {
            if (_context.Part == null)
            {
                return NotFound();
            }
            var parts = await _context.Part.FindAsync(id);
            if (parts == null)
            {
                return NotFound();
            }

            _context.Part.Remove(parts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartsExists(int id)
        {
            return (_context.Part?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
