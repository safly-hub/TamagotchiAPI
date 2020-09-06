using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TamogotchiAPI.Models;
namespace TamogotchiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public PetsController(DatabaseContext context)
        {
            _context = context;
        }
        private async Task<Pet> FindAsync(int petId)
        {
            return await _context.Pets.FirstOrDefaultAsync(pet => pet.PetId == petId);
        }
        [HttpGet]
        private async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            return await _context.Pets.OrderBy(row => row.PetId).ToListAsync();
        }
        [HttpGet("{petId}")]
        public async Task<ActionResult<Pet>> GetPet(int petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }
            return pet;
        }
        [HttpPost("/CreatePet")]
        public async Task<ActionResult<Pet>> PostNewPetAsync(Pet petToCreate)
        {
            petToCreate.Birthday = DateTime.Now;
            petToCreate.HungerLevel = 0;
            petToCreate.HappinessLevel = 0;
            _context.Pets.Add(petToCreate);
            await _context.SaveChangesAsync();
            return CreatedAtAction(null, null, petToCreate);
        }
        [HttpDelete("{petId}")]
        public async Task<ActionResult<Pet>> DeletePetByIdAsync(int petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return Ok(pet);
        }
        [HttpPost("{petId}/Playtime")]
        public async Task<ActionResult<Pet>> PostPlayPetByIdAsync(int petId, Playtime playtime)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }
            playtime.When = DateTime.Now;
            playtime.PetId = pet.PetId;
            _context.Playtimes.Add(playtime);
            pet.HappinessLevel += 5;
            pet.HungerLevel += 3;
            await _context.SaveChangesAsync();
            return Ok(pet);
        }
        [HttpPost("{petId}/Feeding")]
        public async Task<ActionResult<Pet>> PostFeedPetByIdAsync(int petId, Feeding feeding)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }
            feeding.When = DateTime.Now;
            feeding.PetId = pet.PetId;
            _context.Feedings.Add(feeding);
            pet.HappinessLevel += 3;
            pet.HungerLevel -= 5;
            await _context.SaveChangesAsync();
            return Ok(pet);
        }
        [HttpPost("{petId}/Scolding")]
        public async Task<ActionResult<Pet>> PostScoldPetByIdAsync(int petId, Scolding scolding)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }
            scolding.When = DateTime.Now;
            scolding.PetId = pet.PetId;
            _context.Scoldings.Add(scolding);
            pet.HappinessLevel -= 5;
            await _context.SaveChangesAsync();
            return Ok(pet);
        }
    }
}