using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace TamagotchiAPI.Controllers
{
    [Route("api/[controller]“)]
    [ApiController]
    public class PetController : ControllerBase
    {
        [ApiController]
        [Route("[controller]")]
        public class PetController : ControllerBase
        {
            private readonly DatabaseContext _context;
            public PetController(DatabaseContext context)
            {
                _context = context;
            }
            private async Task<Pet> FindPetAsync(int id)
            {
                return await _context.Pets.FirstOrDefaultAsync(pet => pet.Id == id);
            }
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Pet>>> GetAllPetsAsync()
            {
                return Ok(await _context.Pets.ToListAsync());
            }
            [HttpGet("{id}")]
            public async Task<ActionResult<Pet>> GetPetByIdAsync(int id)
            {
                var selectedPet = await FindPetAsync(id);
                if (selectedPet == null)
                {
                    return NotFound();
                }
                return Ok(selectedPet);
            }
            [HttpPost("/Create_new_pet")]
            public async Task<ActionResult<Pet>> PostNewPetAsync(Pet petToCreate)
            {
                petToCreate.Birthday = DateTime.Now;
                petToCreate.HungerLevel = 0;
                petToCreate.HappinessLevel = 0;
                _context.Pets.Add(petToCreate);
                await _context.SaveChangesAsync();
                return CreatedAtAction(null, null, petToCreate);
            }
            [HttpPost("{id}/Playtime")]
            public async Task<ActionResult<Pet>> PostPlayPetByIdAsync(int id)
            {
                var selectedPet = await FindPetAsync(id);
                if (selectedPet == null)
                {
                    return NotFound();
                }
                selectedPet.HappinessLevel += 5;
                selectedPet.HungerLevel += 3;
                await _context.SaveChangesAsync();
                return Ok(selectedPet);
            }
            [HttpPost("{id}/Feeding")]
            public async Task<ActionResult<Pet>> PostFeedPetByIdAsync(int id)
            {
                var selectedPet = await FindPetAsync(id);
                if (selectedPet == null)
                {
                    return NotFound();
                }
                selectedPet.HappinessLevel += 3;
                selectedPet.HungerLevel -= 5;
                if (selectedPet.HungerLevel < 0)
                {
                    selectedPet.HungerLevel = 0;
                }
                await _context.SaveChangesAsync();
                return Ok(selectedPet);
            }
            [HttpPost("{id}/Scolding")]
            public async Task<ActionResult<Pet>> PostScoldPetByIdAsync(int id)
            {
                var selectedPet = await FindPetAsync(id);
                if (selectedPet == null)
                {
                    return NotFound();
                }
                selectedPet.HappinessLevel -= 5;
                if (selectedPet.HappinessLevel < 0)
                {
                    selectedPet.HappinessLevel = 0;
                }
                await _context.SaveChangesAsync();
                return Ok(selectedPet);
            }
            [HttpDelete("{id}")]
            public async Task<ActionResult<Pet>> DeletePetByIdAsync(int id)
            {
                var selectedPet = await FindPetAsync(id);
                if (selectedPet == null)
                {
                    return NotFound();
                }
                _context.Pets.Remove(selectedPet);
                await _context.SaveChangesAsync();
                return Ok(selectedPet);
            }
        }
    }
}