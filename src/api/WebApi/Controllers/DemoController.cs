using System.Linq.Expressions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly JuanKataContext _context;

    public DemoController(JuanKataContext context)
    {
        _context = context;
        var e = FindEmployee(1);
    }

    [HttpGet("/{id}")]
    public async Task<ActionResult<Employee>> FindEmployee(int id)
    {
        var e = await _context.Employees
            .Include(e => e.Team)
            .Include(e => e.Specializations)
            .FirstOrDefaultAsync(e => e.Id == id);

        return e is null
            ? NotFound()
            : Ok(e);
    }

    [HttpGet]
    public async Task<IEnumerable<Employee>> Get()
    {
        var employees = await _context.Employees.ToArrayAsync();

        return employees;
    }

    [HttpPost]
    public async Task<Employee> CreateEmployee(string employeeName, string mail, DateTime employeeSince, string teamName, string specializationName)
    {
        var team = await FindOrCreate(
            _context.Teams,
            e => e.Name == teamName,
            e => e.Id,
            id => new Team {Id = id, Name = teamName});

        var specialization = await FindOrCreate(
            _context.Specializations,
            e => e.Name == specializationName,
            e => e.Id,
            id => new Specialization {Id = id, Name = specializationName});

        var employee = new Employee
        {
            Id = await NextId(_context.Employees, e => e.Id),
            Name = employeeName,
            Mail = mail,
            EmployeeSince = employeeSince,
            Team = team,
            Specializations = new[] { specialization }
        };

        _ = await _context.AddAsync(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    private static async Task<int> NextId<T>(IQueryable<T> set, Expression<Func<T, int>> idExpression) where T : class
        => await set.AnyAsync()
            ? await set.MaxAsync(idExpression) + 1
            : 1;

    private static async Task<T> FindOrCreate<T>(IQueryable<T> set, Expression<Func<T, bool>> query, Expression<Func<T, int>> idExpression, Func<int, T> factory) where T : class =>
        await set.FirstOrDefaultAsync(query) ?? factory(await NextId(set, idExpression));
}