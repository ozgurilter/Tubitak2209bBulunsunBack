using DemoSc.Data;
using DemoSc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

[ApiController]
[Route("api/scooters")]
public class ScooterController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ScooterController(ApplicationDbContext context)
    {
        _context = context;
    }

    private void TestMySqlConnection()
    {
        string connString = @"Server=localhost;Port=3306;Database=scooterdb;user=root;password=admin123;";

        using (var connection = new MySqlConnection(connString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Bağlantı başarılı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bağlantı hatası: {ex.Message}");
            }
        }
    }

    // GET: api/scooters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Scooter>>> GetScooters()
    {
        return await _context.Scooters.ToListAsync();
    }

    // GET: api/scooters/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Scooter>> GetScooter(int id)
    {
        var scooter = await _context.Scooters.FindAsync(id);

        if (scooter == null)
        {
            return NotFound();
        }

        return scooter;
    }

    // PUT: api/scooters/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateScooter(int id, Scooter scooter)
    {
        if (id != scooter.Id)
        {
            return BadRequest();
        }

        _context.Entry(scooter).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ScooterExists(id))
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

    // PUT: api/scooters/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> ToggleScooterStatus(int id)
    {
        var scooter = await _context.Scooters.FindAsync(id);

        if (scooter == null)
        {
            return NotFound();
        }

        scooter.IsEnabled = !scooter.IsEnabled;
        await _context.SaveChangesAsync();

        return Ok(scooter);
    }


    // PUT: api/scooters/{id}/location
    [HttpPut("{id}/location")]
    public async Task<IActionResult> UpdateScooterLocation(int id, [FromBody] LocationUpdateRequest request)
    {
        var scooter = await _context.Scooters.FindAsync(id);
        if (scooter == null)
        {
            return NotFound("Scooter bulunamadı");
        }

        try
        {
            scooter.Latitude = request.Latitude;
            scooter.Longitude = request.Longitude;

            await _context.SaveChangesAsync();
            return Ok(scooter);
        }
        catch (Exception ex)
        {
            return BadRequest($"Konum güncellenirken hata oluştu: {ex.Message}");
        }
    }

    // Location güncellemesi için request modeli
    public class LocationUpdateRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    private bool ScooterExists(int id)
    {
        return _context.Scooters.Any(e => e.Id == id);
    }



}
