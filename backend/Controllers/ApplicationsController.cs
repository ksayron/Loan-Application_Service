using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriInkom.DataBase;
using TriInkom.DTO;
using TriInkom.Entities;

namespace TriInkom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {

        private readonly LoanDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(LoanDbContext ctx, IMapper mapper, ILogger<ApplicationsController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// GET /api/applications
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ApplicationFilterDto filter)
        {
            var dbQuery = _ctx.Applications.AsQueryable();

            if (filter.Status.HasValue)
                dbQuery = dbQuery.Where(x => x.Status == filter.Status.Value);

            if (filter.MinAmount.HasValue)
                dbQuery = dbQuery.Where(x => x.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                dbQuery = dbQuery.Where(x => x.Amount <= filter.MaxAmount.Value);

            if (filter.MinTerm.HasValue)
                dbQuery = dbQuery.Where(x => x.TermValue >= filter.MinTerm.Value);

            if (filter.MaxTerm.HasValue)
                dbQuery = dbQuery.Where(x => x.TermValue <= filter.MaxTerm.Value);

            var list = await dbQuery.OrderByDescending(x => x.CreatedAt).ToListAsync();
            var dto = _mapper.Map<List<ApplicationDto>>(list);
            return Ok(dto);
        }

        /// <summary>
        /// GET /api/applications/{number}
        /// </summary>
        [HttpGet("{number}")]
        public async Task<IActionResult> GetOne(string number)
        {
            var entity = await _ctx.Applications.FindAsync(number);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ApplicationDto>(entity));
        }

        /// <summary>
        /// POST /api/applications
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApplicationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Number))
                return BadRequest(new { message = "Номер заявки обязательно должен быть указан." });

            var exists = await _ctx.Applications.AnyAsync(x => x.Number == dto.Number);
            if (exists)
                return Conflict(new { message = "Заявка с таким номером уже существует." });

            var entity = _mapper.Map<Application>(dto);
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.ModifiedAt = DateTimeOffset.UtcNow;
            entity.Status = EApplicationStatus.Published;

            _ctx.Applications.Add(entity);
            await _ctx.SaveChangesAsync();

            var outDto = _mapper.Map<ApplicationDto>(entity);
            return CreatedAtAction(nameof(GetOne), new { number = outDto.Number }, outDto);
        }

        /// <summary>
        /// PATCH /api/applications/{number}/status
        /// </summary>
        [HttpPatch("{number}/status")]
        public async Task<IActionResult> ToggleStatus(string number)
        {
            var entity = await _ctx.Applications.FindAsync(number);
            if (entity == null) return NotFound();

            entity.Status = entity.Status == EApplicationStatus.Published
                ? EApplicationStatus.Unpublished
                : EApplicationStatus.Published;

            entity.ModifiedAt = DateTimeOffset.UtcNow;
            await _ctx.SaveChangesAsync();

            return Ok(_mapper.Map<ApplicationDto>(entity));
        }

    }
}
